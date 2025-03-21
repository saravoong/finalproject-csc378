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
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
        }
    }
    
    void OnDestroy()
    {
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
        
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }
        
        hearts = new Image[playerHealth.absoluteMaxHealth];
        
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
        
        for (int i = 0; i < hearts.Length; i++)
        {
            Debug.Log("CH: " + playerHealth.CurrentHealth);
            hearts[i].enabled = (i < playerHealth.CurrentHealth);
            Debug.Log("HELLO" + i);
            Debug.Log("HELLO" + hearts[i].enabled);
            hearts[i].color = Color.white;
        }
    }
}