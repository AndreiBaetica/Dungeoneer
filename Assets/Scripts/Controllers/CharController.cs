using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

//Name shortened to not conflict with Unity's default CharacterController
public class CharController : MonoBehaviour
{
    protected new String name = "Character";
    public bool doneTurn;
    private const float Speed = 10f;
    protected int maxHealth = 100;
    protected int currentHealth;
    [SerializeField] private GameObject damageIndicator;
    
    //player is 1 unit thick, so a raylength from the middle will stick out 0.9 units.
    private float rayLength = 1.4f;
    private float rayOffsetX = 0.4f;
    private float rayOffsetY = 0.4f;
    private float rayOffsetZ = 0.4f;
    protected bool moving;
    protected CharacterDir _dirFacing = CharacterDir.Back;

    protected enum CharacterDir
    {
        Forward,
        Back,
        Left,
        Right
    }

    private Vector3 _targetPosition;
    private Vector3 _startPosition;
    
    //can be used to hit multiple grid squares
    private Vector3 meleeAttackShape = new Vector3(0.4f, 0.4f, 0.4f);
    private Vector3 meleeAttackMultiplier = new Vector3(1f, 1f, 1f);

    private int base_melee_damage = 1;
    private int melee_damage_multiplier = 1;

    protected Animator animator;
    
    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    protected void Update()
    {

    }
    
    public virtual bool Action()
    {
        return false;
    }
    
    //raycasts the corners of the character cube to check for collision
    protected bool CanMove(Vector3 direction)
    {
        LayerMask everyLayerButInteractableLayerMask =~ LayerMask.GetMask("Interactable");
        if (Vector3.Equals(Vector3.forward, direction) || Vector3.Equals(Vector3.back, direction))
        {
            if (Physics.Raycast(transform.position + Vector3.up * rayOffsetY + Vector3.right * rayOffsetX, direction,
                rayLength, everyLayerButInteractableLayerMask)) return false;
            if (Physics.Raycast(transform.position + Vector3.up * rayOffsetY - Vector3.right * rayOffsetX, direction,
                rayLength, everyLayerButInteractableLayerMask)) return false;
        }
        else if (Vector3.Equals(Vector3.left, direction) || Vector3.Equals(Vector3.right, direction))
        {
            if (Physics.Raycast(transform.position + Vector3.up * rayOffsetY + Vector3.forward * rayOffsetZ, direction,
                rayLength, everyLayerButInteractableLayerMask)) return false;
            if (Physics.Raycast(transform.position + Vector3.up * rayOffsetY - Vector3.forward * rayOffsetZ, direction,
                rayLength, everyLayerButInteractableLayerMask)) return false;
        }
        return true;
    }

    //snaps character to the precise final position at the end of a movement.
    protected void SnapToGridSquare()
    {
        if (Vector3.Distance(_startPosition, transform.position) > 1f)
        {
            transform.position = _targetPosition;
            moving = false;
            return;
        }

        transform.position += (_targetPosition - _startPosition) * Speed * Time.deltaTime;
    }


    protected bool Move(Vector3 direction)
    {
        bool isFree = true;

        bool isFacingDirection = false;
        
        if (direction.Equals(Vector3.forward) && _dirFacing == CharacterDir.Forward) isFacingDirection = true;
        else if (direction.Equals(Vector3.right) && _dirFacing == CharacterDir.Right) isFacingDirection = true;
        else if (direction.Equals(Vector3.back) && _dirFacing == CharacterDir.Back) isFacingDirection = true;
        else if (direction.Equals(Vector3.left) && _dirFacing == CharacterDir.Left) isFacingDirection = true;

        if (isFacingDirection)
        {
            _targetPosition = transform.position + direction;
            if (CanMove(direction) && !GameLoopManager.reservedPositions.Contains(_targetPosition))
            {
                GameLoopManager.reservedPositions.Add(_targetPosition);
                _startPosition = transform.position;
                moving = true;
                isFree = false;
            }
        }
        else
        {
            Rotate(direction);
        }

        return isFree;
    }

    protected void Rotate(Vector3 direction)
    {
        //fwd
        if (direction.Equals(Vector3.forward))
        {
            //look up
            _dirFacing = CharacterDir.Forward;
            animator.SetInteger("intDirection", 0);
            
        }
        //back
        else if (direction.Equals(Vector3.back))
        {
            //look down
            _dirFacing = CharacterDir.Back;
            animator.SetInteger("intDirection", 2);
            
        }
        //left
        else if (direction.Equals(Vector3.left))
        {
            //look left
            _dirFacing = CharacterDir.Left;
            animator.SetInteger("intDirection", 3);
            
        }
        //right
        else if (direction.Equals(Vector3.right))
        {
            //look right
            _dirFacing = CharacterDir.Right;
            animator.SetInteger("intDirection", 1);
            
        }
    }


    protected bool MeleeAttack(LayerMask mask)
    {
        bool attackUnsuccessful = true;

        if (!moving)
        {
            Vector3 actualAttackShape = Vector3.Scale(meleeAttackShape, meleeAttackMultiplier);
            int actualMeleeDamage = base_melee_damage * melee_damage_multiplier;

            //TODO
            //play animation

            //detect enemy
            Vector3 attackCenter;
            switch (_dirFacing)
            {
                case CharacterDir.Forward:
                    attackCenter = transform.position + Vector3.forward;
                    break;
                case CharacterDir.Back:
                    attackCenter = transform.position + Vector3.back;
                    break;
                case CharacterDir.Left:
                    attackCenter = transform.position + Vector3.left;
                    break;
                case CharacterDir.Right:
                    attackCenter = transform.position + Vector3.right;
                    break;
                default:
                    throw new InvalidOperationException("Character has no facing.");
            }

            Collider[] targetsHit = Physics.OverlapBox(attackCenter, actualAttackShape, Quaternion.identity, mask);

            //deal damage
            foreach (Collider target in targetsHit)
            {
                //target.GetComponent<CharController>().TakeDamage(actualMeleeDamage);
                //target.GetComponent<Rigidbody>().detectCollisions = false;
                target.GetComponentInParent<CharController>().TakeDamage(actualMeleeDamage);
            }
            animator.SetTrigger("attack");
            attackUnsuccessful = false;
        }

        return attackUnsuccessful;
    }

    private void TakeDamage(int damage)
    {
        //TODO: play hurt animation
        currentHealth -= damage;
        DamageIndicator.Create(transform.position, damage, damageIndicator);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        //TODO: play death animation
        animator.SetBool("isDead", true);
        enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = false;
        doneTurn = true;

        Debug.Log(name + " has died.");
        
    }
    
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public int CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }
}
