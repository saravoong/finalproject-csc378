using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTrigger : MonoBehaviour
{
    public string endSceneName = "EndScene";
    public panelFadeOut levelLoader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // SceneManager.LoadScene(endSceneName);
            levelLoader.LoadNextLevel();
        }
    }
}
