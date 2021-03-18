using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MovementController
{
    [SerializeField] private float speed = 10f;

    private float lookRadius = 4.5f;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform; //target.position player Position; transform.position for enemy position
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius )
        {
            if (moving) snapToGridSquare();

            if (Math.Round(transform.position.z) < Math.Round(target.position.z))
            { //Player is above
                moveUp();
            }
             if (Math.Round(transform.position.x) < Math.Round(target.position.x))
            { //Player is to the right
                moveRight();
            }
             if (Math.Round(transform.position.z) > Math.Round(target.position.z))
            { //Player is below
                moveDown();

            } if (Math.Round(transform.position.x) > Math.Round(target.position.x))
            { //Player is to the left
                moveLeft();
            }
        }
    }



}
