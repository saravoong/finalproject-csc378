using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    public GridBased gridBased;
    public PlayerHealth playerHealth;
    public LayerMask collectibleLayer;
    public Vector2 cellSize = new Vector2(0.9f, 0.9f);
    public BoxCollider2D frontFacingCollider;
    
    public int healthPerCollectible = 1;
    
    public AudioClip collectSound;
    public float collectSoundVolume = 1f;
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
            bool success = playerHealth.AddHealth(healthPerCollectible);
            AudioSource.PlayClipAtPoint(collectSound, hit.transform.position, collectSoundVolume);
            
            Destroy(hit.gameObject);
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