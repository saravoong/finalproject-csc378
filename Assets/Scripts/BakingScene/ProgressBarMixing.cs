using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBarMixing : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] public FillingCustardTransition custardTransition;
    [SerializeField] private Button nextStepButton;
    [SerializeField] private GameObject arrow;
    public AudioClip doneSoundEffect;
    private AudioSource audioSource;

    private void Awake() {
        slider = gameObject.GetComponent<Slider>();
        audioSource = GetComponent<AudioSource>();
    }

    public void addProgress(float currentValue) {
        slider.value += currentValue;

        if (slider.value >= slider.maxValue) {
            ShowFilling();
        }
    }

    private void ShowFilling()
    {
        arrow.SetActive(false);
        audioSource.PlayOneShot(doneSoundEffect);
        StartCoroutine(custardTransition.FadeIn()); 
        Invoke("EnableNextStepButton", 1.5f);
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
