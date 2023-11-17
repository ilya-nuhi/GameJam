using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] ParticleSystem deathEffect;
    [SerializeField] ParticleSystem impactEffect;
    PlayerController playerController;

    public bool stopEnemy = false;

    void Awake() {
        playerController = FindObjectOfType<PlayerController>();    
    }
    void Start() {
        SceneManager.activeSceneChanged += OnSceneLoad;
    }

    void OnSceneLoad(Scene arg0, Scene arg1)
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update() {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(!isActiveAndEnabled){return;}
        if(other.tag=="PlayerBullet"){
            Instantiate(impactEffect, transform.position, Quaternion.identity);
            Health -= playerController.power;
            if(Health<=0){
                StartCoroutine(DestroyEnemy());
            }
        }
    }

    IEnumerator DestroyEnemy()
    {
        //Destroy effect
        stopEnemy = true;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public int GetHealth(){
        return Health;
    }
}
