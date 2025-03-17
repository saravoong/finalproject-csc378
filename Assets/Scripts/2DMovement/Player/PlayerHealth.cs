using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int absoluteMaxHealth = 10; // Maximum number of hearts the player can have
    private int currentHealth;
    public Transform respawnPoint;
    
    // Event that will be triggered when health changes
    public event Action OnHealthChanged;
    
    public int CurrentHealth { get { return currentHealth; } }
    public int MaxHealth { get { return maxHealth; } }
    
    void Start()
    {
        currentHealth = maxHealth;
        if(respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        Debug.Log("Player Health: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Health now: " + currentHealth);
        
        // Notify listeners that health has changed
        OnHealthChanged?.Invoke();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool AddHealth(int amount)
    {
        if (maxHealth >= absoluteMaxHealth)
        {
            Debug.Log("Player already at maximum health capacity!");
            return false;
        }
        
        // Increase max health
        maxHealth = Mathf.Min(maxHealth + amount, absoluteMaxHealth);
        
        // Also heal the player
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        
        Debug.Log("Player max health increased to: " + maxHealth + ", current health: " + currentHealth);
        
        // Notify listeners that health has changed
        OnHealthChanged?.Invoke();
        
        return true;
    }

    void Die()
    {
        Debug.Log("Player has died.");
        Respawn();
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        if(respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        Debug.Log("Player respawned. Health restored to " + currentHealth);
        
        // Notify listeners that health has changed
        OnHealthChanged?.Invoke();
    }
}