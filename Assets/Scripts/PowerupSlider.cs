using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupSlider : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Slider powerUpSlider;
    [SerializeField] Image[] Icons;
    [SerializeField] int[] toPowerUp;
    [SerializeField] Image onUseIcon;
    PlayerController playerController;
    int currentPowerup;
    bool poweringUp = false;
    

    void Awake() {
        playerController = FindObjectOfType<PlayerController>();
        powerUpSlider.maxValue = toPowerUp[0];
        onUseIcon = Icons[0];
    }

    void Start() {
        currentPowerup = 0;
        powerUpSlider.value = 0;
        
    }

    void Update() {
        powerUpSlider.value = playerController.powerupCount;
        if(powerUpSlider.value == toPowerUp[currentPowerup] && !poweringUp){
            StartCoroutine(Powerup());
        }
    }

    IEnumerator Powerup()
    {
        poweringUp = true;
        currentPowerup++;
        //play levelup effect
        //animate player
        Debug.Log("animations and effects");
        yield return new WaitForSeconds(2);
        playerController.powerupCount = 0;
        //change player sprite

    }
}
