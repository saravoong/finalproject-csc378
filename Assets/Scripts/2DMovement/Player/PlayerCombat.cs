using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GridBased gridBasedMovement;
    public Animator animator; 
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
        animator.SetBool("Attacking", true);
        
        Vector3 targetPos = frontFacingCollider.bounds.center;
        
        Collider2D enemyHit = Physics2D.OverlapBox(targetPos, cellSize, 0f, enemyLayer);
        if (enemyHit != null && enemyHit.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = enemyHit.GetComponent<EnemyHealth>();
            BossHealth bossHealth = enemyHit.GetComponent<BossHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            } else if (bossHealth != null)
            {
                bossHealth.TakeDamage(1);
            }
        }
    }
}
