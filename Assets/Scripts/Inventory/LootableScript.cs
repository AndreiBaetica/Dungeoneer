using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableScript : BagScript
{
    public static LootableScript instance;
    
    //Remove once pouch and player inventory can more easily share items
    [SerializeField]
    private Item[] items;

    private HealthPotion healthPotion;
    private ManaPotion manaPotion;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        AddSlots(12);
        healthPotion = (HealthPotion)Instantiate(items[0]);
        manaPotion = (ManaPotion)Instantiate(items[1]);
    }

    public void AddHealthPotion()
    {
        AddItem(healthPotion);
    }
    
    public void AddManaPotion()
    {
        AddItem(manaPotion);
    }
}
