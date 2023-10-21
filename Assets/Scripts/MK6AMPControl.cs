using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MK6AMPControl : MonoBehaviour
{
    [SerializeField] float normalSpeed = 1f;
    [SerializeField] float fastSpeed = 2.5f;   
    [SerializeField] int damage = 20;
    [SerializeField] UnityEngine.Vector2 touchKick = new UnityEngine.Vector2(3f,13f);
    [SerializeField] float distance = 5f;
    [SerializeField] Transform rayPosition;
    [SerializeField] Transform gunPosition;
    [SerializeField] GameObject mkBullet;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    CircleCollider2D myCircleCollider;
    PlayerController playerController;
    float speed =1f;
    GameObject player;
    bool canShoot = true;

    public UnityEngine.Vector2 direction;
    void Awake() {
        playerController = FindObjectOfType<PlayerController>();
        player = playerController.gameObject;
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myCircleCollider = GetComponent<CircleCollider2D>();
    }
    void FixedUpdate() {
        RaycastHit2D hit = Physics2D.Raycast(rayPosition.position, UnityEngine.Vector2.right, distance, LayerMask.GetMask("Doggy"));
        RaycastHit2D hit2 = Physics2D.Raycast(rayPosition.position, UnityEngine.Vector2.left, distance, LayerMask.GetMask("Doggy"));
        if(hit.collider!=null){
            if(myRigidBody.velocity.x < 0){
                FlipEnemyFacing();
            }
            speed = fastSpeed;
            if(canShoot){
                direction = UnityEngine.Vector2.right;
                StartCoroutine(Shoot(direction));
            }
        }
        else if(hit2.collider!=null){
            if(myRigidBody.velocity.x > 0){
                FlipEnemyFacing();
            }
            
            speed = fastSpeed;
            if(canShoot){
                direction = UnityEngine.Vector2.left;
                StartCoroutine(Shoot(direction));
            }
            
        }
        else{
            
            speed = normalSpeed;
        }
    }

    IEnumerator Shoot(UnityEngine.Vector2 direction)
    {
        canShoot = false;
        Instantiate(mkBullet, gunPosition.position ,UnityEngine.Quaternion.identity);
        yield return new WaitForSeconds(.1f);
        Instantiate(mkBullet, gunPosition.position ,UnityEngine.Quaternion.identity);
        yield return new WaitForSeconds(.1f);
        Instantiate(mkBullet, gunPosition.position ,UnityEngine.Quaternion.identity);
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }

    void Update()
    {
        myRigidBody.velocity = new UnityEngine.Vector2 (Mathf.Sign(myRigidBody.transform.localScale.x)*speed,0f);
        FlipEnemyFacingHelper();
    }

    void FlipEnemyFacingHelper()
    {
        if(myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))){
            FlipEnemyFacing();
        }
        if(!myCircleCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))){
            FlipEnemyFacing();
        }
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new UnityEngine.Vector2 (-transform.localScale.x,transform.localScale.y);
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
            playerRB.velocity = new UnityEngine.Vector2(-touchKick.x,touchKick.y);
        }
        else{
            playerRB.velocity = touchKick;
        }
        yield return new WaitForSeconds(0.5f);
        playerController.stopMovement = false;
    }
}
