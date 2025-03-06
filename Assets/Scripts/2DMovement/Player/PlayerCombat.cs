using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator animator; 
    public SpriteRenderer spriteRenderer;
    public LayerMask enemyLayer; 
    public Vector2 cellSize = new Vector2(0.9f, 0.9f); 
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }
    
    void Attack()
    {
        Vector2 lastDir = playerMovement.lastMoveDirection;
        if (lastDir == Vector2.zero)
            lastDir = Vector2.down;
        
        if (lastDir.y > 0)
        {
            animator.SetTrigger("AttackUp");
            spriteRenderer.flipX = false;
        }
        else if (lastDir.y < 0)
        {
            animator.SetTrigger("AttackDown");
            spriteRenderer.flipX = false;
        }
        else if (lastDir.x < 0)
        {
            animator.SetTrigger("AttackLeft");
            spriteRenderer.flipX = false;
        }
        else if (lastDir.x > 0)
        {
            animator.SetTrigger("AttackLeft");
            spriteRenderer.flipX = true;
        }
        
        Vector3 targetPos = transform.position + new Vector3(lastDir.x, lastDir.y, 0);
        targetPos = new Vector3(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y), targetPos.z);
        
        Collider2D enemyHit = Physics2D.OverlapBox(targetPos, cellSize, 0f, enemyLayer);
        if (enemyHit != null && enemyHit.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = enemyHit.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }
        }
    }
    /*
    void OnDrawGizmosSelected()
    {
        if (playerMovement == null)
            return;
            
        Vector2 lastDir = playerMovement.lastMoveDirection;
        if (lastDir == Vector2.zero)
            lastDir = Vector2.down;
        
        Vector3 targetPos = transform.position + new Vector3(lastDir.x, lastDir.y, 0);
        targetPos = new Vector3(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y), targetPos.z);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetPos, cellSize);
    }
    */
}
