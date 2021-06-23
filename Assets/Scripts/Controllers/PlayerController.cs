using System.ComponentModel.Design;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharController
{
    #region Singleton

    public static PlayerController instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    
    public HealthBar healthBar;
    public ManaBar manaBar;
    public GameManager GameManager;
    
    private LayerMask NPCMask;
    private int maxMana = 10;
    private int currentMana;
    private IInteractable interactable;
    
    protected new void Start()
    {
        NPCMask= LayerMask.GetMask("NPC");
        name = "Player";
        maxHealth = 15;
        base.Start();
        healthBar.SetMaxHealth(maxHealth);
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }

    protected new void Update()
    {
        healthBar.SetHealth(5);
        manaBar.SetMana(currentMana);

        if (moving) SnapToGridSquare();
        if (GameLoopManager.GetPlayerTurn() && !doneTurn)
        {
            doneTurn = !Action();
            if (doneTurn)
            {
                if (healthBar.health > 0)
                {
                    GameLoopManager.SetPlayerTurn(false);
                }
                else
                {
                    PlayerDeath(10);
                }
            }
        }
    }
    
    public override bool Action()
    {
        bool isFree = true;
        //movement
        
        if (Input.GetKeyDown("w")) isFree = Move(Vector3.forward);
        if (Input.GetKeyDown("a")) isFree = Move(Vector3.left);
        if (Input.GetKeyDown("s")) isFree = Move(Vector3.back);
        if (Input.GetKeyDown("d")) isFree = Move(Vector3.right);
            
        //attack
        if (Input.GetKeyDown("q"))
        {
            isFree = MeleeAttack(NPCMask);
        }

        return isFree;
    }
    
    public void Heal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }
    
    public void Interact()
    {
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Interactable")
        {
            Debug.Log("Player has entered the interaction zone of an interactable object."); // TODO : Remove
            interactable = collision.GetComponent<IInteractable>();
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Interactable")
        {
            if (interactable != null)
            {
                Debug.Log("Player has left the interaction zone of an interactable object."); // TODO : Remove
                interactable.StopInteract();
                interactable = null;
            }
        }
    }


    public void PlayerDeath(int maxHealth)
    {
        Debug.Log("PLAYER HAS DIED");
        GameManager.Setup(maxHealth);
    }


    public int MaxMana
    {
        get => maxMana;
        set => maxMana = value;
    }

    public int CurrentMana
    {
        get => currentMana;
        set => currentMana = value;
    }
}