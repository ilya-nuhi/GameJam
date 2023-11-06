using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrogControl : MonoBehaviour
{
    [SerializeField] float distance = 10f;
    [SerializeField] Transform mouth;
    [SerializeField] GameObject spit;
    [SerializeField] Vector2 touchKick = new Vector2(2f,8f);
    [SerializeField] int damage = 20;

    GameObject player;

    bool canSpit = true;
    EnemyHealth enemyHealth;
    PlayerController playerController;


    void Awake() {
        enemyHealth = GetComponent<EnemyHealth>();
    }
    void Start() {
        player = GameObject.Find("Doggy");
        playerController = player.GetComponent<PlayerController>();
        SceneManager.activeSceneChanged+=OnSceneChanged;
    }

    void OnSceneChanged(Scene arg0, Scene arg1)
    {
        playerController = FindObjectOfType<PlayerController>();
        player = GameObject.Find("Doggy");
    }
    void FixedUpdate() {
        RaycastHit2D hit = Physics2D.Raycast(mouth.position, UnityEngine.Vector2.right, distance, LayerMask.GetMask("Doggy"));
        RaycastHit2D hit2 = Physics2D.Raycast(mouth.position, UnityEngine.Vector2.left, distance, LayerMask.GetMask("Doggy"));
        if(hit.collider!=null){
            transform.rotation = Quaternion.Euler(0,180,0);
            if(canSpit){
                StartCoroutine(Spit());
            }
        }
        else if(hit2.collider!=null){
            transform.rotation = Quaternion.Euler(0,0,0);
            if(canSpit){
                StartCoroutine(Spit());
            }
        }
    }

    void Update() {
        if(enemyHealth.stopEnemy){
            canSpit = false;
        }    
    }
    

    IEnumerator Spit(){
        canSpit = false;
        Instantiate(spit,mouth.position,Quaternion.identity);
        yield return new WaitForSeconds(2);
        canSpit = true;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Doggy")){
            StartCoroutine(DamagePlayer());
        }
    }

    IEnumerator DamagePlayer(){
        playerController.stopMovement = true;
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
