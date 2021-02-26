using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    //enemy is 1 unit thick (same as player), so a raylength from the middle will stick out 0.9 units.
    private float rayLength = 1.4f;
    private float rayOffsetX = 0.4f;
    private float rayOffsetY = 0.4f;
    private float rayOffsetZ = 0.4f;
    private bool moving;
    private enemyDir dirFacing = enemyDir.Down;
    public float lookRadius = 10f;
    Transform target;
    NavMeshAgent agent;


    public Animator animator;

    enum enemyDir
    {
        Up,
        Down,
        Left,
        Right
    }

    private Vector3 enemyTargetPosition;
    private Vector3 enemyStartPosition;
    //private PlayerMovement Player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //Player = GetComponent<PlayerMovement>();
        Debug.Log("Start: " + animator);


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Target: "+Player.playerTargetPosition);
        if (moving)
        {
            if (Vector3.Distance(enemyStartPosition, transform.position) > 1f)
            {
                transform.position = enemyTargetPosition;
                moving = false;
                return;
            }

            transform.position += (enemyTargetPosition - enemyStartPosition) * speed * Time.deltaTime;
            return;
        }//should the rest not go under an else here?

        //up
        if (Input.GetKeyDown("w"))
        {

            if (dirFacing != enemyDir.Up)
            {
                //look up
                dirFacing = enemyDir.Up;
                animator.SetInteger("intDirection", 4);
            }
            else
            {
                if (canMove(Vector3.forward))
                {
                    enemyTargetPosition = transform.position + Vector3.forward;
                    enemyStartPosition = transform.position;
                    moving = true;
                }
            }
        }

        //left
        if (Input.GetKeyDown("a"))
        {
            if (dirFacing != enemyDir.Left)
            {
                //look left
                dirFacing = enemyDir.Left;
                animator.SetInteger("intDirection", 2);
            }
            else
            {
                if (canMove(Vector3.left))
                {
                    enemyTargetPosition = transform.position + Vector3.left;
                    enemyStartPosition = transform.position;
                    moving = true;
                }
            }
        }

        //down
        if (Input.GetKeyDown("s"))
        {
            if (dirFacing != enemyDir.Down)
            {
                //look down
                dirFacing = enemyDir.Down;
                animator.SetInteger("intDirection", 2);
            }
            else
            {
                if (canMove(Vector3.back))
                {
                    enemyTargetPosition = transform.position + Vector3.back;
                    enemyStartPosition = transform.position;
                    moving = true;
                }
            }
        }

        //right
        if (Input.GetKeyDown("d"))
        {

            if (dirFacing != enemyDir.Right)
            {
                //look right
                dirFacing = enemyDir.Right;
                animator.SetInteger("intDirection", 4);
            }
            else
            {
                if (canMove(Vector3.right))
                {
                    enemyTargetPosition = transform.position + Vector3.right;
                    enemyStartPosition = transform.position;
                    moving = true;
                }
            }
        }
    }

    //raycasts the corners of the enemy cube
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
}
