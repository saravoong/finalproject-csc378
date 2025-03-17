using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth = 3;
    public int absoluteMaxHealth = 10; // Maximum number of hearts the player can have
    private int currentHealth;
    public Transform respawnPoint;
    
    // Event that will be triggered when health changes
    public event Action OnHealthChanged;

    public PlayerHealthUI phUI;
    
    public int CurrentHealth { get { return currentHealth; } }
    public int StartHealth { get { return startHealth; } }
    
    void Start()
    {
        // Check if we have a GameManager with persisted health data
        if (GameManager.Instance != null)
        {
            // Set absolute max health to match the GameManager
            absoluteMaxHealth = GameManager.Instance.playerMaxHealth;
            
            // Let GameManager handle the initialization - it will call SetHealth()
            // GameManager will also call phUI.InitializeHearts()
        }
        else
        {
            // No GameManager found, use default initialization
            currentHealth = startHealth;
        }
        
        if(respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        
        Debug.Log("Player Health: " + currentHealth);
        phUI.InitializeHearts();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Health now: " + currentHealth);
        
        // Save health to GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerHealth(currentHealth);
        }
        
        // Notify listeners that health has changed
        OnHealthChanged?.Invoke();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool AddHealth(int amount)
    {
        if (currentHealth >= absoluteMaxHealth)
        {
            Debug.Log("Player already at maximum health capacity!");
            return false;
        }
        
        currentHealth = Mathf.Min(currentHealth + amount, absoluteMaxHealth);
        
        Debug.Log("Player health increased to: " + currentHealth);
        
        // Save health to GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerHealth(currentHealth);
        }
        
        // Notify listeners that health has changed
        OnHealthChanged?.Invoke();
        
        return true;
    }

    // New method to set health from GameManager
    public void SetHealth(int healthValue)
    {
        currentHealth = Mathf.Clamp(healthValue, 0, absoluteMaxHealth);
        
        Debug.Log($"Player health set to {currentHealth}");
        
        // Notify listeners that health has changed
        OnHealthChanged?.Invoke();
    }

    void Die()
    {
        Debug.Log("Player has died.");
        Respawn();
    }

    void Respawn()
    {
        currentHealth = startHealth;
        if(respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        Debug.Log("Player respawned. Health restored to " + currentHealth);
        
        // Save respawned health to GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerHealth(currentHealth);
        }
        
        // Notify listeners that health has changed
        OnHealthChanged?.Invoke();
    }
}