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

    private bool isMoving = false;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        StartCoroutine(EnemyLoop());
    }

    IEnumerator EnemyLoop()
    {
        while (true)
        {
            if (!isMoving)
            {
                Vector2Int enemyPos = Vector2Int.RoundToInt(transform.position);
                Vector2Int playerPos = Vector2Int.RoundToInt(player.position);

                if (Vector2Int.Distance(enemyPos, playerPos) <= 1)
                {
                    Attack();
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
                                yield return StartCoroutine(MoveToPosition(targetPos));
                            }
                        }
                    }
                    Attack();
                }
            }
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    void Attack()
    {
        Vector2Int enemyPos = Vector2Int.RoundToInt(transform.position);
        Vector2Int playerPos = Vector2Int.RoundToInt(player.position);
        Vector2Int attackDir = playerPos - enemyPos;

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
        Vector2Int attackCell = enemyPos + attackDir;
        Vector3 attackPos = new Vector3(attackCell.x, attackCell.y, transform.position.z);
        GameObject attackObj = Instantiate(attackPrefab, attackPos, Quaternion.identity);
        Destroy(attackObj, 0.5f);
    }

    IEnumerator MoveToPosition(Vector3 targetPos)
    {
        isMoving = true;
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
}
