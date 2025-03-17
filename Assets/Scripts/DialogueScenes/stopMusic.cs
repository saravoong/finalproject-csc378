using UnityEngine;
using TMPro;

public class stopMusic : MonoBehaviour
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
        if (dialoguetext.text.Contains("It's a story I always remember.")) {
            audioSource.Stop();
        }
    }
}
