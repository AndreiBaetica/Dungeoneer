using UnityEngine;

public class EnemyController : CharController
{
    private float lookRadius = 4.5f;
    private Transform target;
    private LayerMask PlayerMask;
    private BehaviourState currentBehaviourState = BehaviourState.Idle;

    private enum BehaviourState
    {
        Idle,
        Chase,
        Attack
    }

    // Start is called before the first frame update
    protected new void Start()
    {
        currentHealth = maxHealth;
        PlayerMask = LayerMask.GetMask("Player");
        base.Start();
        //target.position player Position; transform.position for enemy position
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    protected new void Update()
    {
        if (!GameLoopManager.GetPlayerTurn() && !doneTurn)
        {
            doneTurn = !Action();
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public override bool Action()
    {
        bool isFree = true;
        if (!moving) StateSwitch();
        if (currentBehaviourState == BehaviourState.Idle)
        {
            //Idle behaviour
            isFree = false;
        } else if (currentBehaviourState == BehaviourState.Chase)
        {
            //Chase behaviour

            if (moving) SnapToGridSquare();
            else
            {
                string direction = FindBestMovement();
                if (direction.Equals("fwd")) isFree = Move(Vector3.forward);
                else if (direction.Equals("back")) isFree = Move(Vector3.back);
                else if (direction.Equals("left")) isFree = Move(Vector3.left);
                else if (direction.Equals("right")) isFree = Move(Vector3.right);
            }
            
        } else if (currentBehaviourState == BehaviourState.Attack)
        {
            //attack behaviour
            if (!moving)
            {
                //rotate to face player, if not already facing
                if ((target.position - transform.position) == Vector3.left) Rotate(Vector3.left);
                else if ((target.position - transform.position) == Vector3.right) Rotate(Vector3.right);
                else if ((target.position - transform.position) == Vector3.forward) Rotate(Vector3.forward);
                else if ((target.position - transform.position) == Vector3.back) Rotate(Vector3.back);
                
                isFree = MeleeAttack(PlayerMask);
            }
        }

        return isFree;
    }

    private void StateSwitch()
    {
        if (currentBehaviourState == BehaviourState.Idle)
        {
            if (Vector3.Distance(target.position, transform.position) <= lookRadius)
            {
                currentBehaviourState = BehaviourState.Chase;
            }
        } else if (currentBehaviourState == BehaviourState.Chase)
        {
            if (Vector3.Distance(target.position, transform.position) > lookRadius && !moving)
            {
                currentBehaviourState = BehaviourState.Idle;
            } else if (Vector3.Distance(target.position, transform.position) <= 1f && !moving)
            {
                currentBehaviourState = BehaviourState.Attack;
            }
        } else if (currentBehaviourState == BehaviourState.Attack)
        {
            if (Vector3.Distance(target.position, transform.position) > 1f)
            {
                currentBehaviourState = BehaviourState.Chase;
            }
        }
    }
    
    //gives the direction that the enemy should move in to get closer to the player
    private string FindBestMovement()
    {
        //none should never happen
        string bestMovement = "none";
        float distanceToPlayer;
        float shortestDistanceToPlayer = 5f;
        
        //up
        if (CanMove(Vector3.forward) && !GameLoopManager.reservedPositions.Contains(transform.position + Vector3.forward))
        {
            distanceToPlayer = Vector3.Distance((transform.position + Vector3.forward), target.position);
            if (distanceToPlayer < shortestDistanceToPlayer)
            {
                shortestDistanceToPlayer = distanceToPlayer;
                bestMovement = "fwd";
            }
        }
        //down
        if (CanMove(Vector3.back) && !GameLoopManager.reservedPositions.Contains(transform.position + Vector3.back))
        {
            distanceToPlayer = Vector3.Distance((transform.position + Vector3.back), target.position);
            if (distanceToPlayer < shortestDistanceToPlayer)
            {
                shortestDistanceToPlayer = distanceToPlayer;
                bestMovement = "back";
            }
        }
        //left
        if (CanMove(Vector3.left) && !GameLoopManager.reservedPositions.Contains(transform.position + Vector3.left))
        {
            
            distanceToPlayer = Vector3.Distance((transform.position + Vector3.left), target.position);
            if (distanceToPlayer < shortestDistanceToPlayer)
            {
                shortestDistanceToPlayer = distanceToPlayer;
                bestMovement = "left";
            }
        }
        //right
        if (CanMove(Vector3.right) && !GameLoopManager.reservedPositions.Contains(transform.position + Vector3.right))
        {
            distanceToPlayer = Vector3.Distance((transform.position + Vector3.right), target.position);
            if (distanceToPlayer < shortestDistanceToPlayer)
            {
                bestMovement = "right";
            }
        }

        return bestMovement;
    }
}
