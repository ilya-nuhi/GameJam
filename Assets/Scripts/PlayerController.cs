using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] AudioClip powerupAudio;
    [SerializeField] GameObject doggyBullet;
    [SerializeField] Transform firePosition;
    public int power = 20;
    Vector2 moveInput;

    Rigidbody2D myRigidBody;

    public Animator myAnimator;

    //JumpSensor jumpSensor;
    CapsuleCollider2D jumpSensorCollider;

    public bool stopMovement = false;

    public bool isDead = false;
    public int powerupCount;
    public float movementSpeed;

    public bool canFire = true;
    bool canTrigger = true;

    bool isTouchingGround = false;
    float jumpTime = 0;

    void Awake() {
        
        //jumpSensor = transform.Find("jump_sensor").GetComponent<JumpSensor>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        jumpSensorCollider = GetComponent<CapsuleCollider2D>();

    }
    
    void Start() {
        movementSpeed = runSpeed;
    }

    void Update()
    {
        if(isDead){return;}
        Run();
        FlipSprite();

        if(jumpSensorCollider.IsTouchingLayers(LayerMask.GetMask("Platform"))){
            isTouchingGround = true;
            myAnimator.SetBool("isJumping", false);
        }
        else{
            isTouchingGround = false;
            myAnimator.SetBool("isJumping", true);
        }
        jumpTime -= Time.deltaTime;
    }

    void OnMove(InputValue value){
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value){
        if(stopMovement || isDead){return;}
        if(canJump()){
            myRigidBody.velocity += new Vector2 (0f,jumpSpeed);
            jumpTime = 0.2f;
        }
    }

    private bool canJump()
    {
        if(jumpTime > 0){return false;}
        if(isTouchingGround){
            return true;
        }
        else{
            return false;
        }
    }

    void Run(){
        if(stopMovement){return;}
        Vector2 playerVelocity = new Vector2(moveInput.x * movementSpeed, myRigidBody.velocity.y);
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
        if(other.gameObject.tag=="Powerup" && canTrigger){
            canTrigger = false;
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            powerupCount++;
            AudioSource.PlayClipAtPoint(powerupAudio,transform.position);
            canTrigger = true;
        }
        else if(other.gameObject.tag == "Cliff"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if(other.gameObject.layer == LayerMask.GetMask("NextScene")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

    void OnFire(){
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        Instantiate(doggyBullet, firePosition.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
    }
}
