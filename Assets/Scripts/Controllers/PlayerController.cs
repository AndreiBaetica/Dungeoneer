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
        healthBar.SetHealth(currentHealth);
        manaBar.SetMana(currentMana);
        //movement
        if (moving) SnapToGridSquare();
        if (Input.GetKeyDown("w")) MoveForward();
        if (Input.GetKeyDown("a")) MoveLeft();
        if (Input.GetKeyDown("s")) MoveBack();
        if (Input.GetKeyDown("d")) MoveRight();
        
        //attack
        if (Input.GetKeyDown("q")) MeleeAttack(NPCMask);
    }
}