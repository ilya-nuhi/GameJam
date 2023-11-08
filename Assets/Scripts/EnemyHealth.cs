using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int Health;
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
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public int GetHealth(){
        return Health;
    }
}
