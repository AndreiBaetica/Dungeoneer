using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private enemyDir dirFacing = enemyDir.Up;
    private float lookRadius = 4.5f;
    Transform target;
    NavMeshAgent agent;


    public Animator animator;
    //public bool isTurn;

    enum enemyDir
    {
        Up,
        Down,
        Left,
        Right
    }

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform; //target.position player Position; transform.position for enemy position
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {


        float distance = Vector3.Distance(target.position, transform.position);
        Debug.Log("Distance: " + distance);
        Debug.Log("Radius: " + lookRadius);

        if (distance <= lookRadius )//&& isTurn==true)
        {
            agent.SetDestination(target.position);
            Debug.Log("target pos: " + target.position);
            Debug.Log("enemy pos: " + transform.position);



        if (transform.position.x == target.position.x && transform.position.z < target.position.z)
            { //Player is above
                animator.SetInteger("intDirection", 4);
                dirFacing = enemyDir.Up;
                agent.Move(transform.position + Vector3.forward);
                //isTurn = false;
            }
            else if (transform.position.x < target.position.x && transform.position.z == target.position.z)
            { //Player is to the right
                animator.SetInteger("intDirection", 2);
                dirFacing = enemyDir.Right;
                agent.Move(transform.position + Vector3.right);
                //isTurn = false;
            }
            else if (transform.position.x == target.position.x && transform.position.z > target.position.z)
            { //Player is below
                animator.SetInteger("intDirection", 2);
                dirFacing = enemyDir.Down;
                agent.Move(transform.position + Vector3.back);
               // isTurn = false;
            }
            else if (transform.position.x > target.position.x && transform.position.z == target.position.z)
            { //Player is to the left
                animator.SetInteger("intDirection", 4);
                dirFacing = enemyDir.Left;
                agent.Move(transform.position + Vector3.left);
               // isTurn = false;
            }


        }

        if (distance <= agent.stoppingDistance)
            {
                //do somethin;
            }
        }
    }
