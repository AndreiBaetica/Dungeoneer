using UnityEngine;
using System;

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
    protected int maxHealth = 100; 
    public int currentHealth;
    public int currentLevel;
    private LayerMask NPCMask;
    private int maxMana = 10;
    public int currentMana;
    private IInteractable interactable;
    [SerializeField] private GameObject damageIndicator;
    public bool saved;


    protected new void Start()
    {

        //try
        // {
        saved = GameStartManager.PlayingSavedGame;
        Debug.Log("SAVED IS "+saved);

        if (saved)
        { 
            Debug.Log("SAVED IS TRUE!!!!!!!@@@@@@@@@");

            PlayerSaveData data = SaveSystem.LoadPlayer();
            Debug.Log("SAVE FOUND!@@@@@@@@@@@@@@");

            Debug.Log("Player controller data pos x:"+data.position[0]+" y:"
                      +data.position[1]+" z:"+data.position[2]
                      +" currenthealth:"+data.health+" mana:"+data.mana);
            NPCMask = LayerMask.GetMask("NPC");
            name = "Player";
            maxHealth = 15;
            currentLevel = 0;
            base.Start();
            healthBar.SetMaxHealth(maxHealth);
            manaBar.SetMaxMana(maxMana);
            var position = PlayerSaveData.ApplyPlayerSavedData(this, data);
            transform.position = position;
            /*PlayerController.instance.currentLevel = data.level;
            currentLevel = data.level;
            PlayerController.instance.CurrentHealth = data.health;
            currentHealth = data.health;
            PlayerController.instance.CurrentMana = data.mana;
            currentMana = data.mana;
            float[] position = new float[3];
            var playerPos = transform.position;
            playerPos.x = data.position[0];
            playerPos.y = data.position[1];
            playerPos.z = data.position[2];*/

            //data.ApplyPlayerSavedData(this);
        }
        else
        {
            Debug.Log("SAVED IS FALSE!!!!!!!@@@@@@@@@");
            currentLevel = 0;

            NPCMask = LayerMask.GetMask("NPC");
            name = "Player";
            maxHealth = 15;
            currentLevel = 0;
            base.Start();
            healthBar.SetMaxHealth(maxHealth);
            currentMana = maxMana;
            currentHealth = maxHealth;
            manaBar.SetMaxMana(maxMana);
            Debug.Log("REGULAR PLAYER STATS");
        }

        //   }
        /* catch (Exception e)
         {
             Debug.Log("SAVE NOT FOUND!!!!!!!@@@@@@@@@EXCEPTION");
             System.Console.WriteLine(e);
             currentLevel = 0;
 
             Debug.Log("REGULAR PLAYER STATS");
             throw;
         }*/



       
        Debug.Log("CONTINUE AFTER EXCEPTION THROW?");

    }

    protected new void Update()
    {

        healthBar.SetHealth(currentHealth);
        manaBar.SetMana(currentMana);

        if (moving) SnapToGridSquare();
        if (GameLoopManager.GetPlayerTurn() && !doneTurn)
        {
            doneTurn = !Action();
            if (doneTurn)
            {
                GameLoopManager.SetPlayerTurn(false);
            }
        }
    }
    
    public override bool Action()
    {
        bool isFree = true;
        //movement
        if (UIManager.IsFrozen() == false)
        {
            if (Input.GetKeyDown("w")) isFree = Move(Vector3.forward);
            if (Input.GetKeyDown("a")) isFree = Move(Vector3.left);
            if (Input.GetKeyDown("s")) isFree = Move(Vector3.back);
            if (Input.GetKeyDown("d")) isFree = Move(Vector3.right);
            
            //attack
            if (Input.GetKeyDown("q"))
            {
                isFree = MeleeAttack(NPCMask);
            }
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
    
    protected override void Die()
    {
        Debug.Log("PLAYER HAS DIED");
        base.Die();
        UIManager.EndGame();
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
    
    public void TakeDamage(int damage)
    {
        //TODO: play hurt animation
        currentHealth -= damage;
        DamageIndicator.Create(transform.position, damage, damageIndicator);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public static void attackCardAction(int damage, int distance, int radius, string typeOfDamage, int[] damageOverTime, bool instantTravel)
    {
        // Add attack card action on actual game here (ROSS)
        Debug.Log("Used attack type card");
    }

    public static void healingCardAction(int heal)
    {
        // Add healing card action on actual game here (ROSS)
        Debug.Log("Used healing type card");
    }

    public static void shieldingCardAction(int shield)
    {
        // Add shielding card action on actual game here (ROSS)
        Debug.Log("Used shielding type card");
    }
}
