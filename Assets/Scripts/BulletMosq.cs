using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BulletMosq : MonoBehaviour
{
    [SerializeField] float shootSpeed = 10f;
    [SerializeField] int damage = 20;
    [SerializeField] AudioClip mosqBullet;
    private Rigidbody2D myRigidBody;
    private float bulletLifeTime = 4f;
    private GameObject player;



    UnityEngine.Vector2 direction;
    void Awake() {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Start() {
        AudioSource.PlayClipAtPoint(mosqBullet ,transform.position);
        direction = -transform.right;
    }

    void Update()
    {
        bulletLifeTime -= Time.deltaTime;
        myRigidBody.velocity = new UnityEngine.Vector2 (direction.x*shootSpeed, direction.y*shootSpeed);
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

    public void SetDamage(int damage){
        this.damage = damage;
    }
}
