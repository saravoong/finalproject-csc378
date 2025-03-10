using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    public int damage = 1;
    public AudioClip damageSound;
    public float damageSoundVolume = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                if (damageSound != null)
                {
                    AudioSource.PlayClipAtPoint(damageSound, other.transform.position, damageSoundVolume);
                }
            }
        }
    }
}
