using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] AudioClip powerupAudio;
    [SerializeField] GameObject doggyBullet;
    [SerializeField] Transform firePosition;
    [SerializeField] RuntimeAnimatorController[] powerAnims;
    [SerializeField] float detectRadius = 20f;
    [SerializeField] AudioClip barkingSFX;
    [SerializeField] Animator nextSceneAnimator;
    public int power = 20;
    Vector2 moveInput;

    Rigidbody2D myRigidBody;

    public Animator myAnimator;

    public bool stopMovement = false;

    public bool isDead = false;
    public int powerupCount;
    public bool canFire = false;
    public bool canFly = false;
    bool canFlap = false;
    bool canTrigger = true;
    bool isTouchingGround = false;
    float jumpTime = 0;
    public bool canTakeDamage = true;
    DoggyAttributes doggyAttributes;
    Health health;
    Transform jumpRay;
    int beforeOverlapCircle=0;
    bool canBark=true;

    void Awake() {
        jumpRay = transform.Find("jumpingRay");
        health = GetComponent<Health>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    
    void Start() {
        doggyAttributes = FindObjectOfType<DoggyAttributes>();
        myAnimator = GetComponent<Animator>();
        ChangeAnimator(doggyAttributes.powerup);
        movementSpeed = doggyAttributes.runSpeed;
        powerupCount = doggyAttributes.powerupCount;
        power = doggyAttributes.power;
        canFire = doggyAttributes.canFire;
        canFly = doggyAttributes.canFly;
    }

    void Update()
    {
        if(isDead){return;}
        Run();
        FlipSprite();
        jumpTime -= Time.deltaTime;

        RaycastHit2D hitBottom = Physics2D.Raycast(jumpRay.position, Vector2.down, Mathf.Epsilon , LayerMask.GetMask("Platform"));
        if(hitBottom.collider!=null){
            isTouchingGround = true;
            canFlap = true;
            myAnimator.SetBool("isJumping", false);
        }
        else{
            isTouchingGround = false;
            myAnimator.SetBool("isJumping", true);
        }
        int currentOverlapCircle = Physics2D.OverlapCircleAll(transform.position, detectRadius, LayerMask.GetMask("Enemy")).Length;
        if( currentOverlapCircle > beforeOverlapCircle && canBark){
            StartCoroutine(Bark());
        }
        beforeOverlapCircle = currentOverlapCircle;
    }

    IEnumerator Bark()
    {
        canBark = false;
        AudioSource.PlayClipAtPoint(barkingSFX, transform.position);
        yield return new WaitForSeconds(1);
        canBark = true;
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
        else if(canFly && canFlap){
            canFlap = false;
            myRigidBody.velocity = new Vector2 (myRigidBody.velocity.x,0f);
            myRigidBody.velocity += new Vector2 (0f,jumpSpeed);
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
            doggyAttributes.powerupCount++;
            if(powerupCount<5){
                AudioSource.PlayClipAtPoint(powerupAudio,transform.position);
            }
            canTrigger = true;
        }
        else if(other.gameObject.tag == "Cliff"){
            StartCoroutine(health.Die());
        }
        else if(other.gameObject.layer == LayerMask.GetMask("NextLevel")){
            // if all the powerups are collected we can pass to the next level.
            if(doggyAttributes.powerup + 2 > SceneManager.GetActiveScene().buildIndex){
                nextSceneAnimator.SetTrigger("SceneEnd");
            }
        }
    }

    void OnFire(){
        if(!canFire){return;}
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        canFire = false;
        Instantiate(doggyBullet, firePosition.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        canFire = true;
    }

    public void ChangeAnimator(int animNum){
        myAnimator.runtimeAnimatorController = powerAnims[animNum];
    }
}
