using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public Transform respawnPoint;
    public int CurrentHealth { get { return currentHealth; } }
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
        if (currentHealth <= 0)
        {
            Die();
        }
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
    }
}
