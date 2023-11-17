using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] Animator shipGateAnim;
    [SerializeField] Animator pathDisapperAnim;
    [SerializeField] GameObject newPathObj;
    [SerializeField] Transform doggy;
    [SerializeField] CinemachineVirtualCamera shipCamera;
    [SerializeField] CinemachineVirtualCamera newPathCamera;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject spaceShip;
    PlayerController playerController;
    Rigidbody2D playerRB;
    Animator playerAnimator;
    bool inCutScene = false;
    bool pathActivated = false;
    private void Awake() {
        playerController = doggy.GetComponent<PlayerController>();
        playerRB = doggy.GetComponent<Rigidbody2D>();
        playerAnimator = doggy.GetComponent<Animator>();
    }

    void Update()
    {
        if(doggy==null){return;}
        if(boss==null && !pathActivated){
            StartCoroutine(NewPathActivate());
        }
        if(spaceShip==null){return;}
        if(Mathf.Abs(doggy.position.x - spaceShip.transform.position.x)< 8 && !inCutScene){
            shipCamera.Priority = 11;
            shipGateAnim.enabled = true;
            playerController.stopMovement = true;
            playerRB.velocity = Vector2.zero;
            StartCoroutine(GoToShip());
        }

    }

    IEnumerator NewPathActivate()
    {
        pathActivated = true;
        // We are waiting for also the rockets to be destroyed for 4 sec
        yield return new WaitForSeconds(4);
        playerController.stopMovement=true;
        playerRB.velocity = Vector2.zero;
        playerAnimator.SetBool("isRunning", false);
        newPathCamera.Priority = 11;
        yield return new WaitForSeconds(2);
        newPathObj.SetActive(true);
        pathDisapperAnim.enabled = true;
        yield return new WaitForSeconds(3);
        newPathCamera.Priority = 9;
        playerController.stopMovement = false;

    }

    IEnumerator GoToShip()
    {
        inCutScene = true;
        playerAnimator.SetBool("isRunning", false);
        yield return new WaitForSeconds(2);
        playerAnimator.SetBool("isRunning", true);
        playerRB.velocity = Vector2.right*6;
        playerRB.gravityScale = 0f;
    }
}
