using UnityEditor;
using UnityEngine;

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
    public Gold gold;
    private int maxMana;
    [SerializeField]public int currentMana;
    [SerializeField]public int currentLevel;    
    [SerializeField] private GameObject goldIndicator;
    private LayerMask NPCMask;
    private IInteractable interactable;
    public bool saved;


    protected new void Start()
    {
        saved = GameStartManager.PlayingSavedGame;
        Debug.Log("SAVED IS "+saved);
        NPCMask = LayerMask.GetMask("NPC");
        name = "Player";
        MaxHealth = 20;
        MaxMana = 10;
        CurrentLevel = 0;
        base.Start();
        healthBar.SetMaxHealth(MaxHealth);
        manaBar.SetMaxMana(MaxMana);
        if (saved)
        {
            PlayerSaveData data = SaveSystem.LoadPlayer();

            Debug.Log("Player controller data pos x:"+data.position[0]+" y:"
                      +data.position[1]+" z:"+data.position[2]
                      +" currenthealth:"+data.health+" mana:"+data.mana);
            PlayerSaveData.ApplyPlayerSavedData(this, data);
        }
        else
        {
            CurrentMana = MaxMana;
            CurrentHealth = MaxHealth;
            Debug.Log("REGULAR PLAYER STATS");
        }
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
                isFree = bool.Parse(MeleeAttack(NPCMask)[0]);
            }
            
            //Test functions
            if (Input.GetKeyDown("k"))
            {
                gold.DecrementGold(5);
                Gold.CreateIndicator(transform.position, 5, goldIndicator,false);
            }
            if (Input.GetKeyDown("j"))
            {
                gold.IncrementGold(5);
                Gold.CreateIndicator(transform.position, 5, goldIndicator,true);
            }
        }


        return isFree;
    }

    protected override string[] MeleeAttack(LayerMask mask)
    {
        string[] inList = base.MeleeAttack(mask);
        int enemyHealth = int.Parse(inList[1]);
        if (enemyHealth <= 0)
        {
            gold.IncrementGold(5);
            Gold.CreateIndicator(transform.position, 5, goldIndicator,true);

        }
        Debug.Log( " Enemy health:"+enemyHealth);

        return inList;
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
    
    public int CurrentLevel
    {
        get => currentLevel;
        set => currentLevel = value;
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

    void SpendMana(int mana)
    {
        currentMana -= mana;
        manaBar.SetMana(currentMana);
    }

    void RegenMana(int mana)
    {
        currentMana += mana;
        manaBar.SetMana(currentMana);
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
