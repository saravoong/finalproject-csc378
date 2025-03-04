using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private string objName;
    private bool dead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        objName = gameObject.name;
        dead = false;
    } 

    void Update() {
        if (currentHealth <= 0) {
            if (objName == "beetrice") {
                Debug.Log("beet ded");
            } else if (objName == "witch") {
                Destroy(gameObject);
                dead = true;
            }
        }
    }

    public bool isDead() {
        return dead;
    }

    public void takeDamage(int amount) {
        currentHealth -= amount;
    }

    public void heal(int amount) {  // for eventual healing
        currentHealth += amount;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }



    
}
