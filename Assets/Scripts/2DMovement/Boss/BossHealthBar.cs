using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Text bossNameText;
    public GameObject bossHealthPanel;
    
    public Color normalColor = Color.green;
    public Color vulnerableColor = Color.red;
    
    private BossHealth bossHealth;
    private Image fillImage;
    
    void Start()
    {
        if (healthSlider != null)
        {
            fillImage = healthSlider.fillRect.GetComponent<Image>();
            
            if (fillImage != null)
            {
                fillImage.color = normalColor;
            }
        }
    }
    
    public void SetBoss(BossHealth boss, string bossName = "Boss")
    {
        bossHealth = boss;
        
        if (boss != null)
        {
            boss.healthBar = healthSlider;
            
            if (bossHealthPanel != null)
                bossHealthPanel.SetActive(true);
                
            if (bossNameText != null)
                bossNameText.text = bossName;
        }
        
        SetNormalColor();
    }
    
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
    
    public void HideBossHealthBar()
    {
        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);
    }
}