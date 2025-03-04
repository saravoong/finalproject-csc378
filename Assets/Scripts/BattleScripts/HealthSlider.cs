using UnityEngine;
using UnityEngine.UI;

// reference: https://www.youtube.com/watch?v=_lREXfAMUcE

public class HealthSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cameraObj; // to keep bar upright (though it prob doesn't matter)
    [SerializeField] private Transform target; // target as in the char the health bar is attached to
    [SerializeField] private Vector3 offset;


    // public void updateHealthBar(float currentValue, float maxValue) {
    //     slider.value = currentValue / maxValue;
    // }
    public void updateHealthBar(float amount) {
        slider.value += amount;
    }

    private void Update() {
        transform.rotation = cameraObj.transform.rotation;
        transform.position = target.position+offset;
    }
}