using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float damageFlashDuration = 0.1f;
    
    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (spriteRenderer != null)
            StartCoroutine(FlashDamage());
        Debug.Log(gameObject.name + " took " + damage + " damage. Health now: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }
    
    void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);
    }
}
