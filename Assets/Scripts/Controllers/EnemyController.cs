using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MovementController
{
    private float lookRadius = 4.5f;
    private Transform target;
    
    private BehaviourState currentBehaviourState = BehaviourState.Idle;

    private enum BehaviourState
    {
        Idle,
        Chase,
        Attack
    }

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform; //target.position player Position; transform.position for enemy position
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //We absolutely can't have these methods being called once per frame.
        //Leaving them here until we have a timer system. Then we will only call them once per turn.
        StateSwitch();
        Debug.Log(currentBehaviourState);

        if (currentBehaviourState == BehaviourState.Idle)
        {
            //Idle behaviour
        } else if (currentBehaviourState == BehaviourState.Chase)
        {
            //Chase behaviour

            if (moving) snapToGridSquare();
            else
            {
                string direction = FindBestMovement();
                if (direction.Equals("fwd")) MoveForward();
                else if (direction.Equals("back")) MoveBack();
                else if (direction.Equals("left")) MoveLeft();
                else if (direction.Equals("right")) MoveRight();
            }
            
            /*float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                if (moving) snapToGridSquare();

                if (Math.Round(transform.position.z) < Math.Round(target.position.z))
                {
                    //Player is above
                    moveUp();
                }

                if (Math.Round(transform.position.x) < Math.Round(target.position.x))
                {
                    //Player is to the right
                    moveRight();
                }

                if (Math.Round(transform.position.z) > Math.Round(target.position.z))
                {
                    //Player is below
                    moveDown();

                }

                if (Math.Round(transform.position.x) > Math.Round(target.position.x))
                {
                    //Player is to the left
                    moveLeft();
                }
            }*/
        } else if (currentBehaviourState == BehaviourState.Attack)
        {
            //attack behaviour
        }
    }

    private void StateSwitch()
    {
        if (currentBehaviourState == BehaviourState.Idle)
        {
            if (Vector3.Distance(target.position, transform.position) <= lookRadius)
            {
                currentBehaviourState = BehaviourState.Chase;
            }
        } else if (currentBehaviourState == BehaviourState.Chase)
        {
            if (Vector3.Distance(target.position, transform.position) > lookRadius)
            {
                currentBehaviourState = BehaviourState.Idle;
            } else if (Vector3.Distance(target.position, transform.position) <= 1f)
            {
                currentBehaviourState = BehaviourState.Attack;
            }
        } else if (currentBehaviourState == BehaviourState.Attack)
        {
            if (Vector3.Distance(target.position, transform.position) > 1f)
            {
                currentBehaviourState = BehaviourState.Chase;
            }
        }
    }
    
    //gives the direction that the enemy should move in to get closer to the player
    private string FindBestMovement()
    {
        //none should never happen
        string bestMovement = "none";
        float distanceToPlayer;
        float shortestDistanceToPlayer = 5f;
        
        //up
        if (canMove(Vector3.forward))
        {
            distanceToPlayer = Vector3.Distance((transform.position + Vector3.forward), target.position);
            if (distanceToPlayer < shortestDistanceToPlayer)
            {
                shortestDistanceToPlayer = distanceToPlayer;
                bestMovement = "fwd";
            }
        }
        //down
        if (canMove(Vector3.back))
        {
            distanceToPlayer = Vector3.Distance((transform.position + Vector3.back), target.position);
            if (distanceToPlayer < shortestDistanceToPlayer)
            {
                shortestDistanceToPlayer = distanceToPlayer;
                bestMovement = "back";
            }
        }
        //left
        if (canMove(Vector3.left))
        {
            
            distanceToPlayer = Vector3.Distance((transform.position + Vector3.left), target.position);
            if (distanceToPlayer < shortestDistanceToPlayer)
            {
                shortestDistanceToPlayer = distanceToPlayer;
                bestMovement = "left";
            }
        }
        //right
        if (canMove(Vector3.right))
        {
            distanceToPlayer = Vector3.Distance((transform.position + Vector3.right), target.position);
            if (distanceToPlayer < shortestDistanceToPlayer)
            {
                bestMovement = "right";
            }
        }

        return bestMovement;
    }

}
