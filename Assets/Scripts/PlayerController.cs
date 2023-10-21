using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    Vector2 moveInput;

    Rigidbody2D myRigidBody;

    public Animator myAnimator;

    JumpSensor jumpSensor;
    BoxCollider2D sensorBoxCollider;

    public bool stopMovement = false;

    public bool isDead = false;
    public int powerupCount;

    void Awake() {
        jumpSensor = transform.Find("jump_sensor").GetComponent<JumpSensor>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        sensorBoxCollider = jumpSensor.GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        if(isDead){return;}
        Run();
        FlipSprite();
        //Check if character just started falling
        if (!jumpSensor.isTouchingGround) {
            myAnimator.SetBool("isJumping", true);
        }
        //Check if character just landed on the ground
        if(jumpSensor.isTouchingGround) {
            myAnimator.SetBool("isJumping", false);
        }

        if(sensorBoxCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))){
            jumpSensor.isTouchingGround = true;
            myAnimator.SetBool("isJumping", false);
        }
    }

    void OnMove(InputValue value){
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value){
        if(stopMovement || isDead){return;}
        if(jumpSensor.canJump()){
            myRigidBody.velocity += new Vector2 (0f,jumpSpeed);
            myAnimator.SetBool("isJumping",true);
        }
        
    }

    void Run(){
        if(stopMovement){return;}
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed){
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x),1f);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Powerup"){
            powerupCount++;
            Destroy(other.gameObject);
            //play pickup effect

        }    
    }
}
