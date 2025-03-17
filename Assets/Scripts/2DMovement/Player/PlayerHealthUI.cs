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

    public void InitializeHearts()
    {
        Debug.Log("HELLO1");
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
        }
        
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        Debug.Log("HELLO2");
        if (hearts == null || playerHealth == null)
            return;
        Debug.Log("HELLO3");
        // Update visibility of heart containers based on max health
        for (int i = 0; i < hearts.Length; i++)
        {
            Debug.Log("CH: " + playerHealth.CurrentHealth);
            // Only show hearts up to max health
            hearts[i].enabled = (i < playerHealth.CurrentHealth);
            Debug.Log("HELLO" + i);
            Debug.Log("HELLO" + hearts[i].enabled);
            hearts[i].color = Color.white;
        }
    }
}