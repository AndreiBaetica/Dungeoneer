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

    private SlotScript fromSlot; // Used to know what slot we are taking an item from

    //List of bag, will only use 1 bag for now however
    private List<Bag> bags = new List<Bag>();
        
    [SerializeField]
    private BagButton[] bagButtons;
    
    //For debugging only
    [SerializeField]
    private Item[] items;
    
    public bool CanAddBag
    {
        get { return bags.Count < 1; } // Limit to 1 bag for now. Change limit here if you want more than 1 bag. 
        //IMPORTANT: REQUIRED to add more bag buttons in the scene if you want to have more than 1 bag, because AddBag relies on it to properly add the bags to the inventory
    }

    public SlotScript FromSlot
    {
        get => fromSlot;
        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.gray;
            }
        }
    }

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

    public void AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return;
            }
            
        }
        
        PlaceInEmpty(item);
    }

    private void PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                return;
            }
        }
    }

    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (slot.StackItem(item))
                {
                    return true;
                }
            }
        }

        return false;
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Bag bag = (Bag) Instantiate(items[0]);
            bag.Initialize(16);
            AddItem(bag);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
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
