using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public LayerMask obstacleLayers; 
    public float moveSpeed = 4f;
    public float pauseDuration = 2f;
    public GameObject attackPrefab;
    
    public float detectionRadius = 8f;
    
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 1.5f;
    
    public float projectileSpeed = 5f;
    public float attackDelay = 0.3f;
    
    private Animator animator;
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    
    private SpriteRenderer spriteRenderer;
    private bool isFacingLeft = true;
    
    private EnemyHealth healthComponent;
    
    private bool isMoving = false;
    private bool playerDetected = false;
    private BoxCollider2D boxCollider;
    private Vector3 startPosition;
    private float floatTimer = 0f;
    private Coroutine enemyLoopCoroutine;
    
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthComponent = GetComponent<EnemyHealth>();
    }

    void Start()
    {
        startPosition = transform.position;
        
        UpdateSpriteDirection(true);
    }
    
    void Update()
    {
        ApplyFloatingEffect();
        
        CheckPlayerDetection();
    }
    
    void CheckPlayerDetection()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRadius)
        {
            if (!playerDetected)
            {
                playerDetected = true;
                enemyLoopCoroutine = StartCoroutine(EnemyLoop());
            }
        }
        else
        {
            if (playerDetected)
            {
                playerDetected = false;
                if (enemyLoopCoroutine != null)
                {
                    StopCoroutine(enemyLoopCoroutine);
                    enemyLoopCoroutine = null;
                }
            }
        }
    }
    
    void UpdateSpriteDirection(bool faceLeft)
    {
        if (isFacingLeft != faceLeft)
        {
            isFacingLeft = faceLeft;
            spriteRenderer.flipX = !faceLeft;
        }
    }
    
    void ApplyFloatingEffect()
    {
        if (!isMoving)
        {
            floatTimer += Time.deltaTime;
            float yOffset = Mathf.Sin(floatTimer * floatFrequency) * floatAmplitude;
            Vector3 currentPosition = transform.position;
            
            transform.position = new Vector3(
                currentPosition.x,
                Mathf.RoundToInt(currentPosition.y) + yOffset,
                currentPosition.z
            );
        }
    }

    IEnumerator EnemyLoop()
    {
        while (true)
        {
            if (!isMoving)
            {
                Vector2Int enemyPos = Vector2Int.RoundToInt(transform.position);
                Vector2Int playerPos = Vector2Int.RoundToInt(player.position);

                UpdateSpriteDirection(playerPos.x < enemyPos.x);

                if (Vector2Int.Distance(enemyPos, playerPos) <= 1)
                {
                    StartCoroutine(AttackCoroutine());
                }
                else
                {
                    Vector2Int? targetCell = GetClosestAdjacentCell(playerPos, enemyPos);
                    if (targetCell.HasValue)
                    {
                        List<Vector2Int> path = FindPath(enemyPos, targetCell.Value);
                        if (path != null && path.Count > 1)
                        {
                            int steps = Mathf.Min(2, path.Count - 1);
                            for (int i = 1; i <= steps; i++)
                            {
                                Vector3 targetPos = new Vector3(path[i].x, path[i].y, transform.position.z);
                                
                                Vector2 moveDirection = targetPos - transform.position;
                                if (Mathf.Abs(moveDirection.x) > 0.1f)
                                {
                                    UpdateSpriteDirection(moveDirection.x < 0);
                                }
                                
                                yield return StartCoroutine(MoveToPosition(targetPos));
                            }
                        }
                    }
                    StartCoroutine(AttackCoroutine());
                }
            }
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    IEnumerator AttackCoroutine()
    {
        if (animator != null)
        {
            animator.SetTrigger(AttackTrigger);
        }
        
        Vector2Int enemyPos = Vector2Int.RoundToInt(transform.position);
        Vector2Int playerPos = Vector2Int.RoundToInt(player.position);
        Vector2Int attackDir = playerPos - enemyPos;
        
        if (attackDir.x != 0)
        {
            UpdateSpriteDirection(attackDir.x < 0);
        }

        if (Mathf.Abs(attackDir.x) > Mathf.Abs(attackDir.y))
        {
            attackDir.y = 0;
            attackDir.x = (attackDir.x > 0) ? 1 : -1;
        }
        else
        {
            attackDir.x = 0;
            attackDir.y = (attackDir.y > 0) ? 1 : -1;
        }
        
        Vector2 projectileDirection = new Vector2(attackDir.x, attackDir.y).normalized;
        
        float rotationAngle = 0f;
        
        if (attackDir.x < 0)
        {
            rotationAngle = -90f;
        }
        else if (attackDir.x > 0)
        {
            rotationAngle = 90f;
        }
        else if (attackDir.y > 0)
        {
            rotationAngle = 180f;
        }
        
        Vector3 spawnPos = transform.position + new Vector3(attackDir.x * 0.5f, attackDir.y * 0.5f, 0);
        
        Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);
        GameObject projectile = Instantiate(attackPrefab, spawnPos, rotation);
        
        if (healthComponent != null)
        {
            healthComponent.RegisterProjectile(projectile);
        }
        
        Vector3 originalScale = projectile.transform.localScale;
        
        projectile.transform.localScale = Vector3.zero;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < attackDelay)
        {
            if (this == null || projectile == null)
                yield break;
                
            float t = elapsedTime / attackDelay;
            
            float scaleFactor = Mathf.SmoothStep(0, 1, t);
            projectile.transform.localScale = originalScale * scaleFactor;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (this == null || projectile == null)
            yield break;
            
        projectile.transform.localScale = originalScale;
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = projectile.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }
        
        rb.linearVelocity = projectileDirection * projectileSpeed;
        
        ProjectileCleanup cleanup = projectile.AddComponent<ProjectileCleanup>();
        cleanup.Initialize(healthComponent, 2f);
    }

    IEnumerator MoveToPosition(Vector3 targetPos)
    {
        isMoving = true;
        floatTimer = 0f;
        Vector3 startPos = transform.position;
        float timeToMove = 1f / moveSpeed;
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

    Vector2Int? GetClosestAdjacentCell(Vector2Int playerPos, Vector2Int enemyPos)
    {
        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };
        List<Vector2Int> candidates = new List<Vector2Int>();
        foreach (Vector2Int dir in directions)
        {
            Vector2Int candidate = playerPos + dir;
            if (IsWalkable(candidate))
                candidates.Add(candidate);
        }
        if (candidates.Count == 0)
            return null;

        Vector2Int best = candidates[0];
        float bestDist = ManhattanDistance(enemyPos, best);
        foreach (Vector2Int candidate in candidates)
        {
            float d = ManhattanDistance(enemyPos, candidate);
            if (d < bestDist)
            {
                bestDist = d;
                best = candidate;
            }
        }
        return best;
    }

    float ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };

        bool found = false;
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == target)
            {
                found = true;
                break;
            }
            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = current + dir;
                if (!visited.Contains(neighbor) && IsWalkable(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        if (!found)
            return null;

        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int curr = target;
        path.Add(curr);
        while (curr != start)
        {
            curr = cameFrom[curr];
            path.Add(curr);
        }
        path.Reverse();
        return path;
    }

    bool IsWalkable(Vector2Int pos)
    {
        Vector2 center = new Vector2(pos.x, pos.y);
        Collider2D hit = Physics2D.OverlapBox(center, new Vector2(0.9f, 0.9f), 0f, obstacleLayers);
        Vector2Int playerPos = Vector2Int.RoundToInt(player.position);
        if (pos == playerPos)
            return false;
        return hit == null;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}