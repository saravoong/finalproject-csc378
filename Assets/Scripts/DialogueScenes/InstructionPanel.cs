using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class InstructionPanel: MonoBehaviour
{
    public GameObject instructionPanel;
    public GameObject collectible;
    public bool playerIsClose;
    
    void Update() {
        if (playerIsClose && collectible) {
            instructionPanel.SetActive(true);
        } else {
            instructionPanel.SetActive(false);
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
        }
    }
}