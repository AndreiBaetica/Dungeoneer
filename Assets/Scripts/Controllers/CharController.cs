﻿using System;
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
    private CharacterDir _dirFacing = CharacterDir.Up;

    private enum CharacterDir
    {
        Up,
        Down,
        Left,
        Right
    }

    private Vector3 _targetPosition;
    private Vector3 _startPosition;
    
    //can be used to hit multiple grid squares
    private Vector3 meleeAttackShape = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 meleeAttackMultiplier = new Vector3(1f, 1f, 1f);

    private int base_melee_damage = 1;
    private int melee_damage_multiplier = 1;

    protected Animator animator;
    
    
    
    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponent<Animator>();
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

    //better to use an emum for direction
    protected bool Move(Vector3 direction)
    {
        //done flag allows for rotation without wasting a turn
        bool isFree = true;
        
            //fwd
            if (direction.Equals(Vector3.forward))
            {
                if (_dirFacing != CharacterDir.Up)
                {
                    //look up
                    _dirFacing = CharacterDir.Up;
                    animator.SetInteger("intDirection", 4);
                }
                else
                {
                    if (CanMove(Vector3.forward))
                    {
                        _targetPosition = transform.position + Vector3.forward;
                        _startPosition = transform.position;
                        moving = true;
                        isFree = false;
                    }
                }
            }
            //back
            else if (direction.Equals(Vector3.back))
            {
                if (_dirFacing != CharacterDir.Down)
                {
                    //look down
                    _dirFacing = CharacterDir.Down;
                    animator.SetInteger("intDirection", 2);
                }
                else
                {
                    if (CanMove(Vector3.back))
                    {
                        _targetPosition = transform.position + Vector3.back;
                        _startPosition = transform.position;
                        moving = true;
                        isFree = false;
                    }
                }
            }
            //left
            else if (direction.Equals(Vector3.left))
            {
                if (_dirFacing != CharacterDir.Left)
                {
                    //look left
                    _dirFacing = CharacterDir.Left;
                    animator.SetInteger("intDirection", 2);
                }
                else
                {
                    if (CanMove(Vector3.left))
                    {
                        _targetPosition = transform.position + Vector3.left;
                        _startPosition = transform.position;
                        moving = true;
                        isFree = false;
                    }
                }
            }
            //right
            else if (direction.Equals(Vector3.right))
            {
                if (_dirFacing != CharacterDir.Right)
                {
                    //look right
                    _dirFacing = CharacterDir.Right;
                    animator.SetInteger("intDirection", 4);
                }
                else
                {
                    if (CanMove(Vector3.right))
                    {
                        _targetPosition = transform.position + Vector3.right;
                        _startPosition = transform.position;
                        moving = true;
                        isFree = false;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid move direction.");
            }

            return isFree;
    }
    protected void MoveForward()
    {
        
        if (_dirFacing != CharacterDir.Up)
        {
            //look up
            _dirFacing = CharacterDir.Up;
            animator.SetInteger("intDirection", 4);
        }
        else
        {
            if (CanMove(Vector3.forward))
            {
                _targetPosition = transform.position + Vector3.forward;
                _startPosition = transform.position;
                moving = true;
            }
        }
    }

    protected void MoveBack()
    {
        if (_dirFacing != CharacterDir.Down)
        {
            //look down
            _dirFacing = CharacterDir.Down;
            animator.SetInteger("intDirection", 2);
        }
        else
        {
            if (CanMove(Vector3.back))
            {
                _targetPosition = transform.position + Vector3.back;
                _startPosition = transform.position;
                moving = true;
            }
        }
    }

    protected void MoveLeft()
    {
        if (_dirFacing != CharacterDir.Left)
        {
            //look left
            _dirFacing = CharacterDir.Left;
            animator.SetInteger("intDirection", 2);
        }
        else
        {
            if (CanMove(Vector3.left))
            {
                _targetPosition = transform.position + Vector3.left;
                _startPosition = transform.position;
                moving = true;
            }
        }
    }

    protected void MoveRight()
    {
        if (_dirFacing != CharacterDir.Right)
        {
            //look right
            _dirFacing = CharacterDir.Right;
            animator.SetInteger("intDirection", 4);
        }
        else
        {
            if (CanMove(Vector3.right))
            {
                _targetPosition = transform.position + Vector3.right;
                _startPosition = transform.position;
                moving = true;
            }
        }
    }

    protected void MeleeAttack(LayerMask mask)
    {
        Vector3 actualAttackShape = Vector3.Scale(meleeAttackShape, meleeAttackMultiplier);
        int actualMeleeDamage = base_melee_damage * melee_damage_multiplier;

        //TODO
        //play animation
        
        //detect enemy
        Vector3 attackCenter;
        switch (_dirFacing)
        {
            case CharacterDir.Up:
                attackCenter = transform.position + Vector3.forward;
                break;
            case CharacterDir.Down:
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

    private void Die()
    {
        //TODO: play death animation

        enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = false;
        doneTurn = true;
        
        
        Debug.Log(name + " has died.");
        
    }
}
