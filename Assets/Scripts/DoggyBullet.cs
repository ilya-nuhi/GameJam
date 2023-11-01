using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class DoggyBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] int damage = 20;
    Rigidbody2D myRigidBody;
    float bulletLifeTime = 4f;
    PlayerController playerController;
    void Awake() {
        playerController = FindObjectOfType<PlayerController>();    
    }
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRigidBody.velocity = new UnityEngine.Vector2 (playerController.transform.localScale.x , 0f)*bulletSpeed;
        transform.localScale = new UnityEngine.Vector3(-1*playerController.transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void Update()
    {
        bulletLifeTime -= Time.deltaTime;
        if(bulletLifeTime<=0){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);    
    }
}
