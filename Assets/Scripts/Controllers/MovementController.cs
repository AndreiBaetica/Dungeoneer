using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    //player is 1 unit thick, so a raylength from the middle will stick out 0.9 units.
    private float rayLength = 1.4f;
    private float rayOffsetX = 0.4f;
    private float rayOffsetY = 0.4f;
    private float rayOffsetZ = 0.4f;
    public bool moving;
    private playerDir dirFacing = playerDir.Up;
    
    enum playerDir
    {
        Up,
        Down,
        Left,
        Right
    }

    private Vector3 targetPosition;
    private Vector3 startPosition;


    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    //raycasts the corners of the character cube to check for collision
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

    //snaps character to the precise final position at the end of a movement.
    public void snapToGridSquare()
    {
        if (Vector3.Distance(startPosition, transform.position) > 1f)
        {
            transform.position = targetPosition;
            moving = false;
            return;
        }

        transform.position += (targetPosition - startPosition) * speed * Time.deltaTime;
        return;
    }

    public void moveUp()
    {
        
        if (dirFacing != playerDir.Up)
        {
            //look up
            dirFacing = playerDir.Up;
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

    public void moveDown()
    {
        if (dirFacing != playerDir.Down)
        {
            //look down
            dirFacing = playerDir.Down;
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

    public void moveLeft()
    {
        if (dirFacing != playerDir.Left)
        {
            //look left
            dirFacing = playerDir.Left;
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

    public void moveRight()
    {
        if (dirFacing != playerDir.Right)
        {
            //look right
            dirFacing = playerDir.Right;
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
