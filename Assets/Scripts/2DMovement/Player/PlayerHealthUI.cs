using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image heartPrefab;
    public Transform heartsContainer;
    private Image[] hearts;

    void Start()
    {
        // Initialize UI based on player's max health
        InitializeHearts();
        
        // Subscribe to health change events
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events when destroyed
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }

    void InitializeHearts()
    {
        if (playerHealth == null || heartPrefab == null || heartsContainer == null)
        {
            Debug.LogError("PlayerHealthUI is missing required references!");
            return;
        }
        
        // Clear any existing hearts
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Create array to hold heart images
        hearts = new Image[playerHealth.absoluteMaxHealth];
        
        // Instantiate heart images based on player's max health
        for (int i = 0; i < playerHealth.absoluteMaxHealth; i++)
        {
            Image newHeart = Instantiate(heartPrefab, heartsContainer);
            hearts[i] = newHeart;
            
            // Initially disable hearts beyond max health
            hearts[i].enabled = (i < playerHealth.MaxHealth);
            
            // Make hearts beyond current health appear inactive
            if (i >= playerHealth.CurrentHealth)
            {
                hearts[i].color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
        
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (hearts == null || playerHealth == null)
            return;
        
        // Update visibility of heart containers based on max health
        for (int i = 0; i < hearts.Length; i++)
        {
            // Only show hearts up to max health
            hearts[i].enabled = (i < playerHealth.MaxHealth);
            
            // Active hearts for current health
            if (hearts[i].enabled)
            {
                hearts[i].color = (i < playerHealth.CurrentHealth) 
                    ? Color.white  // Active heart
                    : new Color(0.5f, 0.5f, 0.5f, 0.5f);  // Inactive heart
            }
        }
    }
}