using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class showOnContact : MonoBehaviour
{
    public GameObject instructionPanel;
    private bool playerIsClose;

    void Update() {
        if (playerIsClose) {
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

    // forcing player to finish convo
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            playerIsClose = false;
        }
    }

}
