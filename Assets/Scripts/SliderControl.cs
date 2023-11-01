using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SliderControl : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider healtSlider;
    Health playerHealth;

    void Awake() {
        playerHealth = FindObjectOfType<Health>();
    }

    void Start() {
        healtSlider.maxValue = playerHealth.GetHealth();
    }

    void Update() {
        healtSlider.value = playerHealth.GetHealth();
    }

    public void AttachHealth(){
        playerHealth = FindObjectOfType<Health>();
    }
}
