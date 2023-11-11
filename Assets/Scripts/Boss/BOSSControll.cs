using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSControll : MonoBehaviour
{
    public Transform player;
    [SerializeField] GameObject bulletBoss;
    [SerializeField] Transform[] gunBoss;
    [SerializeField] Vector2 touchKick = new Vector2(3f,13f);
    [SerializeField] int damage = 20;
    AudioSource audioSource;
    Animator myAnimator;
    PlayerController playerController;
    EnemyHealth enemyHealth;
    public bool canAttack=true;
	public bool isFlipped = false;
    public Vector2 direction;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        playerController = FindObjectOfType<PlayerController>();
        enemyHealth = GetComponent<EnemyHealth>();
        myAnimator = GetComponent<Animator>();
    }
    private void Start() {
        enemyHealth.enabled = false;
    }
	public void LookAtPlayer()
	{
        if(enemyHealth.GetHealth()<=0){return;}
		//Vector3 flipped = transform.localScale;
		//flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			//transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = false;
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			//transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = true;
		}
	}

    public void Attack(){
        if(enemyHealth.GetHealth()<=0){return;}
        StartCoroutine(startAttack());
    }

    void Update() {
        //direction = new Vector2(player.position.x-transform.position.x,player.position.y-transform.position.y);
        //direction.Normalize();
    }

    IEnumerator startAttack()
    {
        canAttack = false;
        Instantiate(bulletBoss, gunBoss[0].position, Quaternion.identity);
        Instantiate(bulletBoss, gunBoss[1].position, Quaternion.identity);
        yield return new WaitForSeconds(2);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        StartCoroutine(WakeUp());
    }

    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(3);
        audioSource.Stop();
        enemyHealth.enabled = true;
        myAnimator.SetTrigger("WakeUp");
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Doggy")){
            StartCoroutine(DamagePlayer());
            StartCoroutine(WakeUp());
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
