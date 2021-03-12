using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
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
    //public bool isTurn;
    
    enum playerDir
    {
        Up,
        Down,
        Left,
        Right
    }

    private Vector3 playerTargetPosition;
    private Vector3 playerStartPosition;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       // if (isTurn){
            if (moving)
            {
                move();
            }//should the rest not go under an else here?

            //up
            if (Input.GetKeyDown("w"))
            {

                if (dirFacing != playerDir.Up)
                {
                    //look up
                    dirFacing = playerDir.Up;
                    animator.SetInteger("intDirection", 4);
                }
                else
                {
                    moveUp();
                    //isTurn = false;

                }
            }

            //left
            if (Input.GetKeyDown("a"))
            {
                if (dirFacing != playerDir.Left)
                {
                    //look left
                    dirFacing = playerDir.Left;
                    animator.SetInteger("intDirection", 2);
                }
                else
                {
                    moveLeft();
                    //isTurn = false;


                }
            }

            //down
            if (Input.GetKeyDown("s"))
            {
                if (dirFacing != playerDir.Down)
                {
                    //look down
                    dirFacing = playerDir.Down;
                    animator.SetInteger("intDirection", 2);
                }
                else
                {
                    moveDown();
                    //isTurn = false;

                }
            }

            //right
            if (Input.GetKeyDown("d"))
            {

                if (dirFacing != playerDir.Right)
                {
                    //look right
                    dirFacing = playerDir.Right;
                    animator.SetInteger("intDirection", 4);
                }
                else
                {
                    moveRight();
                    //isTurn = false;

                }
            }
       // }
        
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


    private void move()
    {
        if (Vector3.Distance(playerStartPosition, transform.position) > 1f)
        {
            transform.position = playerTargetPosition;
            moving = false;
            return;
        }

        transform.position += (playerTargetPosition - playerStartPosition) * speed * Time.deltaTime;
        return;
    }

    private void moveUp()
    {
        if (canMove(Vector3.forward))
        {
            playerTargetPosition = transform.position + Vector3.forward;
            playerStartPosition = transform.position;
            moving = true;
        }
    }

    private void moveDown()
    {
        if (canMove(Vector3.back))
        {
            playerTargetPosition = transform.position + Vector3.back;
            playerStartPosition = transform.position;
            moving = true;
        }
    }

    private void moveLeft()
    {
        if (canMove(Vector3.left))
        {
            playerTargetPosition = transform.position + Vector3.left;
            playerStartPosition = transform.position;
            moving = true;
        }
    }

    private void moveRight()
    {
        if (canMove(Vector3.right))
        {
            playerTargetPosition = transform.position + Vector3.right;
            playerStartPosition = transform.position;
            moving = true;
        }
    }
}