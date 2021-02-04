using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    //player is 1 unit thick, so a raylength from the middle will stick out 0.9 units.
    private float rayLength = 1.4f;
    private float rayOffsetX = 0.4f;
    private float rayOffsetY = 0.4f;
    private float rayOffsetZ = 0.4f;
    private bool moving;
    private playerDir dirFacing = playerDir.Up;

    public Animator animator;
    
    enum playerDir
    {
        Up,
        Down,
        Left,
        Right
    }

    private Vector3 targetPosition;
    private Vector3 startPosition;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

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

            if (dirFacing != playerDir.Up)
            {
                //look up
                dirFacing = playerDir.Up;
                animator.SetInteger("intDirection",4);
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
            if (dirFacing != playerDir.Left)
            {
                //look left
                dirFacing = playerDir.Left;
                animator.SetInteger("intDirection",2);
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
            if (dirFacing != playerDir.Down)
            {
                //look down
                dirFacing = playerDir.Down;
                animator.SetInteger("intDirection",2);
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
            
            if (dirFacing != playerDir.Right)
            {
                //look right
                dirFacing = playerDir.Right;
                animator.SetInteger("intDirection",4);
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
//raycasts the corners of the player cube
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