using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.IO;
using System;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;

    //public Transform enemyGFX;

    Pathfinding.Path path;

    Seeker seeker;
    Rigidbody2D rb;



    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    void OnPathComplete(Pathfinding.Path p)
    {
        if(!p.error){
            path=p;
            currentWayPoint = 0;
        }
    }

    void UpdatePath() {
        if(seeker.IsDone()){
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void FixedUpdate() {
        if (path == null){
            return;
        }

        if(currentWayPoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }
        else{
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if(distance < nextWayPointDistance){
            currentWayPoint++;
        }

        // if(force.x >= 0.01f){
        //     enemyGFX.localScale = new Vector3(-1f,1f,1f);
        // }
        // else if(force.x <= -0.01f){
        //     enemyGFX.localScale = new Vector3(1f,1f,1f);
        // }
    }
}

