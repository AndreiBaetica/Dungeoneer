﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharController
{
    
    public HealthBar healthBar;
    public ManaBar manaBar;
    
    private LayerMask NPCMask;
    private int maxMana = 10;
    private int currentMana;
    protected new void Start()
    {
        NPCMask= LayerMask.GetMask("NPC");
        name = "Player";
        maxHealth = 100;
        base.Start();
        healthBar.SetMaxHealth(maxHealth);
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }

    protected new void Update()
    {

    }

    public override bool Action()
    {
        bool isFree = false;
        healthBar.SetHealth(currentHealth);
        manaBar.SetMana(currentMana);

        //movement
        if (moving) SnapToGridSquare();
            if (Input.GetKeyDown("w")) isFree = Move(Vector3.forward);
            if (Input.GetKeyDown("a")) isFree = Move(Vector3.left);
            if (Input.GetKeyDown("s")) isFree = Move(Vector3.back);
            if (Input.GetKeyDown("d")) isFree = Move(Vector3.right);
            
        //attack
        if (Input.GetKeyDown("q")) MeleeAttack(NPCMask);

        if (!Input.anyKey) isFree = true;
        return isFree;
    }
    

}