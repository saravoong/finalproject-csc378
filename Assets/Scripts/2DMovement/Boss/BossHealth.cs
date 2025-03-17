using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 30;
    private int currentHealth;
    
    [Header("UI References")]
    public Slider healthBar;
    public Text healthText;  // Optional, for showing numeric health
    
    [Header("Visual Feedback")]
    public GameObject deathEffect;  // Optional particle effect
    public float damageFlashDuration = 0.1f;
    
    private SpriteRenderer spriteRenderer;

    public BossHealthBar healthUI;
    public BossBattleManager bossBM;
    public GameObject GoldenStrawberry;
    public ParticleSystem sbParticle;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
            
        // Initialize the health bar UI
        UpdateHealthBar();
    }
    
    public void TakeDamage(int damage)
    {
        if(bossBM.IsVulnerable())
        {
            currentHealth -= damage;
            Debug.Log(gameObject.name + " took " + damage + " damage. Health now: " + currentHealth);
            
            // Visual feedback
            if (spriteRenderer != null)
                StartCoroutine(FlashDamage(spriteRenderer.color));
            
            // Update UI
            UpdateHealthBar();
            
            if (currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }
    
    System.Collections.IEnumerator FlashDamage(Color originalColor)
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }
    
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
        
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }
    
    IEnumerator Die()
    {
        Debug.Log(gameObject.name + " has been defeated!");
        // Spawn death effect if assigned
        spriteRenderer.enabled = false;
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        
        // You might want to trigger a game over or victory sequence here
        // Instead of destroying the boss, we could trigger a death animation
        // or disable the boss components
        
        // Example: disable components instead of destroying
        GetComponent<Collider2D>().enabled = false;
        GetComponent<BossEnemyAI>().enabled = false;
        GetComponent<BossBattleManager>().enabled = false;

        
        // Or you might want to trigger a specific death animation
        /*Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }*/
        
        // If you still want to destroy the GameObject after some time:
        
         // Destroy after 2 seconds
        yield return new WaitForSeconds(1f);
        healthUI.HideBossHealthBar();
        yield return new WaitForSeconds(1f);
        Instantiate(deathEffect, GoldenStrawberry.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        GoldenStrawberry.SetActive(true);
        Destroy(gameObject);
    }
}