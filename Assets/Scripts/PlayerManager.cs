using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Keeps track of the player */

public class PlayerManager : MonoBehaviour
{

	#region Singleton

	public static PlayerManager instance;

	void Awake()
	{
		instance = this;
	}

	#endregion

	public GameObject player;
    public static bool isTurn = true;
	public int maxHealth = 100;
    [SerializeField]public int currentHealth;
	public int maxMana = 10;
    [SerializeField]public int currentMana;
    [SerializeField]public int currentLevel;
    public bool saved;


	public HealthBar healthBar;
	public ManaBar manaBar;
    // Start is called before the first frame update
    void Start()
    {
         
        try
        {
            saved = GameStartManager.PlayingSavedGame;
            
            if (saved)
            { 
                Debug.Log("SAVED IS TRUE!!!!!!!@@@@@@@@@");

                PlayerSaveData data = SaveSystem.LoadPlayer();
                Debug.Log("SAVE FOUND!@@@@@@@@@@@@@@");

                Debug.Log("Player controller data pos x:"+data.position[0]+" y:"
                          +data.position[1]+" z:"+data.position[2]
                          +" currenthealth:"+data.health+" mana:"+data.mana);
                currentLevel = data.level;
                currentHealth = data.health;
                currentMana = data.mana;
                float[] position = new float[3];
                var playerPos = transform.position;
                playerPos.x = data.position[0];
                playerPos.y = data.position[1];
                playerPos.z = data.position[2];

                //data.ApplyPlayerSavedData(this);
            }
            else
            {
                Debug.Log("SAVED IS FALSE!!!!!!!@@@@@@@@@");

            }

        }
        catch (Exception e)
        {
            Debug.Log("SAVE NOT FOUND!!!!!!!@@@@@@@@@EXCEPTION");
            System.Console.WriteLine(e);
            Debug.Log("REGULAR PLAYER STATS");
            throw;
        }

       
        Debug.Log("CONTINUE AFTER EXCEPTION THROW?");
        
        
        
        
        UIManager.FinalRoomScore = currentLevel;

        //currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //currentMana = maxMana;
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

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        //DamageIndicator.Create(transform.position, damage, damageIndicator);
    }

    void Heal(int heal)
    {
        currentHealth += heal;
        healthBar.SetHealth(currentHealth);
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
}