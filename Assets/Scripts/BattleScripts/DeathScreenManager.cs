using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathScreenManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject deathScreenPanel;
    public Button retryButton;
    public float fadeInTime = 0.5f;
    public float delayBeforeInteractive = 0.5f;

    [Header("Audio")]
    public AudioClip deathSound;
    public float deathSoundVolume = 1f;

    public static DeathScreenManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            SetupUI();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void SetupUI()
    {
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(RetryGame);
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(false);
        }
    }

    public void ShowDeathScreen()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        }

        Time.timeScale = 0f;

        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(true);
            StartCoroutine(FadeInDeathScreen());
        }
    }

    private IEnumerator FadeInDeathScreen()
    {
        CanvasGroup canvasGroup = deathScreenPanel.GetComponent<CanvasGroup>();
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            
            float elapsed = 0f;
            while (elapsed < fadeInTime)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInTime);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            
            canvasGroup.alpha = 1f;
        }
        
        yield return new WaitForSecondsRealtime(delayBeforeInteractive);
        
        if (retryButton != null)
        {
            retryButton.interactable = true;
        }
    }

    private void RetryGame()
    {
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}