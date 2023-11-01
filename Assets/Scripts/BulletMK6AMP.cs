using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMK6AMP : MonoBehaviour
{
    [SerializeField] float shootSpeed = 10f;
    [SerializeField] int damage = 20;
    Rigidbody2D myRigidBody;
    float bulletLifeTime = 4f;
    GameObject player;
    Vector2 direction;
    PlayerController playerController;

    void Awake() {
        playerController = FindObjectOfType<PlayerController>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    void Start() {
        direction = new Vector2 (MathF.Sign(transform.position.x - playerController.transform.position.x), 0);
        transform.localScale = new Vector3 ((direction.x)*transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void Update()
    {
        bulletLifeTime -= Time.deltaTime;
        myRigidBody.velocity = -1*direction*shootSpeed;
        if(bulletLifeTime<=0){
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Doggy" ){
            player = other.gameObject;
            Health playerHealth = player.GetComponent<Health>();
            playerHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
        
    }
}
