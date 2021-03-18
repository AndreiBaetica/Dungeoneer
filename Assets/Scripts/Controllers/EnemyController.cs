using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MovementController
{
    private float lookRadius = 4.5f;
    private Transform target;
    
    private BehaviourState currentBehaviourState = BehaviourState.idle;

    private enum BehaviourState
    {
        idle,
        chase,
        attack
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
        stateSwitch();

        if (currentBehaviourState == BehaviourState.idle)
        {
            //Idle behaviour
        } else if (currentBehaviourState == BehaviourState.chase)
        {
            //Chase behaviour

            float distance = Vector3.Distance(target.position, transform.position);

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
            }
        } else if (currentBehaviourState == BehaviourState.attack)
        {
            //attack behaviour
        }
    }

    //stub, fill out the commented if statements with real logic
    private void stateSwitch()
    {
        if (currentBehaviourState == BehaviourState.idle)
        {
            /*if (distance to player <= lookRadius)
            {
                currentBehaviourState = BehaviourState.chase;
            }*/
        } else if (currentBehaviourState == BehaviourState.chase)
        {
            /*if (distance to player > lookRadius)
            {
                currentBehaviourState = BehaviourState.idle;
            } else if (distance to player <= 0.5f)
            {
                currentBehaviourState = BehaviourState.attack;
            }*/
        } else if (currentBehaviourState == BehaviourState.attack)
        {
            /*if (distance to player > 0.5f)
            {
                currentBehaviourState = BehaviourState.chase;
            }*/
        }
    }

}
