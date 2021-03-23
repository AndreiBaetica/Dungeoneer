using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CharacterController : MonoBehaviour
{
    public String name = "Character";
    
    private float speed = 10f;
    public int maxHealth = 100;
    private int currentHealth;
    
    //player is 1 unit thick, so a raylength from the middle will stick out 0.9 units.
    private float rayLength = 1.4f;
    private float rayOffsetX = 0.4f;
    private float rayOffsetY = 0.4f;
    private float rayOffsetZ = 0.4f;
    public bool moving;
    private CharacterDir dirFacing = CharacterDir.Up;
    
    enum CharacterDir
    {
        Up,
        Down,
        Left,
        Right
    }

    private Vector3 targetPosition;
    private Vector3 startPosition;
    
    //can be used to hit multiple grid squares
    private Vector3 meleeAttackShape = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 meleeAttackMultiplier = new Vector3(1f, 1f, 1f);

    private int base_melee_damage = 1;
    private int melee_damage_multiplier = 1;
    
    public Animator animator;
    
    
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
    
    //raycasts the corners of the character cube to check for collision
    public bool canMove(Vector3 direction)
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

    public void MoveForward()
    {
        
        if (dirFacing != CharacterDir.Up)
        {
            //look up
            dirFacing = CharacterDir.Up;
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

    public void MoveBack()
    {
        if (dirFacing != CharacterDir.Down)
        {
            //look down
            dirFacing = CharacterDir.Down;
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

    public void MoveLeft()
    {
        if (dirFacing != CharacterDir.Left)
        {
            //look left
            dirFacing = CharacterDir.Left;
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

    public void MoveRight()
    {
        if (dirFacing != CharacterDir.Right)
        {
            //look right
            dirFacing = CharacterDir.Right;
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


    public void MeleeAttack()
    {
        Vector3 actualAttackShape = Vector3.Scale(meleeAttackShape, meleeAttackMultiplier);
        int actualMeleeDamage = base_melee_damage * melee_damage_multiplier;

        //TODO
        //play animation
        
        //detect enemy
        Vector3 attackCenter;
        switch (dirFacing)
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
        //TODO: CHARACTER LAYER
        Collider[] targetsHit = Physics.OverlapBox(attackCenter, actualAttackShape);
        
        //deal damage
        foreach (Collider target in targetsHit)
        {
            target.GetComponent<CharacterController>().TakeDamage(actualMeleeDamage);
        }
    }

    private void TakeDamage(int damage)
    {
        //TODO: play hurt animation
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //TODO: play death animation

        this.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = false;
        
        Debug.Log(name + " has died.");
        
    }
}
