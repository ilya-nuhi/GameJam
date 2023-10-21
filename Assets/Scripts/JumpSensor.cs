using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSensor : MonoBehaviour
{
    public bool isTouchingGround;
    float waitJump = 0;
    void Update()
    {
        waitJump -= Time.deltaTime;
    }
    public bool canJump(){
        if(waitJump>0){return false;}
        if(isTouchingGround){
            return true;
        }
        else{
            return false;
        }

    }
    void OnTriggerEnter2D(Collider2D other) {
        isTouchingGround = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        isTouchingGround = false;    
    }

    void WaitForJump(){
        waitJump = 0.2f;
    }

}
