using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class MosquitoControl : MonoBehaviour
{
    [SerializeField] float shootDistance;
    [SerializeField] BulletMosq Bullet;
    [SerializeField] Transform bulletPos;
    [SerializeField] int damage=20;
    [SerializeField] Vector2 touchKick = new Vector2(2f,8f);
    [SerializeField] float aiActivationDist = 15;
    Transform trackingObj;
    AIPath aIPath;
    AIDestinationSetter aIDestinationSetter;
    GameObject player;
    PlayerController playerController;
    EnemyHealth enemyHealth;

    bool canShoot = true;

    void Awake() {
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        enemyHealth = GetComponent<EnemyHealth>();
        aIPath = GetComponent<AIPath>();
        aIPath.enabled = false;
    }

    void Start() {
        playerController = FindObjectOfType<PlayerController>();
        trackingObj = GameObject.Find("Doggy").transform;
        SceneManager.activeSceneChanged+=OnSceneChanged;
    }

    void OnSceneChanged(Scene arg0, Scene arg1)
    {
        playerController = FindObjectOfType<PlayerController>();
        trackingObj = GameObject.Find("Doggy").transform;
        aIDestinationSetter.target = trackingObj;
    }

    void Update(){
        if(playerController==null){
            Debug.Log("null");
        }
        else{
            Debug.Log("null değil");
        }
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

    // AB = |B-A|

    IEnumerator Shoot()
    {
        
        Vector2 direction = new Vector2(trackingObj.position.x-transform.position.x, trackingObj.position.y-transform.position.y);
        direction.Normalize();
        canShoot = false;
        var bulletTransform = Instantiate(Bullet);
        bulletTransform.transform.position = bulletPos.position;
        bulletTransform.transform.right = -direction;
        yield return new WaitForSeconds(.2f);
        bulletTransform = Instantiate(Bullet);
        bulletTransform.transform.position = bulletPos.position;
        bulletTransform.transform.right = -direction;
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
