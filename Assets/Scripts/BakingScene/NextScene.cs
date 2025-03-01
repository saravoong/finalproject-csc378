using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
    public string sceneName;
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null) {
            button.onClick.AddListener(ChangeScene);
        }
    }

    void ChangeScene() {
        SceneManager.LoadScene(sceneName);
    }
}
