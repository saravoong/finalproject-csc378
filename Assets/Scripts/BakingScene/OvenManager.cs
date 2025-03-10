using UnityEngine;
using UnityEngine.UI;


public class OvenManager : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject unbakedCrust;
    [SerializeField] private GameObject bakedCrust;
    [SerializeField] private GameObject ovenGlass;
    [SerializeField] public FillingCustardTransition custardTransition;
    [SerializeField] private Button nextStepButton;
    public AudioClip doneSoundEffect;
    private AudioSource audioSource;

    private bool hasStartedBaking = false;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
    
    }

    void Update()
    {
        if (slider.value == 1 && !hasStartedBaking) {
            hasStartedBaking = true;
            slider.gameObject.SetActive(false);
            arrow.SetActive(false);
            unbakedCrust.SetActive(true);
            ovenGlass.SetActive(true);
            bakedCrust.SetActive(true);
            showBaking();
        }
    }

    private void showBaking() {
        StartCoroutine(custardTransition.FadeIn()); 
        Invoke("openOven", 2.5f);
    }

    private void openOven() {
        ovenGlass.SetActive(false);
        audioSource.PlayOneShot(doneSoundEffect);
        nextStepButton.gameObject.SetActive(true);
    }
}
