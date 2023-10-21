using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogControl : MonoBehaviour
{
    [SerializeField] float distance = 10f;
    [SerializeField] Transform mouth;
    [SerializeField] GameObject spit;
    [SerializeField] Vector2 touchKick = new Vector2(2f,8f);
    [SerializeField] int damage = 20;
    Animator myAnimator;

    GameObject player;

    bool canSpit = true;
    PlayerController playerController;

    void Awake() {
        playerController = FindObjectOfType<PlayerController>();
        myAnimator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Mathf.Sign(transform.localScale.x)* new Vector2(1,0), distance, LayerMask.GetMask("Doggy"));
        if(hit.collider!=null && canSpit){
            StartCoroutine(Spit());
        }

    }

    IEnumerator Spit(){
        canSpit = false;
        myAnimator.SetTrigger("Spit");
        Instantiate(spit,mouth.position,Quaternion.identity);
        yield return new WaitForSeconds(.2f);
        Instantiate(spit,mouth.position,Quaternion.identity);
        yield return new WaitForSeconds(1);
        canSpit = true;
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
