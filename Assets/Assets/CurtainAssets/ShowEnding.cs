using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowEnding : MonoBehaviour
{
    [SerializeField] private Button endButton;
    [SerializeField] private Canvas curtains;
    public AudioClip endingSoundEffect;
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        if (endButton != null) {
            endButton.onClick.AddListener(CloseCurtains);
        }
    }

    void CloseCurtains()
    {
        curtains.gameObject.SetActive(true);
        audioSource.PlayOneShot(endingSoundEffect);
    }
}
