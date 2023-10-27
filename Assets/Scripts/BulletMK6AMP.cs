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
    MK6AMPControl mK6AMPControl;
    Vector2 direction;

    void Awake() {
        mK6AMPControl = FindObjectOfType<MK6AMPControl>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    void Start() {
        direction = mK6AMPControl.direction;
        transform.localScale = new Vector3 (mK6AMPControl.gameObject.transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void Update()
    {
        bulletLifeTime -= Time.deltaTime;
        myRigidBody.velocity = direction*shootSpeed;
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
