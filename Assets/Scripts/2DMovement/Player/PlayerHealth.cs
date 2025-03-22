using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth = 3;
    public int absoluteMaxHealth = 10;
    private int currentHealth;
    public Transform respawnPoint;
    
    [Header("Damage Flash Effect")]
    public float damageFlashDuration = 0.2f;
    public Color damageFlashColor = Color.red;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isFlashing = false;
    public GameObject dialogueObject;
    private bool inDialouge = false;
    
    public event Action OnHealthChanged;

    public PlayerHealthUI phUI;
    
    public int CurrentHealth { get { return currentHealth; } }
    public int StartHealth { get { return startHealth; } }
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
            else
            {
                Debug.LogWarning("No SpriteRenderer found on player or children. Damage flash effect will be disabled.");
            }
        }
    }
    
    void Start()
    {
        if (GameManager.Instance != null)
        {
            absoluteMaxHealth = GameManager.Instance.playerMaxHealth;
        }
        else
        {
            currentHealth = startHealth;
        }
        
        if(respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        
        Debug.Log("Player Health: " + currentHealth);
        phUI.InitializeHearts();
    }

    void Update() {
        string activeScene = SceneManager.GetActiveScene().name;
        if (activeScene == "forestScene") {
            if (dialogueObject != null && dialogueObject.activeSelf) {
                Debug.Log("Player currently in dialouge scene, cannot get hurt");
                inDialouge = true;
            }
        }

    }

    public void TakeDamage(int damage)
    {
        if (inDialouge) {
            return;
        }

        if (spriteRenderer != null && !isFlashing)
        {
            StartCoroutine(FlashDamage());
        }
        
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Health now: " + currentHealth);
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerHealth(currentHealth);
        }
        
        OnHealthChanged?.Invoke();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashDamage()
    {
        isFlashing = true;
        
        if (originalColor == Color.clear)
        {
            originalColor = spriteRenderer.color;
        }
        
        spriteRenderer.color = damageFlashColor;
        
        yield return new WaitForSeconds(damageFlashDuration);
        
        spriteRenderer.color = originalColor;
        
        isFlashing = false;
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
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerHealth(currentHealth);
        }
        
        OnHealthChanged?.Invoke();
        
        return true;
    }

    public void SetHealth(int healthValue)
    {
        currentHealth = Mathf.Clamp(healthValue, 0, absoluteMaxHealth);
        
        Debug.Log($"Player health set to {currentHealth}");
        
        OnHealthChanged?.Invoke();
    }

    void Die()
    {
        Debug.Log("Player has died.");
        
        if (DeathScreenManager.Instance != null)
        {
            DeathScreenManager.Instance.ShowDeathScreen();
        }
        else
        {
            Debug.LogWarning("DeathScreenManager not found. Reloading scene directly.");
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}