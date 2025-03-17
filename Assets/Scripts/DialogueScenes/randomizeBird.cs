using UnityEngine;

public class randomizeBird : MonoBehaviour
{
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // animator = GetComponent<Animator>();
        // animator.SetFloat("Offset", Random.Range(0f, 3f));
        float randomRange = Random.Range(1.0f, 2.0f);
        transform.localScale = new Vector3(randomRange, randomRange, 0);
    }

}
