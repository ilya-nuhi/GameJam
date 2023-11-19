using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetInShip : MonoBehaviour
{
    [SerializeField] GameObject doggyInShip;
    [SerializeField] float velocity=0.3f;
    [SerializeField] GameObject rocketFlames;
    [SerializeField] AudioClip rocketEngineSFX;
    [SerializeField] Animator fadeOut;
    Singleton singleton;
    Rigidbody2D shipRB;
    AudioSource audioSource;
    bool launched = false;
    private void Awake() {
        singleton = FindObjectOfType<Singleton>();
        audioSource = GetComponent<AudioSource>();
        shipRB = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Doggy"){
            Destroy(other.gameObject);
            doggyInShip.SetActive(true);
            StartCoroutine(LaunchShip());
        }
    }

    private void Update() {
        if(launched){
            shipRB.velocity += UnityEngine.Vector2.up*velocity;
        }
    }

    IEnumerator LaunchShip()
    {
        yield return new WaitForSeconds(2);
        rocketFlames.SetActive(true);
        audioSource.PlayOneShot(rocketEngineSFX);
        launched = true;
        yield return new WaitForSeconds(6);
        fadeOut.enabled = true;
        yield return new WaitForSeconds(1.5f);
        singleton.OnGameOver();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        
    }
}
