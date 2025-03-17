using System.Collections;
using UnityEngine;

public class BossEnemyAI : MonoBehaviour
{
    // Shooting cadence
    public float shootInterval = 3f;
    public float moveInterval = 3f;
    
    // Projectile settings
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float projectileLifetime = 2f;
    
    // Movement settings
    public float moveSpeed = 1f;
    private bool isMoving = false;
    private int currentMoveIndex = 0;
    public Transform upPosition;
    public Transform downPosition;
    
    // Left firing points
    public Transform firePoint_1_1;
    public Transform firePoint_1_2;
    public Transform firePoint_1_3;
    
    // Right firing points
    public Transform firePoint_3_1;
    public Transform firePoint_3_2;
    public Transform firePoint_3_3;

    // Up/Down firing points
    public Transform firePoint_2_1;
    public Transform firePoint_2_3;
    
    private Coroutine shootingCoroutine;
    private Coroutine movementCoroutine;
    private Coroutine currentMovementCoroutine; // Track the specific movement coroutine

    void Start()
    {
        // Start the boss behavior cycle (commented out as in your script)
        // StartCoroutine(ShootProjectiles());
        // StartCoroutine(MoveNextPosition());
    }

    public void StartAttacking()
    {
        StopAttacking(); // Clear any existing coroutines
        currentMoveIndex = 0;
        shootingCoroutine = StartCoroutine(ShootProjectiles());
        movementCoroutine = StartCoroutine(MoveNextPosition());
        Debug.Log("Boss started attacking");
    }

    public void StopAttacking()
    {
        // Stop shooting coroutine
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
        
        // Stop main movement coroutine
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
        
        // Stop the current movement directly
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
            currentMovementCoroutine = null;
        }
        
        // Ensure the boss is not flagged as moving anymore
        isMoving = false;
        
        Debug.Log("Boss stopped attacking exactly where it is");
    }

    public void TeleportTo(Vector3 position)
    {
        StopAttacking();
        transform.position = position;
        Debug.Log("Boss teleported to " + position);
    }

    IEnumerator ShootProjectiles()
    {
        while (true) {
            if (firePoint_1_1 != null)
                FireProjectile(firePoint_1_1.position, Vector2.left);
            
            if (firePoint_1_2 != null)
                FireProjectile(firePoint_1_2.position, Vector2.left);
            
            if (firePoint_1_3 != null)
                FireProjectile(firePoint_1_3.position, Vector2.left);
            
            // Fire from all right points
            if (firePoint_3_1 != null)
                FireProjectile(firePoint_3_1.position, Vector2.right);
            
            if (firePoint_3_2 != null)
                FireProjectile(firePoint_3_2.position, Vector2.right);
            
            if (firePoint_3_3 != null)
                FireProjectile(firePoint_3_3.position, Vector2.right);

            // Fire up and down
            if (firePoint_2_1 != null)
                FireProjectile(firePoint_2_1.position, Vector2.up);
            
            if (firePoint_2_3 != null)
                FireProjectile(firePoint_2_3.position, Vector2.down);

            yield return new WaitForSeconds(shootInterval);
        }
    }
    
    IEnumerator MoveNextPosition()
    {
        Vector3 targetPosition = Vector3.zero;
        
        while (true) {
            // Determine the next position based on the pattern
            switch (currentMoveIndex)
            {
                case 0: // Move up 1 unit
                    targetPosition = upPosition.position;
                    break;
                case 1: // Move down 1 unit
                    targetPosition = downPosition.position;
                    break;
                case 2: // Move down 1 unit
                    targetPosition = downPosition.position;
                    break;
                case 3: // Move up 1 unit
                    targetPosition = upPosition.position;
                    break;
            }
            
            // Store reference to the movement coroutine so we can stop it later
            currentMovementCoroutine = StartCoroutine(MoveToPosition(targetPosition));
            
            // Wait for movement to complete (or be canceled)
            while (isMoving)
            {
                yield return null;
            }
            
            // Update the movement index for next time
            currentMoveIndex = (currentMoveIndex + 1) % 4;
            
            // Wait for the "reload" time
            yield return new WaitForSeconds(moveInterval);
        }
    }
    
    IEnumerator MoveToPosition(Vector3 targetPos)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float timeToMove = 1f / moveSpeed;
        float elapsedTime = 0f;
        
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Only set the final position if we completed the full movement
        if (elapsedTime >= timeToMove)
        {
            transform.position = targetPos;
        }
        
        isMoving = false;
        currentMovementCoroutine = null;
    }
    
    void FireProjectile(Vector3 position, Vector2 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * projectileSpeed;
        }
        
        Destroy(projectile, projectileLifetime);
    }
}