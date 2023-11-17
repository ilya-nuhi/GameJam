using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] Animator shipGateAnim;
    [SerializeField] Transform doggy;
    [SerializeField] GameObject doggyInShip;

    void Update()
    {
        if(doggy==null){return;}
        if(Mathf.Abs(doggy.position.x - transform.position.x)< 5){
            shipGateAnim.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Doggy"){
            Destroy(other.gameObject);
            doggyInShip.SetActive(true);
        }
    }
}
