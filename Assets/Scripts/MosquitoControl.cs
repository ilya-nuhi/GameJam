using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MosquitoControl : MonoBehaviour
{
    [SerializeField] float shootDistance;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform bulletPos;
    [SerializeField] int damage=20;
    [SerializeField] Vector2 touchKick = new Vector2(2f,8f);
    [SerializeField] float aiActivationDist = 15;
    Transform trackingObj;
    AIPath aIPath;

    public Vector2 direction;

    GameObject player;
    PlayerController playerController;
    EnemyHealth enemyHealth;

    bool canShoot = true;

    void Awake() {
        enemyHealth = GetComponent<EnemyHealth>();
        aIPath = GetComponent<AIPath>();
        aIPath.enabled = false;
    }

    void Start() {
        playerController = FindObjectOfType<PlayerController>();
        trackingObj = GameObject.Find("Doggy").transform;
    }

    void Update(){
        if(trackingObj==null){return;}
        if(enemyHealth.stopEnemy){
            aIPath.maxSpeed = 0;
            canShoot = false;
        }
        if(MathF.Abs(trackingObj.transform.position.x-transform.position.x)<aiActivationDist){
            aIPath.enabled = true;
        }
        if(trackingObj.position.x < transform.position.x && transform.localScale.x < 0 ){
            transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }
        else if(trackingObj.position.x > transform.position.x && transform.localScale.x > 0 ){
            transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
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
        Instantiate(Bullet,bulletPos.position, Quaternion.Euler(0,0,MathF.Atan(direction.y/direction.x)*Mathf.Rad2Deg));
        yield return new WaitForSeconds(.2f);
        Instantiate(Bullet,bulletPos.position, Quaternion.Euler(0,0,MathF.Atan(direction.y/direction.x)*Mathf.Rad2Deg));
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
