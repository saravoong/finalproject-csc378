using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image[] hearts;

    void Update()
    {
        int current = playerHealth.CurrentHealth;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = (i < current);
        }
    }
}
