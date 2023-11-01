using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    Animator myAnimator;

    PlayerController playerController;
    public int currentHealth;
    Singleton singleton;

    SliderControl sliderControl;

    DoggyAttributes doggyAttributes;

    void Awake() {
        sliderControl = FindObjectOfType<SliderControl>();
        doggyAttributes = FindObjectOfType<DoggyAttributes>();
        singleton = FindObjectOfType<Singleton>();
        myAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        currentHealth = maxHealth;
    }

    void Start() {
        sliderControl.AttachHealth();
    }

    public void TakeDamage(int damage){
        if(!playerController.canTakeDamage){
            return;
        }
        currentHealth -= damage;
        if(currentHealth<=0){
            StartCoroutine(Die());
        }
    }

    public int GetHealth(){
        return currentHealth;
    }

    public IEnumerator Die(){
        playerController.isDead = true;
        myAnimator.SetTrigger("isDead");
        yield return new WaitForSeconds(2);
        doggyAttributes.lives--;
        if(doggyAttributes.lives>0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else{
            singleton.OnGameOver();
            singleton = null;
            SceneManager.LoadScene(0);
        }
    }


}
