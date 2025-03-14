using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GridBased gridBasedMovement;
    public Animator animator; 
    public SpriteRenderer spriteRenderer;
    public LayerMask enemyLayer; 
    public Vector2 cellSize = new Vector2(0.9f, 0.9f); 
    public BoxCollider2D frontFacingCollider;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }
    
    void Attack()
    {
        Vector2 lastDir = gridBasedMovement.lastDir;
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
        
        Vector3 targetPos = frontFacingCollider.bounds.center;
        
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
}
