using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

// tutorial: https://www.youtube.com/watch?v=1nFNOyCalzo&t=35s
// dialogue only scene
public class dialoguescene : MonoBehaviour
{
    // public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;    // dialogue: actual dialogue
    public TextMeshProUGUI speakerText;     // speaker: ie whos talking
    public string[] dialogue;               
    public string[] speaker;
    private int index;
    public float wordSpeed;
    // public bool playerIsClose;
    // public GameObject continueButton;
    
    public void Start() {
        dialogueText.text = string.Empty;
        speakerText.text = string.Empty;
        StartDialogue();
    }

    void StartDialogue() {
        index = 0;
        StartCoroutine(Typing());
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (dialogueText.text == dialogue[index]) {
                NextLine();
            } else {
                StopAllCoroutines();
                dialogueText.text = dialogue[index];
            }
        }

    }

    public void zeroText() {
        dialogueText.text = "";
        speakerText.text = "";
        index = 0;
        // hideObj.SetActive(false);
        // if (instructionPanel.activeInHierarchy) {
        //     instructionPanel.SetActive(false);
        // }
    }

    IEnumerator Typing() {
        speakerText.text = speaker[index];
        foreach(char letter in dialogue[index].ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine() {
        if (index < dialogue.Length - 1) {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        } else {
            //zeroText();
            string activeScene = SceneManager.GetActiveScene().name;
            if (activeScene == "IntroCutscene") {
                SceneManager.LoadScene(8);
            }
        }
    }

}
