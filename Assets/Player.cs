using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }
    
    private int maxHealth = 100;
    private int currentHealth;
    private int maxMana = 10;
    private int currentMana;

    public HealthBar healthBar;
    public ManaBar manaBar;

    private IInteractable interactable;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentHealth > 0)
        {
            TakeDamage(5);
        }

        if (Input.GetKeyDown(KeyCode.H) && currentHealth < maxHealth)
        {
            Heal(5);
        }
        
        if (Input.GetKeyDown(KeyCode.J) && currentMana > 0)
        {
            SpendMana(2);
        }
        
        if (Input.GetKeyDown(KeyCode.K) && currentMana < maxMana)
        {
            RegenMana(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    
    public void Heal(int heal)
    {
        currentHealth += heal;
        healthBar.SetHealth(currentHealth);
    }
    
    public void SpendMana(int mana)
    {
        currentMana -= mana;
        manaBar.SetMana(currentMana);
    }
    
    public void RegenMana(int mana)
    {
        currentMana += mana;
        manaBar.SetMana(currentMana);
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
