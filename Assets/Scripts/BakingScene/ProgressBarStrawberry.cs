using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBarStrawberry : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Button nextStepButton;
    public AudioClip doneSoundEffect;
    private AudioSource audioSource;

    private void Awake() {
        slider = gameObject.GetComponent<Slider>();
        audioSource = GetComponent<AudioSource>();
    }

    public void addProgress(float currentValue) {
        slider.value += currentValue;

        if (slider.value >= slider.maxValue) {
            audioSource.PlayOneShot(doneSoundEffect);
            Invoke("EnableNextStepButton", 0.5f);
        }
    }

    private void EnableNextStepButton() {
        nextStepButton.gameObject.SetActive(true);
    }

    public float GetValue() {
        return slider.value;
    }

    public float GetMaxValue() {
        return slider.maxValue;
    }
}
