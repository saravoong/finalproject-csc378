using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
    // https://www.youtube.com/watch?v=uDlGIXFeNwg&ab_channel=zonlib
    public Image _bar;
    public RectTransform button;
    public float _value = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        valueChange(_value);
    }

    void valueChange(float value) {
        float amount = (value/100.00f) * 180.0f/360;
        _bar.fillAmount = amount;
        float buttonAngle = amount * 360;
        button.localEulerAngles = new Vector3(0, 0, -buttonAngle);
    }
}
