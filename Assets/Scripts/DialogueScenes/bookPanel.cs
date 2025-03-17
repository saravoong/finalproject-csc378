using UnityEngine;

public class bookPanel : MonoBehaviour
{
    public GameObject bookCanvas;
    public GameObject instructionPanel;
    private int index;
    public bool playerIsClose;
    // public GameObject continueButton;
    
    void Update() {
        if (playerIsClose) {
            instructionPanel.SetActive(true);
        } else {
            instructionPanel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R) && playerIsClose) {
            if (bookCanvas.activeInHierarchy) {
                bookCanvas.SetActive(false);
            } else {
                bookCanvas.SetActive(true);
            }
        } 

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            playerIsClose = false;
            bookCanvas.SetActive(false);
        }
    }
}
