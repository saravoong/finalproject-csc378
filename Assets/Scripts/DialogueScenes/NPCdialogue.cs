using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

// tutorial: https://www.youtube.com/watch?v=1nFNOyCalzo&t=35s
public class NPCdialogue : MonoBehaviour
{
    public panelFadeOut levelLoader;
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
    
    void Update() {
        if (instructionPanel != null) {
            if (playerIsClose) {
                instructionPanel.SetActive(true);
            } else {
                instructionPanel.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && playerIsClose) {
            if (dialoguePanel.activeInHierarchy) {
                zeroText();
            } else {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        } 

        if (dialoguePanel.activeInHierarchy) {
            if (Input.GetMouseButtonDown(0)) {
                if (dialogueText.text == dialogue[index]) {
                    NextLine();
                } else {
                    StopAllCoroutines();
                    dialogueText.text = dialogue[index];
                }
            }
        }
    }

    public void zeroText() {
        StopAllCoroutines();    // resets dialogue chararray (to prevent the random characters after reset)
        index = 0;
        dialogueText.text = "";
        speakerText.text = "";
        dialoguePanel.SetActive(false);
        if (instructionPanel != null) {
            if (instructionPanel.activeInHierarchy) {
                instructionPanel.SetActive(false);
            }
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
        // continueButton.SetActive(false);
        if (index < dialogue.Length - 1) {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        } else {
            // chip running away
            if (hideObj != null) {
                hideObj.SetActive(false);
            }

            // going to witch fight after conversation
            string activeScene = SceneManager.GetActiveScene().name;
            if (activeScene == "forestScene") {
                //SceneManager.LoadScene(11);
                levelLoader.LoadNextLevel();
            }
            zeroText();
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
            zeroText();
        }
    }
    // private void OnTriggerExit2D(Collider2D other) {
    //     if (other.gameObject.name == "Player") {
    //         playerIsClose = false;
    //     }
    // }

}
