using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableScript : BagScript
{
    //Remove once pouch and player inventory can more easily share items
    [SerializeField]
    private Item[] items;
    
    // Start is called before the first frame update
    void Awake()
    {
        AddSlots(12);
        HealthPotion healthPotion = (HealthPotion)Instantiate(items[0]);
        AddItem(healthPotion);
        ManaPotion manaPotion = (ManaPotion)Instantiate(items[1]);
        AddItem(manaPotion);
        AddItem(manaPotion);
        AddItem(manaPotion);
    }
    
    //Debug purposes only
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.O))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[0]);
            AddItem(potion);
        }*/
    }
}
