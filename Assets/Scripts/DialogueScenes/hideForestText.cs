using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hideForestText : MonoBehaviour
{
    public GameObject canvas;

    void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            if (canvas.activeInHierarchy) {
                canvas.SetActive(false);
            } else {
            canvas.SetActive(true);
            }
        }
        
    }
//     public float delay = 5f;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         StartCoroutine(hide());
//     }

//     IEnumerator hide() {
//         yield return new WaitForSeconds(delay);
//         obj.gameObject.SetActive(false);
//     }
}
