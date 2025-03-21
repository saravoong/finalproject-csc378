using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hideForestText : MonoBehaviour
{
    public GameObject h;            // h (help) icon

    public GameObject keybinds;     // the boxes with the keybinds

    void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            if (keybinds.activeInHierarchy) {
                keybinds.SetActive(false);
                h.SetActive(true);
            } else {
                keybinds.SetActive(true);
                h.SetActive(false);
            }
        }
        
    }

}
