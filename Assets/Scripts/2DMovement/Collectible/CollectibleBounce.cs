using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CollectibleBounce : MonoBehaviour
{
    public float bounceHeight = 0.5f;
    public float bounceSpeed = 1f;
    
    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}