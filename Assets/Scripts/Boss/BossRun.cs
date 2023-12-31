using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    public float speed =2.5f;
    public float atackRange = 13f;
    Transform player;
    Rigidbody2D rb;
    BOSSControll boss;
    EnemyHealth enemyHealth;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Doggy").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<BOSSControll>();
        enemyHealth = animator.GetComponent<EnemyHealth>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(enemyHealth.GetHealth()<=0){return;}
        boss.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x,rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed*Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if(Vector2.Distance(player.position,rb.position)<= atackRange){
            if(boss.canAttack){
                boss.Attack();
            }
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
