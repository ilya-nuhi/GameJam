using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider healtSlider;
    [SerializeField] Health playerHealth;
    

    void Awake() {
        
    }

    void Start() {
        healtSlider.maxValue = playerHealth.GetHealth();
    }

    void Update() {
        healtSlider.value = playerHealth.GetHealth();
    }
}
