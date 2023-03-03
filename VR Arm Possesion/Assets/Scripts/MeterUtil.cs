using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterUtil : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void SetSlider(float val)
    {
        slider.value = val;
        Debug.Log("Value set to: " + val);
    }
}
