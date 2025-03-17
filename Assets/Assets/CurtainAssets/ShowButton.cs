using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ShowButton : MonoBehaviour
{
    private Button button;
    private Image buttonImage;
    [SerializeField] private TextMeshProUGUI dialoguetext;
    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();
    }

    void Update() {
        if (dialoguetext.text.Contains("Congratulations Beetrice, you have officially become our Kingdom Baker!")) {
            showButton();
        }
    }

    private void showButton() {
        Color color = buttonImage.color;
        color.a = 1f;
        buttonImage.color = color;
    }
}
