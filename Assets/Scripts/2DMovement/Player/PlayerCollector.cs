using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public GridBased gridBased;
    public PlayerHealth playerHealth;
    public LayerMask collectibleLayer;
    public Vector2 cellSize = new Vector2(0.9f, 0.9f);
    public BoxCollider2D frontFacingCollider;
    
    // Amount of health to add for each collectible
    public int healthPerCollectible = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CollectItem();
        }
    }
    
    void CollectItem()
    {
        Vector2 direction = gridBased.lastDir;
        if (direction == Vector2.zero)
            direction = Vector2.up;
        
        Vector3 targetPos = frontFacingCollider.bounds.center;

        Collider2D hit = Physics2D.OverlapBox(targetPos, cellSize, 0f, collectibleLayer);
        if (hit != null && hit.CompareTag("Collectible"))
        {
            Debug.Log("Collected: " + hit.gameObject.name);
            
            // Try to add health to the player
            if (playerHealth != null)
            {
                bool success = playerHealth.AddHealth(healthPerCollectible);
                
                // Only destroy the collectible if it was successfully used
                if (success)
                {
                    // You could add effects or animations here
                    Destroy(hit.gameObject);
                }
                else
                {
                    Debug.Log("Player already has maximum health. Collectible not used.");
                }
            }
            else
            {
                // Destroy anyway if no playerHealth reference
                Destroy(hit.gameObject);
                Debug.LogError("PlayerHealth reference is missing in PlayerCollector!");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (gridBased == null || frontFacingCollider == null)
            return;
        
        Vector2 direction = gridBased.lastDir;
        if (direction == Vector2.zero)
            direction = Vector2.up;
        
        Vector3 targetPos = frontFacingCollider.bounds.center;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(targetPos, cellSize);
    }
}