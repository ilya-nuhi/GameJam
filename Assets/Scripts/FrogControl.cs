using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogControl : MonoBehaviour
{
    [SerializeField] float distance = 10f;
    [SerializeField] Transform mouth;
    [SerializeField] GameObject spit;
    [SerializeField] Vector2 touchKick = new Vector2(2f,8f);
    Animator myAnimator;

    bool canSpit = true;

    void Awake() {
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

}
