using UnityEngine;
using System.Collections;

public class ProjectileCleanup : MonoBehaviour
{
    private EnemyHealth ownerHealth;
    private float lifetime;
    private bool initialized = false;
    
    public void Initialize(EnemyHealth health, float projectileLifetime)
    {
        ownerHealth = health;
        lifetime = projectileLifetime;
        initialized = true;
        
        StartCoroutine(DestroyAfterLifetime());
    }
    
    void OnDestroy()
    {
        if (ownerHealth != null)
        {
            ownerHealth.UnregisterProjectile(gameObject);
        }
    }
    
    IEnumerator DestroyAfterLifetime()
    {
        if (!initialized) yield break;
        
        yield return new WaitForSeconds(lifetime);
        
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}