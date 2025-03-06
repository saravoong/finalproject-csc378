using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public LayerMask collectibleLayer;
    public Vector2 cellSize = new Vector2(0.9f, 0.9f);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector2 direction = playerMovement.lastMoveDirection;
            if (direction == Vector2.zero)
                direction = Vector2.up;
            
            Vector3 targetPos = transform.position + new Vector3(direction.x, direction.y, 0);
            targetPos = new Vector3(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y), targetPos.z);

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
        if (playerMovement == null)
            return;
        
        Vector2 direction = playerMovement.lastMoveDirection;
        if (direction == Vector2.zero)
            direction = Vector2.up;
        
        Vector3 targetPos = transform.position + new Vector3(direction.x, direction.y, 0);
        targetPos = new Vector3(Mathf.Round(targetPos.x), Mathf.Round(targetPos.y), targetPos.z);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(targetPos, cellSize);
    }
}
