using System.Collections;
using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    public Transform player;                
    public Vector2 lineOfSightDirection = Vector2.right;
    public float lineOfSightDistance = 10f;  
    public LayerMask visionLayers;           
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float projectileSpeed = 5f; 
    
    private bool playerInSight = false;
    private Coroutine shootingCoroutine;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lineOfSightDirection.normalized, lineOfSightDistance, visionLayers);
        
        if (spriteRenderer != null && Mathf.Abs(lineOfSightDirection.x) > 0.1f)
        {
            spriteRenderer.flipX = lineOfSightDirection.x < 0;
        }
        
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (!playerInSight)
            {
                playerInSight = true;
                shootingCoroutine = StartCoroutine(ShootProjectiles());
            }
        }
        else
        {
            if (playerInSight)
            {
                playerInSight = false;
                if (shootingCoroutine != null)
                {
                    StopCoroutine(shootingCoroutine);
                    shootingCoroutine = null;
                }
            }
        }
    }

    IEnumerator ShootProjectiles()
    {
        while (true)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position + (Vector3)lineOfSightDirection.normalized;
            
            float rotationAngle = GetRotationAngleFromDirection(lineOfSightDirection);
            Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);
            
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, rotation);
            
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = lineOfSightDirection.normalized * projectileSpeed;
            }
            
            Destroy(projectile, 2f);
            
            yield return new WaitForSeconds(1f / fireRate);
        }
    }
    
    float GetRotationAngleFromDirection(Vector2 direction)
    {
        direction = direction.normalized;
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0)
                return -90f;
            else
                return 90f;
        }
        else
        {
            if (direction.y < 0)
                return 0f;
            else
                return 180f;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)lineOfSightDirection.normalized * lineOfSightDistance);
    }
}