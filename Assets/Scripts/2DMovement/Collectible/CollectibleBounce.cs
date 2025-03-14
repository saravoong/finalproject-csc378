using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CollectibleBounce : MonoBehaviour
{
    // The maximum vertical distance from the starting position.
    public float bounceHeight = 0.5f;
    // The speed of the bouncing motion.
    public float bounceSpeed = 1f;
    
    // Record the starting position of the GameObject.
    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        // Calculate the new Y position using a sine wave.
        float newY = startPos.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        // Update the GameObject's position while keeping the original X and Z coordinates.
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
