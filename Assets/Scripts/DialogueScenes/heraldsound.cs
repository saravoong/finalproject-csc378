using UnityEngine;
using TMPro;

public class heraldsound : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialoguetext;
    [SerializeField] private AudioSource audioSource; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dialoguetext.text.Contains("Present Day")) {
            audioSource.Play();
        }
    }
}
