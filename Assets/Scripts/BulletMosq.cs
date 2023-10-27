using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BulletMosq : MonoBehaviour
{
    [SerializeField] float shootSpeed = 10f;
    [SerializeField] int damage = 20;
    Rigidbody2D myRigidBody;
    float bulletLifeTime = 4f;
    GameObject player;
    MosquitoControl mosquitoControl;

    UnityEngine.Vector2 direction;
    void Awake() {
        mosquitoControl = FindObjectOfType<MosquitoControl>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Start() {
        direction = mosquitoControl.direction;
        transform.localScale = new UnityEngine.Vector3 (MathF.Sign(mosquitoControl.transform.localScale.x)*transform.localScale.x,transform.localScale.y,transform.localScale.z);
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
}
