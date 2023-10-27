using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerupSlider : MonoBehaviour
{
    [SerializeField] Slider powerUpSlider;
    [SerializeField] Sprite[] Icons;
    [SerializeField] int[] toPowerUp;
    [SerializeField] Image onUseIcon;
    [SerializeField] Animator[] animators;
    PlayerController playerController;

    int currentPowerup;
    bool poweringUp = false;
    [SerializeField] GameObject player;
    Rigidbody2D playerRB;

    void Awake() {
        playerController = FindObjectOfType<PlayerController>();
        playerRB = player.GetComponent<Rigidbody2D>();
        powerUpSlider.maxValue = toPowerUp[0];
        onUseIcon.sprite = Icons[0];
    }

    void Start() {
        currentPowerup = 0;
        powerUpSlider.value = 0;
        Debug.Log(toPowerUp[0]);
    }

    void Update() {
        powerUpSlider.value = playerController.powerupCount;
        if(currentPowerup >= toPowerUp.Count()){
            gameObject.SetActive(false);
            return;}
        else if(powerUpSlider.value == toPowerUp[currentPowerup] && !poweringUp){
            StartCoroutine(Powerup());
        }
    }

    IEnumerator Powerup()
    {   
        poweringUp = true;
        playerRB.velocity = Vector3.zero;
        playerController.stopMovement = true;
        if(currentPowerup == 0){
            playerController.movementSpeed = 7f;
            playerController.myAnimator = animators[0];
        }
        else if(currentPowerup == 1){
            playerController.canFire = true;
            playerController.myAnimator = animators[1];
        }
        else{
            playerController.myAnimator = animators[2];
            //canfly
        }
        currentPowerup++;
        if(currentPowerup == toPowerUp.Count()){
            playerController.stopMovement = false;
        }
        //animate player
        yield return new WaitForSeconds(2);
        onUseIcon.sprite = Icons[currentPowerup];
        playerController.powerupCount = 0;
        playerController.stopMovement = false;
        poweringUp = false;
        //change player sprite

    }
}
