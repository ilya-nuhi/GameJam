using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PowerupSlider : MonoBehaviour
{
    [SerializeField] Slider powerUpSlider;
    [SerializeField] Sprite[] Icons;
    [SerializeField] int[] toPowerUp;
    [SerializeField] Image onUseIcon;
    PlayerController playerController;

    int currentPowerup;
    bool poweringUp = false;
    GameObject player;
    Rigidbody2D playerRB;
    DoggyAttributes doggyAttributes;

    void Awake() {
        doggyAttributes = FindObjectOfType<DoggyAttributes>();
        player = GameObject.FindWithTag("Doggy");
        playerController = FindObjectOfType<PlayerController>();
        playerRB = player.GetComponent<Rigidbody2D>();
        powerUpSlider.maxValue = toPowerUp[0];
    }

    void Start() {
        Debug.Log(doggyAttributes.powerupCount);
        Debug.Log(doggyAttributes.runSpeed);
        currentPowerup = doggyAttributes.powerup;
        onUseIcon.sprite = Icons[currentPowerup];
        powerUpSlider.value = 0;
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
        doggyAttributes.powerup++;
        playerController.canTakeDamage = false;
        poweringUp = true;
        playerRB.velocity = Vector3.zero;
        playerController.stopMovement = true;
        playerController.ChangeAnimator(currentPowerup);
        if(currentPowerup == 0){
            playerController.movementSpeed = 7f;
            doggyAttributes.runSpeed = 7f;
        }
        else if(currentPowerup == 1){
            playerController.canFire = true;
            doggyAttributes.canFire = true;
        }
        else{
            
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
        doggyAttributes.powerupCount = 0;
        playerController.stopMovement = false;
        poweringUp = false;
        playerController.canTakeDamage = true;
        //change player sprite
    }

}
