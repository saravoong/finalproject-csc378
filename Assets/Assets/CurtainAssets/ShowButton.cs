using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowButton : MonoBehaviour
{
    private Button button;
    private Image buttonImage;
    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();
        Invoke("showButton", 4f); 
    }
    private void showButton() {
        Color color = buttonImage.color;
        color.a = 1f;
        buttonImage.color = color;
    }
}
