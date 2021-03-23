using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    //List of bag, will only use 1 bag for now however
    private List<Bag> bags = new List<Bag>();
        
    [SerializeField]
    private BagButton[] bagButtons;
    
    //For debugging only
    [SerializeField]
    private Item[] items;

    private void Awake()
    {
        Bag bag = (Bag) Instantiate((items[0]));
        bag.Initialize(16); // Set number of inventory slots (max 16 with current sprite layout)
        bag.Use(); // Equivalent of right clicking on the bag
    }

    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if (bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bags.Add(bag);
                break;
            }
        }
    }

    public bool CanAddBag
    {
        get { return bags.Count < 1; } // Limit to 1 bag for now
    }

    // Test add a bag to the player (not used for now since we are keeping the player to 1 bag limit)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Bag bag = (Bag) Instantiate(items[0]);
            bag.Initialize(16);
            bag.Use();
        }
    }

    //Open/close all bags. Kinda unnecessary for now since we only use 1 bag
    public void OpenClose()
    {
        bool closedBag = bags.Find(x => !x.MyBagScript.isOpen); 
        // true = we have at least 1 closed bag, so open all closed bags
        // false = we have all bags opened, so close all bags
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.isOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }
}
