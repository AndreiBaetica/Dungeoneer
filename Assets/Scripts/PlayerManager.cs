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
	public int currentHealth;
	public int maxMana = 10;
	public int currentMana;
    public Transform damageTransform;
    public GameObject damageIndicator;


	public HealthBar healthBar;
	public ManaBar manaBar;
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

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        GameObject damageGameObject = (GameObject)Instantiate(damageIndicator,damageTransform.position,damageTransform.rotation);
        damageGameObject.GetComponentInChildren().text = damage.ToString();
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