using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public GridBased gridBased;
    public LayerMask collectibleLayer;
    public Vector2 cellSize = new Vector2(0.9f, 0.9f);
    public BoxCollider2D frontFacingCollider;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector2 direction = gridBased.lastDir;
            if (direction == Vector2.zero)
                direction = Vector2.up;
            
            Vector3 targetPos = frontFacingCollider.bounds.center;

            Collider2D hit = Physics2D.OverlapBox(targetPos, cellSize, 0f, collectibleLayer);
            if (hit != null && hit.CompareTag("Collectible"))
            {
                Debug.Log("Collected: " + hit.gameObject.name);
                Destroy(hit.gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (gridBased == null)
            return;
        
        Vector2 direction = gridBased.lastDir;
        if (direction == Vector2.zero)
            direction = Vector2.up;
        
        Vector3 targetPos = frontFacingCollider.bounds.center;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(targetPos, cellSize);
    }
}
