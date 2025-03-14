using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float sprintMultiplier = 1.5f;
    public LayerMask obstacleLayers;
    public AudioClip blockedSound;
    public float blockedSoundVolume = 1f;
    public float blockedSoundCadence = 0.5f;
    private float lastBlockedSoundTime = -Mathf.Infinity;
    private bool isMoving = false;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    public Vector2 lastMoveDirection;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!isMoving)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.x != 0)
                input.y = 0;
            if (input != Vector2.zero)
            {
                lastMoveDirection = input;
                MoveToGridPosition(input);
            }
        }
    }

        // Call this from Update or other input handling method
    public void MovePlayer(Vector2 inputDirection)
    {
        if (inputDirection.magnitude < 0.1f)
            return;
            
        // Normalize for consistent movement speed
        inputDirection.Normalize();
        
        // Calculate the desired target position
        Vector3 targetPosition = transform.position + new Vector3(inputDirection.x, inputDirection.y, 0) * moveSpeed * Time.deltaTime;
        
        // Get the valid position that won't cause collisions
        Vector3 validPosition = GetValidMovementPosition(targetPosition);
        
        // Move to the valid position
        transform.position = validPosition;
    }

    public void MoveToGridPosition(Vector2 gridPosition)
    {
        Vector3 targetPosition = new Vector3(gridPosition.x, gridPosition.y, transform.position.z);
        
        // Get the valid position that won't cause collisions
        Vector3 validPosition = GetValidMovementPosition(targetPosition);
        
        // Move to the valid position
        transform.position = validPosition;
    }
    
    // Returns the furthest valid position the player can move without colliding
    public Vector3 GetValidMovementPosition(Vector3 targetPos)
    {
        Vector3 currentPosition = transform.position;
        
        // If target position is the same as current, no need to check
        if (Vector3.Distance(currentPosition, targetPos) < 0.01f)
            return currentPosition;
            
        Vector3 movementDirection = (targetPos - currentPosition).normalized;
        float moveDistance = Vector3.Distance(currentPosition, targetPos);
        
        // Create a contact filter for obstacles
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(obstacleLayers);
        filter.useTriggers = false;
        
        // Cast the collider along the movement path and get hits
        RaycastHit2D[] hits = new RaycastHit2D[10];
        int hitCount = boxCollider.Cast(movementDirection, filter, hits, moveDistance);
        
        // Start with the assumption we can move the full distance
        float maxAllowableDistance = moveDistance;
        
        // Check all hits and find the closest one
        for (int i = 0; i < hitCount; i++)
        {
            // Skip self-collisions
            if (hits[i].collider.gameObject != gameObject)
            {
                // If we found a closer obstacle, update the max distance
                if (hits[i].distance < maxAllowableDistance)
                {
                    maxAllowableDistance = hits[i].distance;
                }
            }
        }
        
        // Add a small buffer to prevent overlapping exactly with the obstacle
        // Adjust this value based on your game's scale and needs
        float collisionBuffer = 0.005f;
        maxAllowableDistance = Mathf.Max(0, maxAllowableDistance - collisionBuffer);
        
        // Calculate the final valid position
        return currentPosition + movementDirection * maxAllowableDistance;
    }
    
    // This replaces the old IsBlocked method
    public bool IsPathBlocked(Vector3 targetPos)
    {
        Vector3 validPos = GetValidMovementPosition(targetPos);
        
        // If the valid position differs significantly from the target,
        // then the path is considered blocked
        return Vector3.Distance(validPos, targetPos) > 0.01f;
    }

    IEnumerator Move(Vector2 direction)
    {
        isMoving = true;
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            currentSpeed *= sprintMultiplier;
        // Compute the grid center using transform.position plus the collider's offset.
        Vector3 gridCenter = transform.position + (Vector3)boxCollider.offset;
        Vector3 startPos = gridCenter;
        Vector3 targetPos = startPos + new Vector3(direction.x, direction.y, 0);


        animator.SetBool("WalkUp", false);
        animator.SetBool("WalkDown", false);
        animator.SetBool("WalkLeft", false);

        if (direction.y > 0)
        {
            animator.SetBool("WalkUp", true);
            spriteRenderer.flipX = false;
        }
        else if (direction.y < 0)
        {
            animator.SetBool("WalkDown", true);
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            animator.SetBool("WalkLeft", true);
            spriteRenderer.flipX = false;
        }
        else if (direction.x > 0)
        {
            animator.SetBool("WalkLeft", true);
            spriteRenderer.flipX = true;
        }

        if (IsBlocked(targetPos))
        {
            if (blockedSound != null && Time.time - lastBlockedSoundTime >= blockedSoundCadence)
            {
                AudioSource.PlayClipAtPoint(blockedSound, transform.position, blockedSoundVolume);
                lastBlockedSoundTime = Time.time;
            }
            animator.SetBool("WalkUp", false);
            animator.SetBool("WalkDown", false);
            animator.SetBool("WalkLeft", false);
            isMoving = false;
            yield break;
        }

        float timeToMove = 1f / currentSpeed;
        float elapsedTime = 0f;
        while (elapsedTime < timeToMove)
        {
            // Interpolate between startPos and targetPos (which are the desired grid centers).
            Vector3 newGridCenter = Vector3.Lerp(startPos, targetPos, elapsedTime / timeToMove);
            // Set transform.position so that the collider's center equals newGridCenter.
            transform.position = newGridCenter - (Vector3)boxCollider.offset;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos - (Vector3)boxCollider.offset;

        animator.SetBool("WalkUp", false);
        animator.SetBool("WalkDown", false);
        animator.SetBool("WalkLeft", false);
        isMoving = false;
    }

/*
    bool IsBlocked(Vector3 targetPos)
    {
        Vector2 size = boxCollider.bounds.size;
        Collider2D[] hits = Physics2D.OverlapBoxAll(targetPos, size, 0f, obstacleLayers);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }
    */
    bool IsBlocked(Vector3 targetPos)
    {
        // Calculate movement vector
        Vector3 currentPosition = transform.position;
        Vector3 movementDirection = (targetPos - currentPosition).normalized;
        float moveDistance = Vector3.Distance(currentPosition, targetPos);
        
        // Create a contact filter for obstacles
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(obstacleLayers);
        filter.useTriggers = false;
        
        // Cast the collider along the movement path and get hits
        RaycastHit2D[] hits = new RaycastHit2D[10];
        int hitCount = boxCollider.Cast(movementDirection, filter, hits, moveDistance);
        
        // Check if any obstacle was hit (excluding self)
        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.gameObject != gameObject)
            {
                // Calculate how far the player can actually move
                float distanceToObstacle = hits[i].distance;
                
                // If the obstacle is close enough to block movement completely
                if (distanceToObstacle < 0.01f)
                    return true;
                    
                // Adjust targetPos to stop right at the obstacle
                // You would use this modified position in your movement code
                Vector3 adjustedTargetPos = currentPosition + movementDirection * distanceToObstacle;
                
                // If we need to significantly adjust the position, consider movement blocked
                if (Vector3.Distance(targetPos, adjustedTargetPos) > 0.01f)
                    return true;
            }
        }
        
        return false;
    }


    void OnDrawGizmos()
    {
        if (boxCollider == null)
            return;

        // Draw the collider's current center bounds in green.
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);

        // If a move direction exists, draw the target overlap box in red.
        if (lastMoveDirection != Vector2.zero)
        {
            Vector3 targetPos = boxCollider.bounds.center + new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(targetPos, boxCollider.bounds.size);
        }
    }

}
