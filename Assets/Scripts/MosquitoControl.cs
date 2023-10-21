using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoControl : MonoBehaviour
{
    [SerializeField] Transform trackingObj;
    [SerializeField] float shootDistance;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform bulletPos;
    [SerializeField] int damage=20;
    [SerializeField] Vector2 touchKick = new Vector2(2f,8f);

    public Vector2 direction;

    GameObject player;
    PlayerController playerController;

    //Animator myAnimator;

    bool canShoot = true;

    void Awake() {
        playerController = FindObjectOfType<PlayerController>();
        //myAnimator = GetComponent<Animator>();    
    }

    void Update(){
        if(trackingObj.position.x < transform.position.x && transform.localScale.x > 0 ){
            transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
        }
        else if(trackingObj.position.x > transform.position.x && transform.localScale.x < 0 ){
            transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
        }
        float distance = Vector2.Distance(new Vector2 (transform.position.x,transform.position.y),new Vector2 (trackingObj.position.x,trackingObj.position.y));
        if(distance<shootDistance && canShoot){
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        direction = new Vector2(trackingObj.position.x-transform.position.x,trackingObj.position.y-transform.position.y);
        direction.Normalize();
        canShoot = false;
        //myAnimator.SetTrigger("Shooting");
        Instantiate(Bullet,bulletPos.position,Quaternion.identity);
        yield return new WaitForSeconds(.2f);
        Instantiate(Bullet,bulletPos.position,Quaternion.identity);
        yield return new WaitForSeconds(1);
        canShoot = true;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Doggy")){
            player = other.gameObject;
            StartCoroutine(DamagePlayer(player));
        }
    }

    IEnumerator DamagePlayer(GameObject myPlayer){
        playerController.stopMovement = true;
        //animator.SetTrigger("Spiked");
        Health playerHealth = myPlayer.GetComponent<Health>();
        playerHealth.TakeDamage(damage);
        playerController.myAnimator.SetTrigger("Ouch");
        Rigidbody2D playerRB = myPlayer.GetComponent<Rigidbody2D>();
        if(myPlayer.transform.position.x<=transform.position.x){
            playerRB.velocity = new Vector2(-touchKick.x,touchKick.y);
        }
        else{
            playerRB.velocity = touchKick;
        }
        yield return new WaitForSeconds(0.5f);
        playerController.stopMovement = false;
    }
}
