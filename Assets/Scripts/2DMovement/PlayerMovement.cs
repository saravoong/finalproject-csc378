using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float sprintMultiplier = 1.5f;
    public LayerMask obstacleLayers;
    public AudioClip blockedSound;
    public float blockedSoundVolume = 1f;
    public float blockedSoundCadence = 0.5f;
    private float lastBlockedSoundTime = -Mathf.Infinity;
    private bool isMoving = false;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    public Vector2 lastMoveDirection;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!isMoving)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.x != 0)
                input.y = 0;
            if (input != Vector2.zero)
            {
                lastMoveDirection = input;
                StartCoroutine(Move(input));
            }
        }
    }

    IEnumerator Move(Vector2 direction)
    {
        isMoving = true;
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            currentSpeed *= sprintMultiplier;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(direction.x, direction.y, 0);
        
        if (direction.y > 0)
        {
            animator.SetTrigger("WalkUp");
            spriteRenderer.flipX = false;
        }
        else if (direction.y < 0)
        {
            animator.SetTrigger("WalkDown");
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            animator.SetTrigger("WalkLeft");
            spriteRenderer.flipX = false;
        }
        else if (direction.x > 0)
        {
            animator.SetTrigger("WalkLeft");
            spriteRenderer.flipX = true;
        }
        
        if (IsBlocked(targetPos))
        {
            if (blockedSound != null && Time.time - lastBlockedSoundTime >= blockedSoundCadence)
            {
                AudioSource.PlayClipAtPoint(blockedSound, transform.position, blockedSoundVolume);
                lastBlockedSoundTime = Time.time;
            }
            isMoving = false;
            yield break;
        }

        float timeToMove = 1f / currentSpeed;
        float elapsedTime = 0f;
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    bool IsBlocked(Vector3 targetPos)
    {
        Vector2 size = boxCollider.bounds.size;
        Collider2D[] hits = Physics2D.OverlapBoxAll(targetPos, size, 0f, obstacleLayers);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }
}
