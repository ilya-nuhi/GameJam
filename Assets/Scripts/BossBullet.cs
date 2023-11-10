using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] float shootSpeed = 5f;
    [SerializeField] int damage = 20;

    [SerializeField] float rotationSpeed = 200f;
    [SerializeField] AudioClip bossBullet;
    Rigidbody2D myRigidBody;
    float bulletLifeTime = 4f;
    GameObject player;

    AudioSource audioSource;

    int flip=1;

    void Awake() {
        AudioSource.PlayClipAtPoint(bossBullet,transform.position);
        player = GameObject.Find("Doggy");
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        if(player.transform.position.x > transform.position.x){
            transform.localScale = new UnityEngine.Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }
        else{
            flip=-1;
        }

    }

    void Update()
    {
        UnityEngine.Vector2 targetDirection = new UnityEngine.Vector2(player.transform.position.x-transform.position.x, player.transform.position.y-transform.position.y);
        UnityEngine.Vector2 bulletDirection = flip*transform.right;
        UnityEngine.Vector3 crossProduct = UnityEngine.Vector3.Cross(targetDirection, bulletDirection);

        myRigidBody.angularVelocity = -crossProduct.z*rotationSpeed;
        myRigidBody.velocity = bulletDirection*shootSpeed;
        bulletLifeTime -= Time.deltaTime;
        if(bulletLifeTime<=0){
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "BossBullet"){return;}
        if(other.tag == "Doggy" ){
            Health playerHealth = player.GetComponent<Health>();
            playerHealth.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
