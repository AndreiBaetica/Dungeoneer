using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private enemyDir dirFacing = enemyDir.Up;
    private float lookRadius = 4.5f;
    Transform target;
    NavMeshAgent agent;
    private bool moving;
    private float rayLength = 1.4f;
    private float rayOffsetX = 0.4f;
    private float rayOffsetY = 0.4f;
    private float rayOffsetZ = 0.4f;


    public Animator animator;
    //public bool isTurn;

    enum enemyDir
    {
        Up,
        Down,
        Left,
        Right
    }


    private Vector3 enemyTargetPosition;
    private Vector3 enemyStartPosition;

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
            //agent.SetDestination(target.position);
            Debug.Log("target pos: " + target.position);
            Debug.Log("enemy pos: " + transform.position);


            if (moving)
            {
                move();
            }
            if (transform.position.x == target.position.x && transform.position.z < target.position.z)
            { //Player is above
                animator.SetInteger("intDirection", 4);
                dirFacing = enemyDir.Up;
                moveUp();
                agent.Move(enemyTargetPosition);
                //isTurn = false;
            }
            else if (transform.position.x < target.position.x && transform.position.z == target.position.z)
            { //Player is to the right
                animator.SetInteger("intDirection", 2);
                dirFacing = enemyDir.Right;
                moveRight();
                agent.Move(enemyTargetPosition);
                //isTurn = false;
            }
            else if (transform.position.x == target.position.x && transform.position.z > target.position.z)
            { //Player is below
                animator.SetInteger("intDirection", 2);
                dirFacing = enemyDir.Down;
                moveDown();
                agent.Move(enemyTargetPosition);
               // isTurn = false;
            }
            else if (transform.position.x > target.position.x && transform.position.z == target.position.z)
            { //Player is to the left
                animator.SetInteger("intDirection", 4);
                dirFacing = enemyDir.Left;
                moveLeft();
                agent.Move(enemyTargetPosition);
               // isTurn = false;
            }


        }

        if (distance <= agent.stoppingDistance)
            {
                //do somethin;
            }
        }

    private bool canMove(Vector3 direction)
    {
        if (Vector3.Equals(Vector3.forward, direction) || Vector3.Equals(Vector3.back, direction))
        {
            if (Physics.Raycast(transform.position + Vector3.up * rayOffsetY + Vector3.right * rayOffsetX, direction,
                rayLength)) return false;
            if (Physics.Raycast(transform.position + Vector3.up * rayOffsetY - Vector3.right * rayOffsetX, direction,
                rayLength)) return false;
        }
        else if (Vector3.Equals(Vector3.left, direction) || Vector3.Equals(Vector3.right, direction))
        {
            if (Physics.Raycast(transform.position + Vector3.up * rayOffsetY + Vector3.forward * rayOffsetZ, direction,
                rayLength)) return false;
            if (Physics.Raycast(transform.position + Vector3.up * rayOffsetY - Vector3.forward * rayOffsetZ, direction,
                rayLength)) return false;
        }
        return true;
    }


    private void move()
    {
        if (Vector3.Distance(enemyStartPosition, transform.position) > 1f)
        {
            transform.position = enemyTargetPosition;
            moving = false;
            return;
        }

        transform.position += (enemyTargetPosition - enemyStartPosition) * speed * Time.deltaTime;
        return;
    }

    private void moveUp()
    {
        if (canMove(Vector3.forward))
        {
            enemyTargetPosition = transform.position + Vector3.forward;
            enemyStartPosition = transform.position;
            moving = true;
        }
    }

    private void moveDown()
    {
        if (canMove(Vector3.back))
        {
            enemyTargetPosition = transform.position + Vector3.back;
            enemyStartPosition = transform.position;
            moving = true;
        }
    }

    private void moveLeft()
    {
        if (canMove(Vector3.left))
        {
            enemyTargetPosition = transform.position + Vector3.left;
            enemyStartPosition = transform.position;
            moving = true;
        }
    }

    private void moveRight()
    {
        if (canMove(Vector3.right))
        {
            enemyTargetPosition = transform.position + Vector3.right;
            enemyStartPosition = transform.position;
            moving = true;
        }
    }

}
