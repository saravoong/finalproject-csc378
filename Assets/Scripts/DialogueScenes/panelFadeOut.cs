using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

// https://www.youtube.com/watch?v=CE9VOZivb3I
public class panelFadeOut : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    // void Update() {
    //     if (Input.GetKeyDown(KeyCode.T)) {
    //         LoadNextLevel();
    //     }
    // }

    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

    }

    IEnumerator LoadLevel(int levelIndex) {
        transition.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
