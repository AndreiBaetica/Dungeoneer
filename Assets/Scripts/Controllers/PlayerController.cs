using System.Collections;
//using ICSharpCode.NRefactory.Ast;
using UnityEngine;

public class PlayerController : ActorController
{
    #region Singleton

    public static PlayerController instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    
    public HealthBar healthBar;
    public ShieldBar shieldBar;
    public ManaBar manaBar;
    private int maxMana;
    private int maxShield;
    [SerializeField]private int currentShield;
    [SerializeField]public int currentMana;
    [SerializeField]public int currentLevel;    
    private LayerMask NPCMask;
    private IInteractable interactable;
    public bool saved;
    private GameObject playerCube;


    protected new void Start()
    {
        saved = GameStartManager.PlayingSavedGame;
        Debug.Log("SAVED IS "+saved);
        NPCMask = LayerMask.GetMask("NPC");
        name = "Player";
        MaxHealth = 15;
        MaxMana = 10;
        MaxShield = 15;
        CurrentLevel = 0;
        base.Start();
        healthBar.SetMaxHealth(MaxHealth);
        manaBar.SetMaxMana(MaxMana);
        shieldBar.SetMaxShield(MaxShield);
        playerCube = GameObject.Find("Player/Cube");
        if (saved)
        {
            PlayerSaveData data = SaveSystem.LoadPlayer();

            Debug.Log("Player controller data pos x:"+data.position[0]+" y:"
                      +data.position[1]+" z:"+data.position[2]
                      +" currenthealth:"+data.health+" mana:"+data.mana);
            PlayerSaveData.ApplyPlayerSavedData(this, data); //Must update for shield, room #, gold
        }
        else
        {
            CurrentMana = MaxMana;
            CurrentHealth = MaxHealth;
            CurrentShield = 0;
            Debug.Log("REGULAR PLAYER STATS");
        }
    }

    protected new void Update()
    {
        healthBar.SetHealth(CurrentHealth);
        manaBar.SetMana(CurrentMana);
        shieldBar.SetShield(CurrentShield);

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
            if (Input.GetKeyDown("w"))
            {
                isFree = Move(Vector3.forward);
                if (!isFree)
                {
                    LoseShield(1);
                }
            }

            if (Input.GetKeyDown("a"))
            {
                isFree = Move(Vector3.left);
                if (!isFree)
                {
                    LoseShield(1);
                }
            }

            if (Input.GetKeyDown("s"))
            {
                isFree = Move(Vector3.back);
                if (!isFree)
                {
                    LoseShield(1);
                }
            }

            if (Input.GetKeyDown("d"))
            {
                isFree = Move(Vector3.right);
                if (!isFree)
                {
                    LoseShield(1);
                }
            }
            //attack
            if (Input.GetKeyDown("q"))
            {
                isFree = MeleeAttack(NPCMask);
                if (!isFree)
                {
                    LoseShield(1);
                }
            }
            //rest
            if (Input.GetKeyDown("r"))
            {        
                Debug.Log("Skipped turn and regained 5 mana!");
                RegenMana(5); 
                isFree = false;
                LoseShield(1);
                doneTurn = true;
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

        if (collision.tag == "BeginGame")
        {
            Debug.Log("PLAYER COLLIDED RUINS");
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

    protected override void TakeDamage(int damage)
    {
        if (damage >= CurrentShield && CurrentShield > 0)
        {
            int extraDamage = damage - CurrentShield;
            CurrentShield = 0;
            CurrentHealth -= extraDamage;
        }
        else if(damage < CurrentShield && CurrentShield > 0)
        {
            CurrentShield -= damage;
        }
        else if(CurrentShield <= 0)
        {
            CurrentHealth -= damage;
        }
        base.TakeDamage(damage);
        
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

    public int MaxShield
    {
        get => maxShield;
        set => maxShield = value;
    }
    
    public int CurrentShield
    {
        get => currentShield;
        set => currentShield = value;
    }
    
    public void RegenMana(int mana)
    {
        currentMana += mana;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        manaBar.SetMana(currentMana);
    }

    public bool SpendMana(int mana)
    {
        if (mana > currentMana)
        {
            //Show something to say that you dont have enough mana
            Debug.Log("NOT ENOUGH MANA!");
            return false;
        }
        else
        {
            currentMana -= mana;
        }
        manaBar.SetMana(currentMana);
        return true;
    }

    public void GainShield(int shield)
    {
        currentShield += shield;
        if (currentShield > maxShield)
        {
            currentShield = maxShield;
        }
        shieldBar.SetShield(currentShield);
    }

    public void LoseShield(int shield)
    {
        if (shield > currentShield)
        {
            currentShield = 0;
        }
        else
        {
            currentShield -= shield;
        }
        shieldBar.SetShield(currentShield);
    }
    
    public static bool attackCardAction(int damage, int distance, int radius, int mana)
    {
        if (instance.SpendMana(mana))
        {
            
            Destroy(GameObject.Find("FireCircle(Clone)"));
            FireCircle._damage = damage;
            FireCircle._radius = radius;
            var selectedSpell = Resources.Load("spells/FireCircle");
            var playerPosition = instance.playerCube.transform.position;
            GameObject currentSpell = null;
            if (instance._dirFacing == CharacterDir.Back)
            {
                currentSpell = (GameObject) Instantiate(selectedSpell,
                    new Vector3(playerPosition.x, playerPosition.y, playerPosition.z - distance),
                    Quaternion.identity);
            }
            else if (instance._dirFacing == CharacterDir.Forward)
            {
                currentSpell = (GameObject) Instantiate(selectedSpell,
                    new Vector3(playerPosition.x, playerPosition.y, playerPosition.z + distance),
                    Quaternion.identity);
            }
            else if (instance._dirFacing == CharacterDir.Left)
            {
                currentSpell = (GameObject) Instantiate(selectedSpell,
                    new Vector3(playerPosition.x - distance, playerPosition.y, playerPosition.z),
                    Quaternion.identity);
            }
            else if (instance._dirFacing == CharacterDir.Right)
            {
                currentSpell = (GameObject) Instantiate(selectedSpell,
                    new Vector3(playerPosition.x + distance, playerPosition.y, playerPosition.z),
                    Quaternion.identity);
            }

            SphereCollider hitbox = currentSpell.transform.GetChild(0).gameObject.GetComponent<SphereCollider>();
            hitbox.radius = radius;
            // TODO: add creation particle effect here

            Debug.Log("Used attack type card");
            instance.LoseShield(1);
            instance.doneTurn = true;
            GameLoopManager.SetPlayerTurn(false);
            return true;
        }

        return false;
    }

    public static bool healingCardAction(int heal, int mana)
    {
        if (instance.SpendMana(mana))
        {
            instance.Heal(heal);
            Debug.Log("Used healing type card");
            instance.LoseShield(1);
            instance.doneTurn = true;
            GameLoopManager.SetPlayerTurn(false);
            return true;
        }

        return false;
    }

    public static bool shieldingCardAction(int shield, int mana)
    {
        if (instance.SpendMana(mana))
        {
            instance.GainShield(shield);
            Debug.Log("Used shielding type card");
            instance.doneTurn = true;
            GameLoopManager.SetPlayerTurn(false);
            return true;
        }
        
        return false;
    }
}
