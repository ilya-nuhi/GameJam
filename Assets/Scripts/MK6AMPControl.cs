using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MK6AMPControl : MonoBehaviour
{
    [SerializeField] float normalSpeed = 1f;
    [SerializeField] float fastSpeed = 2.5f;   
    [SerializeField] int damage = 20;
    [SerializeField] Vector2 touchKick = new Vector2(3f,13f);
    [SerializeField] float distance = 5f;
    [SerializeField] Transform rayPosition;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    CircleCollider2D myCircleCollider;
    PlayerController playerController;
    float speed =1f;
    float flipTime = 0f;
    GameObject player;
    void Awake() {
        playerController = FindObjectOfType<PlayerController>();
        player = playerController.gameObject;
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        myCircleCollider = GetComponent<CircleCollider2D>();
    }
    void FixedUpdate() {
        flipTime -= Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(rayPosition.position, Vector2.right, distance, LayerMask.GetMask("Doggy"));
        RaycastHit2D hit2 = Physics2D.Raycast(rayPosition.position, Vector2.left, distance, LayerMask.GetMask("Doggy"));
        if(hit.collider!=null){
            if(transform.localPosition.x <0 && flipTime<0){
                flipTime = 1.5f;
                FlipEnemyFacing();
            }
            speed = fastSpeed;
        }
        else if(hit2.collider!=null){
            if(transform.localPosition.x >0 && flipTime<0){
                flipTime = 1.5f;
                FlipEnemyFacing();
            }
            
            speed = fastSpeed;
        }
        else{
            
            speed = normalSpeed;
        }
    }
    void Update()
    {
        myRigidBody.velocity = new Vector2 (Mathf.Sign(myRigidBody.transform.localScale.x)*speed,0f);
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
        transform.localScale = new Vector2 (-transform.localScale.x,transform.localScale.y);
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
