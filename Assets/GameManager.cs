using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Player health data to persist between scenes
    public int playerCurrentHealth = 3;
    public int playerMaxHealth = 10;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager created as singleton.");
        }
        else
        {
            Debug.Log("GameManager already exists. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        // Subscribe to scene loading event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe when destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called automatically when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene '{scene.name}' loaded - Updating player health");
        
        // Find and update the player health in the new scene
        UpdatePlayerHealth();
    }

    // Update player health from GameManager's saved value
    public void UpdatePlayerHealth()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            // Initialize the health UI first
            if (playerHealth.phUI != null)
            {
                playerHealth.phUI.InitializeHearts();
            }
            
            // Set health to our persisted value and update UI
            playerHealth.SetHealth(playerCurrentHealth);
        }
        else
        {
            Debug.LogWarning("PlayerHealth component not found in scene!");
        }
    }

    // Save the current health value
    public void SavePlayerHealth(int currentHealth)
    {
        playerCurrentHealth = currentHealth;
        Debug.Log($"Saved player health: {playerCurrentHealth}");
    }
}