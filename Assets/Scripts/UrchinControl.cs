using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] Vector2 touchKick = new Vector2(3f,13f);
    [SerializeField] int damage = 20;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    CircleCollider2D myCircleCollider;
    PlayerController playerController;
    GameObject player;

    Animator animator;

    void Awake() {
        playerController = FindObjectOfType<PlayerController>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myCircleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2 (Mathf.Sign(myRigidBody.transform.localScale.x)*movementSpeed,0f);
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
        transform.localScale = new Vector2 (-Mathf.Sign(myRigidBody.velocity.x),1f);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Doggy")){
            player = other.gameObject;
            StartCoroutine(DamagePlayer(player));
        }
        
    }

    IEnumerator DamagePlayer(GameObject player){
        playerController.stopMovement = true;
        animator.SetTrigger("Spiked");
        Health playerHealth = player.GetComponent<Health>();
        playerHealth.TakeDamage(damage);
        playerController.myAnimator.SetTrigger("Ouch");
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
        if(player.transform.position.x<=transform.position.x){
            playerRB.velocity = new Vector2(-touchKick.x,touchKick.y);
        }
        else{
            playerRB.velocity = touchKick;
        }
        yield return new WaitForSeconds(0.5f);
        playerController.stopMovement = false;
    }
}