using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    Animator myAnimator;

    int currentHealth;

    void Awake() {
        myAnimator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Start() {
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth<=0){
            StartCoroutine(Die());
        }
    }

    public int GetHealth(){
        return currentHealth;
    }

    IEnumerator Die(){
        PlayerController playerController = GetComponent<PlayerController>();
        playerController.isDead = true;
        myAnimator.SetTrigger("isDead");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
