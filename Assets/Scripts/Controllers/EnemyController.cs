using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //[SerializeField] private float speed = 10f;
    //enemy is 1 unit thick (same as player), so a raylength from the middle will stick out 0.9 units.
    //private float rayLength = 1.4f;

    //private bool moving;
    private enemyDir dirFacing = enemyDir.Up;
    public float lookRadius = 10f;
    Transform target;
    NavMeshAgent agent;


    public Animator animator;

    enum enemyDir
    {
        Up,
        Down,
        Left,
        Right,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
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

        if(distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            Debug.Log("target pos: " + target.position);
            Debug.Log("enemy pos: " + transform.position);

            if (transform.position.x > target.position.x && transform.position.z > target.position.z){//Enemy going left and down to player
                animator.SetInteger("intDirection", 2);
                dirFacing = enemyDir.DownLeft;
            }else if (transform.position.x < target.position.x && transform.position.z < target.position.z){//Enemy going right and up to player
                animator.SetInteger("intDirection", 4);
                dirFacing = enemyDir.UpRight;
            }else if (transform.position.x < target.position.x && transform.position.z == target.position.z) { //Enemy going right
                animator.SetInteger("intDirection", 2);
                dirFacing = enemyDir.Right;
            }else if (transform.position.x > target.position.x && transform.position.z == target.position.z){ //Enemy going left
                animator.SetInteger("intDirection", 4);
                dirFacing = enemyDir.Left;
            }else if (transform.position.x == target.position.x && transform.position.z < target.position.z){ //Enemy going up
                animator.SetInteger("intDirection", 4);
                dirFacing = enemyDir.Up;
            }else if (transform.position.x == target.position.x && transform.position.z > target.position.z){ //Enemy going down
                animator.SetInteger("intDirection", 2);
                dirFacing = enemyDir.Down;
            }else if (transform.position.x < target.position.x && transform.position.z < target.position.z){//Enemy going right and down to player
                animator.SetInteger("intDirection", 2);
                dirFacing = enemyDir.DownRight;
            }


            if (distance <= agent.stoppingDistance)
            {
                //do somethin;
            }
        }
    }
    
}
