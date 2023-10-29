using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    Animator myAnimator;

    PlayerController playerController;
    public int currentHealth;
    Singleton singleton;

    void Awake() {
        singleton = GetComponent<Singleton>();
        myAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        currentHealth = maxHealth;
    }

    void Start() {
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
        yield return new WaitForSeconds(3);
        playerController.transform.position = singleton.respawnPoint.position;
        currentHealth = maxHealth;
        singleton.lives--;
        if(singleton.lives>0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else{
            singleton.OnGameOver();
            SceneManager.LoadScene(0);
        }
    }


}
