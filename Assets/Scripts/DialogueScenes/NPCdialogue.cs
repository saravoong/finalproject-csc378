using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// tutorial: https://www.youtube.com/watch?v=1nFNOyCalzo&t=35s
public class NPCdialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;
    public float wordSpeed;
    public bool playerIsClose;
    public GameObject continueButton;
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose) {
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
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing() {
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
        if (other.gameObject.name == "player") {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "player") {
            playerIsClose = false;
            zeroText();
        }
    }

}
