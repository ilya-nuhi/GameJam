using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{   
    [SerializeField] AudioClip audioClip;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider2D;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Doggy"){
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
            StartCoroutine(PickedUp());
        }
    }

    IEnumerator PickedUp()
    {
        spriteRenderer.enabled = false;
        circleCollider2D.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);

    }
}
