using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Animator animator;

    enum enemyDir
    {
        Up,
        Down,
        Left,
        Right
    }

    private Vector3 targetPosition;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (Vector3.Distance(startPosition, transform.position) > 1f)
            {
                transform.position = targetPosition;
                moving = false;
                return;
            }

            transform.position += (targetPosition - startPosition) * speed * Time.deltaTime;
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
                    targetPosition = transform.position + Vector3.forward;
                    startPosition = transform.position;
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
                    targetPosition = transform.position + Vector3.left;
                    startPosition = transform.position;
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
                    targetPosition = transform.position + Vector3.back;
                    startPosition = transform.position;
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
                    targetPosition = transform.position + Vector3.right;
                    startPosition = transform.position;
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
