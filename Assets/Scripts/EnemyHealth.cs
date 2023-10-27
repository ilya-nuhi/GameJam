using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int Health;
    PlayerController playerController;

    void Awake() {
        playerController = FindObjectOfType<PlayerController>();    
    }

    void OnTriggerEnter2D(Collider2D other) {
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
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public int GetHealth(){
        return Health;
    }
}
