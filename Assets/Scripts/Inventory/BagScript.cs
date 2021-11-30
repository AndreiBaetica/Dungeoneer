using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    //Prefab for the bag slots
    [SerializeField]
    private GameObject slotPrefab;

    private CanvasGroup canvasGroup;

    private List<SlotScript> slots = new List<SlotScript>();
    
    public bool isOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }
    
    public List<SlotScript> MySlots
    {
        get
        {
            return slots;
        }
    }


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //Adds and creates the bag slots using the slot prefab
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            foreach (SlotScript slot in slots)
            {
                if (slot.StackItem(item))
                {
                    return true;
                }
            }
        }
        foreach (SlotScript slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                return true; // Added item to empty slot
            }
        }
        return false; // All slots are already filled. Item cannot be added ot bag.
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true; // Open and close
    }
}
