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
                StartCoroutine(Move(input));
            }
        }
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
