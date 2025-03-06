using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// tutorial: https://www.youtube.com/watch?v=1nFNOyCalzo&t=35s
public class NPCdialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject instructionPanel;
    public TextMeshProUGUI dialogueText;    // dialogue: actual dialogue
    public TextMeshProUGUI speakerText;     // speaker: ie whos talking
    public GameObject hideObj;              // to hide npc after talk(if needed)
    public string[] dialogue;               
    public string[] speaker;
    private int index;
    public float wordSpeed;
    public bool playerIsClose;
    public GameObject continueButton;
    
    void Update() {
        if (playerIsClose) {
            instructionPanel.SetActive(true);
        } else {
            instructionPanel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.T) && playerIsClose) {
            if (dialoguePanel.activeInHierarchy) {
                zeroText();
            } else {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        } 

        if (dialogueText.text == dialogue[index]) {
            continueButton.SetActive(true);
        }
    }

    public void zeroText() {
        dialogueText.text = "";
        speakerText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        hideObj.SetActive(false);
        if (instructionPanel.activeInHierarchy) {
            instructionPanel.SetActive(false);
        }
    }

    IEnumerator Typing() {
        speakerText.text = speaker[index];
        foreach(char letter in dialogue[index].ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine() {
        continueButton.SetActive(false);
        if (index < dialogue.Length - 1) {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        } else {
            zeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            playerIsClose = true;
        }
    }

    // forcing player to finish convo
    // private void OnTriggerExit2D(Collider2D other) {
    //     if (other.gameObject.name == "Player") {
    //         playerIsClose = false;
    //         zeroText();
    //     }
    // }
        private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "Player") {
            playerIsClose = false;
        }
    }

}
