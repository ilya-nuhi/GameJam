using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogSpit : MonoBehaviour
{
    [SerializeField] float spitSpeed = 10f;
    [SerializeField] int damage = 20;
    [SerializeField] AudioClip spittingSoundFX;
    Rigidbody2D myRigidBody;
    float xSpeed;
    float bulletLifeTime = 4f;

    GameObject player;
    PlayerController playerController;
    void Awake() {
        playerController = FindObjectOfType<PlayerController>();    
    }
    void Start()
    {
        AudioSource.PlayClipAtPoint(spittingSoundFX,transform.position);
        myRigidBody = GetComponent<Rigidbody2D>();
        xSpeed =  -1*Mathf.Sign(transform.position.x - playerController.gameObject.transform.position.x)* spitSpeed;
    }

    void Update()
    {
        bulletLifeTime -= Time.deltaTime;
        myRigidBody.velocity = new Vector2 (xSpeed,0f);
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
