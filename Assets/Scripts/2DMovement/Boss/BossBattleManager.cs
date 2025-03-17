using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : MonoBehaviour
{
    [Header("Boss References")]
    public BossEnemyAI bossAI;
    public Collider2D bossCollider;
    public ParticleSystem poisonParticles;
    public BossHealthBar bossHealthBar; // Reference to the boss health bar UI
    public SpriteRenderer bossSpriteRenderer; // Reference to the boss sprite renderer

    [Header("Visual Feedback")]
    public Color vulnerableColor = new Color(1f, 0.7f, 0.7f); // Light red/pinkish
    public float flashingSpeed = 0.2f; // Time between flashes
    private Color originalColor;

    [Header("Player Reference")]
    public Transform playerTransform;
    
    [Header("Summon Settings")]
    public GameObject minionPrefab;
    public Transform summonPoint1;
    public Transform summonPoint2;
    private List<GameObject> activeMinions = new List<GameObject>();
    
    [Header("Timing Settings")]
    public float initialWaitTime = 2f;
    public float summonDelay = 1f;
    public float vulnerableDuration = 5f;
    public float warningFlashDuration = 2f; // Duration of flashing warning
    
    [Header("Positions")]
    public Vector3 startPosition;

    // Battle state
    private enum BossState { Initial, Active, Vulnerable }
    private BossState currentState = BossState.Initial;
    
    private Coroutine battleCycleCoroutine;
    private Coroutine flashingCoroutine;

    void Start()
    {
        startPosition = transform.position;
        
        // Get components if not set
        if (bossAI == null)
            bossAI = GetComponent<BossEnemyAI>();
        
        if (bossCollider == null)
            bossCollider = GetComponent<Collider2D>();
            
        if (bossSpriteRenderer == null)
            bossSpriteRenderer = GetComponent<SpriteRenderer>();
            
        // Store the original color
        if (bossSpriteRenderer != null)
            originalColor = bossSpriteRenderer.color;
            
        // Set up the boss health bar if available
        SetupBossHealthBar();
            
        // Start the battle cycle
        StartBattleCycle();
    }
    
    void SetupBossHealthBar()
    {
        // If we don't have a reference to the health bar, try to find it
        if (bossHealthBar == null)
        {
            bossHealthBar = FindObjectOfType<BossHealthBar>();
        }
        
        // Connect the boss health to the UI
        if (bossHealthBar != null)
        {
            BossHealth bossHealth = GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealthBar.SetBoss(bossHealth, "Evil Witch");
            }
        }
    }
    
    void Update()
    {
        // Check if all minions are destroyed
        if (currentState == BossState.Active)
        {
            // Clean up the list to remove any null references (destroyed minions)
            activeMinions.RemoveAll(minion => minion == null);
            
            // If all minions are gone, enter vulnerable state
            if (activeMinions.Count == 0)
            {
                EnterVulnerableState();
            }
        }
    }
    
    void StartBattleCycle()
    {
        // Stop any existing battle cycle
        if (battleCycleCoroutine != null)
            StopCoroutine(battleCycleCoroutine);
        
        // Stop any flashing effect    
        StopFlashingEffect();
            
        // Start a new battle cycle
        battleCycleCoroutine = StartCoroutine(BattleCycle());
    }
    
    IEnumerator BattleCycle()
    {
        // Phase 1: Initial phase
        currentState = BossState.Initial;
        Debug.Log("BossManager: Initial phase started");
        
        // Make sure boss is at start position
        bossAI.TeleportTo(startPosition);
        
        // Reset boss appearance
        ResetBossAppearance();
        
        // Enable collider
        if (bossCollider != null) {
            poisonParticles.Play();
        }
            
        // Stop any attacks
        bossAI.StopAttacking();
        
        // Set health bar to normal color
        if (bossHealthBar != null)
            bossHealthBar.SetNormalColor();
        
        // After summonDelay seconds, summon minions
        yield return new WaitForSeconds(summonDelay);
        SummonMinions();
        
        // Wait the remainder of the initial wait time
        yield return new WaitForSeconds(initialWaitTime - summonDelay);
        
        // Phase 2: Active phase - boss starts attacking
        currentState = BossState.Active;
        bossCollider.enabled = true;
        Debug.Log("BossManager: Active phase started");
        bossAI.StartAttacking();
        
        // The update method will now monitor minions and trigger vulnerable state
    }
    
    void EnterVulnerableState()
    {
        // Phase 3: Vulnerable phase
        currentState = BossState.Vulnerable;
        Debug.Log("BossManager: Vulnerable phase started");
        
        // Stop boss attacks
        bossAI.StopAttacking();
        
        // Disable collider
        if (bossCollider != null) {
            bossCollider.enabled = false;
            poisonParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        
        // Apply vulnerable appearance
        if (bossSpriteRenderer != null)
        {
            bossSpriteRenderer.color = vulnerableColor;
        }
        
        // Change health bar to vulnerable color
        if (bossHealthBar != null)
            bossHealthBar.SetVulnerableColor();
            
        // Start vulnerable timer
        StartCoroutine(VulnerableTimer());
    }
    
    IEnumerator VulnerableTimer()
    {
        // Wait until it's time to start flashing
        float regularVulnerableTime = vulnerableDuration - warningFlashDuration;
        
        if (regularVulnerableTime > 0)
        {
            yield return new WaitForSeconds(regularVulnerableTime);
            
            // Start the flashing warning
            StartFlashingEffect();
            
            // Wait for the flashing duration
            yield return new WaitForSeconds(warningFlashDuration);
        }
        else
        {
            // If vulnerable time is shorter than warning time, just wait the full duration
            yield return new WaitForSeconds(vulnerableDuration);
        }
        
        // Restart the cycle
        StartBattleCycle();
    }
    
    void StartFlashingEffect()
    {
        // Stop any existing flashing
        StopFlashingEffect();
        
        // Start a new flashing coroutine
        flashingCoroutine = StartCoroutine(FlashingEffect());
        Debug.Log("BossManager: Warning flash started");
    }
    
    void StopFlashingEffect()
    {
        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
            flashingCoroutine = null;
        }
    }
    
    IEnumerator FlashingEffect()
    {
        bool showVulnerable = true;
        
        while (true)
        {
            // Toggle between vulnerable and original color
            if (bossSpriteRenderer != null)
            {
                bossSpriteRenderer.color = showVulnerable ? vulnerableColor : originalColor;
            }
            
            showVulnerable = !showVulnerable;
            yield return new WaitForSeconds(flashingSpeed);
        }
    }
    
    void ResetBossAppearance()
    {
        // Reset sprite to original color
        if (bossSpriteRenderer != null)
        {
            bossSpriteRenderer.color = originalColor;
        }
    }
    
    void SummonMinions()
    {
        // Clear any old minions
        activeMinions.Clear();
        
        // Summon at position 1
        if (summonPoint1 != null && minionPrefab != null)
        {
            GameObject minion1 = Instantiate(minionPrefab, summonPoint1.position, Quaternion.identity);
            AssignPlayerToMinion(minion1);
            activeMinions.Add(minion1);
            Debug.Log("BossManager: Summoned minion 1");
        }
        
        // Summon at position 2
        if (summonPoint2 != null && minionPrefab != null)
        {
            GameObject minion2 = Instantiate(minionPrefab, summonPoint2.position, Quaternion.identity);
            AssignPlayerToMinion(minion2);
            activeMinions.Add(minion2);
            Debug.Log("BossManager: Summoned minion 2");
        }
    }

    void AssignPlayerToMinion(GameObject minion)
    {
        // Find any component that might need the player reference
        // First try EnemyAI specifically
        EnemyAI enemyAI = minion.GetComponent<EnemyAI>();
        if (enemyAI != null && playerTransform != null)
        {
            enemyAI.player = playerTransform;
            Debug.Log("Assigned player to minion EnemyAI");
            return;
        }
        
        // If no EnemyAI component found, try a more general approach with reflection
        // This finds any component with a public Transform field named "player"
        MonoBehaviour[] components = minion.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour component in components)
        {
            System.Type type = component.GetType();
            var field = type.GetField("player");
            if (field != null && field.FieldType == typeof(Transform) && playerTransform != null)
            {
                field.SetValue(component, playerTransform);
                Debug.Log($"Assigned player to minion component: {type.Name}");
                break;
            }
        }
    }

    public bool IsVulnerable()
    {
        return currentState == BossState.Vulnerable;
    }
}