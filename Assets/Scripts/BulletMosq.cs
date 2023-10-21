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


    void Awake() {
        mosquitoControl = FindObjectOfType<MosquitoControl>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bulletLifeTime -= Time.deltaTime;
        myRigidBody.velocity = new UnityEngine.Vector2 (mosquitoControl.direction.x*shootSpeed, mosquitoControl.direction.y*shootSpeed);
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
