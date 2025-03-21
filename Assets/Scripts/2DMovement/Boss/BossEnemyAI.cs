using System.Collections;
using UnityEngine;

public class BossEnemyAI : MonoBehaviour
{
    public float shootInterval = 3f;
    public float moveInterval = 3f;
    
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float projectileLifetime = 2f;
    
    public float moveSpeed = 1f;
    private bool isMoving = false;
    private int currentMoveIndex = 0;
    public Transform upPosition;
    public Transform downPosition;
    
    public Transform firePoint_1_1;
    public Transform firePoint_1_2;
    public Transform firePoint_1_3;
    
    public Transform firePoint_3_1;
    public Transform firePoint_3_2;
    public Transform firePoint_3_3;

    public Transform firePoint_2_1;
    public Transform firePoint_2_3;
    
    private Coroutine shootingCoroutine;
    private Coroutine movementCoroutine;
    private Coroutine currentMovementCoroutine;

    void Start()
    {
    }

    public void StartAttacking()
    {
        StopAttacking();
        currentMoveIndex = 0;
        shootingCoroutine = StartCoroutine(ShootProjectiles());
        movementCoroutine = StartCoroutine(MoveNextPosition());
        Debug.Log("Boss started attacking");
    }

    public void StopAttacking()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
        
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
        
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
            currentMovementCoroutine = null;
        }
        
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
            
            if (firePoint_3_1 != null)
                FireProjectile(firePoint_3_1.position, Vector2.right);
            
            if (firePoint_3_2 != null)
                FireProjectile(firePoint_3_2.position, Vector2.right);
            
            if (firePoint_3_3 != null)
                FireProjectile(firePoint_3_3.position, Vector2.right);

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
            switch (currentMoveIndex)
            {
                case 0:
                    targetPosition = upPosition.position;
                    break;
                case 1:
                    targetPosition = downPosition.position;
                    break;
                case 2:
                    targetPosition = downPosition.position;
                    break;
                case 3:
                    targetPosition = upPosition.position;
                    break;
            }
            
            currentMovementCoroutine = StartCoroutine(MoveToPosition(targetPosition));
            
            while (isMoving)
            {
                yield return null;
            }
            
            currentMoveIndex = (currentMoveIndex + 1) % 4;
            
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
        
        if (elapsedTime >= timeToMove)
        {
            transform.position = targetPos;
        }
        
        isMoving = false;
        currentMovementCoroutine = null;
    }
    
    void FireProjectile(Vector3 position, Vector2 direction)
    {
        Quaternion rotation = Quaternion.identity;
        
        if (direction == Vector2.left)
        {
            rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (direction == Vector2.right)
        {
            rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2.up)
        {
            rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (direction == Vector2.down)
        {
            rotation = Quaternion.Euler(0, 0, 0);
        }
        
        GameObject projectile = Instantiate(projectilePrefab, position, rotation);
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * projectileSpeed;
        }
        
        Destroy(projectile, projectileLifetime);
    }
}