using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Text bossNameText;
    public GameObject bossHealthPanel;
    
    // Colors for different states
    public Color normalColor = Color.green;
    public Color vulnerableColor = Color.red;
    
    private BossHealth bossHealth;
    private Image fillImage; // Reference to the slider's fill image
    
    void Start()
    {
        // Get the fill image from the slider
        if (healthSlider != null)
        {
            fillImage = healthSlider.fillRect.GetComponent<Image>();
            
            // Set initial color
            if (fillImage != null)
            {
                fillImage.color = normalColor;
            }
        }
    }
    
    // Call this method when the boss battle starts
    public void SetBoss(BossHealth boss, string bossName = "Boss")
    {
        bossHealth = boss;
        
        // Assign the boss health script to this health bar
        if (boss != null)
        {
            boss.healthBar = healthSlider;
            
            // Show the health bar
            if (bossHealthPanel != null)
                bossHealthPanel.SetActive(true);
                
            // Set boss name if provided
            if (bossNameText != null)
                bossNameText.text = bossName;
        }
        
        // Ensure color is set to normal
        SetNormalColor();
    }
    
    // Public methods to change the health bar color
    public void SetVulnerableColor()
    {
        if (fillImage != null)
        {
            fillImage.color = vulnerableColor;
        }
    }
    
    public void SetNormalColor()
    {
        if (fillImage != null)
        {
            fillImage.color = normalColor;
        }
    }
    
    // You can call this when the boss is defeated
    public void HideBossHealthBar()
    {
        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);
    }
}