using UnityEngine;

public class checkBottom : MonoBehaviour
{
    public SpriteRenderer sprite;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            sprite.sortingOrder = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            sprite.sortingOrder = 2;
        }
    }
}
