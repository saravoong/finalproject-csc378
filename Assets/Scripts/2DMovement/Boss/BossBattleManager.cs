using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : MonoBehaviour
{
    [Header("Boss References")]
    public BossEnemyAI bossAI;
    public Collider2D bossCollider;
    public ParticleSystem poisonParticles;
    public BossHealthBar bossHealthBar;
    public SpriteRenderer bossSpriteRenderer;

    [Header("Boss Sprites")]
    public Sprite initialStateSprite;
    public Sprite combatStateSprite;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip initialStateSound;

    [Header("Visual Feedback")]
    public Color vulnerableColor = new Color(1f, 0.7f, 0.7f);
    public float flashingSpeed = 0.2f;
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
    public float warningFlashDuration = 2f;
    
    [Header("Positions")]
    public Vector3 startPosition;

    private enum BossState { Initial, Active, Vulnerable }
    private BossState currentState = BossState.Initial;
    
    private Coroutine battleCycleCoroutine;
    private Coroutine flashingCoroutine;

    void Start()
    {
        startPosition = transform.position;
        
        if (bossAI == null)
            bossAI = GetComponent<BossEnemyAI>();
        
        if (bossCollider == null)
            bossCollider = GetComponent<Collider2D>();
            
        if (bossSpriteRenderer == null)
            bossSpriteRenderer = GetComponent<SpriteRenderer>();
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
            
        if (bossSpriteRenderer != null)
            originalColor = bossSpriteRenderer.color;
            
        SetupBossHealthBar();
            
        StartBattleCycle();
    }
    
    void SetupBossHealthBar()
    {
        if (bossHealthBar == null)
        {
            bossHealthBar = FindObjectOfType<BossHealthBar>();
        }
        
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
        if (currentState == BossState.Active)
        {
            activeMinions.RemoveAll(minion => minion == null);
            
            if (activeMinions.Count == 0)
            {
                EnterVulnerableState();
            }
        }
    }
    
    void StartBattleCycle()
    {
        if (battleCycleCoroutine != null)
            StopCoroutine(battleCycleCoroutine);
        
        StopFlashingEffect();
            
        battleCycleCoroutine = StartCoroutine(BattleCycle());
    }
    
    IEnumerator BattleCycle()
    {
        currentState = BossState.Initial;
        Debug.Log("BossManager: Initial phase started");
        
        if (bossSpriteRenderer != null && initialStateSprite != null)
        {
            bossSpriteRenderer.sprite = initialStateSprite;
            Debug.Log("BossManager: Set initial state sprite");
        }
        
        PlayInitialStateSound();
        
        bossAI.TeleportTo(startPosition);
        
        ResetBossAppearance();
        
        if (bossCollider != null) {
            poisonParticles.Play();
        }
            
        bossAI.StopAttacking();
        
        if (bossHealthBar != null)
            bossHealthBar.SetNormalColor();
        
        yield return new WaitForSeconds(summonDelay);
        SummonMinions();
        
        yield return new WaitForSeconds(initialWaitTime - summonDelay);
        
        currentState = BossState.Active;
        
        if (bossSpriteRenderer != null && combatStateSprite != null)
        {
            bossSpriteRenderer.sprite = combatStateSprite;
            Debug.Log("BossManager: Set combat state sprite");
        }
        
        bossCollider.enabled = true;
        Debug.Log("BossManager: Active phase started");
        bossAI.StartAttacking();
    }
    
    void PlayInitialStateSound()
    {
        if (audioSource != null && initialStateSound != null)
        {
            audioSource.PlayOneShot(initialStateSound);
            Debug.Log("BossManager: Playing initial state sound");
        }
        else if (initialStateSound == null)
        {
            Debug.LogWarning("BossManager: Initial state sound clip not assigned!");
        }
    }
    
    void EnterVulnerableState()
    {
        currentState = BossState.Vulnerable;
        Debug.Log("BossManager: Vulnerable phase started");
        
        bossAI.StopAttacking();
        
        if (bossCollider != null) {
            bossCollider.enabled = false;
            poisonParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        
        if (bossSpriteRenderer != null)
        {
            if (combatStateSprite != null)
            {
                bossSpriteRenderer.sprite = combatStateSprite;
            }
            
            bossSpriteRenderer.color = vulnerableColor;
        }
        
        if (bossHealthBar != null)
            bossHealthBar.SetVulnerableColor();
            
        StartCoroutine(VulnerableTimer());
    }
    
    IEnumerator VulnerableTimer()
    {
        float regularVulnerableTime = vulnerableDuration - warningFlashDuration;
        
        if (regularVulnerableTime > 0)
        {
            yield return new WaitForSeconds(regularVulnerableTime);
            
            StartFlashingEffect();
            
            yield return new WaitForSeconds(warningFlashDuration);
        }
        else
        {
            yield return new WaitForSeconds(vulnerableDuration);
        }
        
        StartBattleCycle();
    }
    
    void StartFlashingEffect()
    {
        StopFlashingEffect();
        
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
        if (bossSpriteRenderer != null)
        {
            bossSpriteRenderer.color = originalColor;
        }
    }
    
    void SummonMinions()
    {
        activeMinions.Clear();
        
        if (summonPoint1 != null && minionPrefab != null)
        {
            GameObject minion1 = Instantiate(minionPrefab, summonPoint1.position, Quaternion.identity);
            AssignPlayerToMinion(minion1);
            activeMinions.Add(minion1);
            Debug.Log("BossManager: Summoned minion 1");
        }
        
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
        EnemyAI enemyAI = minion.GetComponent<EnemyAI>();
        if (enemyAI != null && playerTransform != null)
        {
            enemyAI.player = playerTransform;
            Debug.Log("Assigned player to minion EnemyAI");
            return;
        }
        
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