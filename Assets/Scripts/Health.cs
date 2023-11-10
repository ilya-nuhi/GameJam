using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] AudioClip dyingSFX;
    [SerializeField] AudioClip takeDamageSFX;
    public int maxHealth = 100;
    Animator myAnimator;

    PlayerController playerController;
    public int currentHealth;
    Singleton singleton;
    SingletonLevel singletonLevel;

    SliderControl sliderControl;

    DoggyAttributes doggyAttributes;

    void Awake() {
        singletonLevel = FindObjectOfType<SingletonLevel>();
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
        else{
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        playerController.canTakeDamage = false;
        AudioSource.PlayClipAtPoint(takeDamageSFX,transform.position);
        yield return new WaitForSeconds(0.8f);
        playerController.canTakeDamage = true;
    }

    public int GetHealth(){
        return currentHealth;
    }

    public IEnumerator Die(){
        playerController.isDead = true;
        myAnimator.SetTrigger("isDead");
        AudioSource.PlayClipAtPoint(dyingSFX,transform.position);
        yield return new WaitForSeconds(2);
        Destroy(playerController.gameObject);
        doggyAttributes.lives--;
        if(doggyAttributes.lives>0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else{
            singleton.OnGameOver();
            singletonLevel.DestroyLevel();
            singleton = null;
            SceneManager.LoadScene(0);
        }
    }


}
