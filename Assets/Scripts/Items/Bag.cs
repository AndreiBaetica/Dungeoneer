using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUsable
{
    private int slots;

    [SerializeField]
    private GameObject bagPrefab;

    public BagScript MyBagScript
    {
        get;
        set;
    }

    public int Slots
    {
        get => slots;
    }

    public void Initialize(int slots)
    {
        if (slots >= 0)
        {
            this.slots = slots;
        }
        
    }

    //Equip the bag
    public void Use()
    {
        if (InventoryScript.MyInstance.CanAddBag) // Limit of only 1 bag for now
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);
            InventoryScript.MyInstance.AddBag(this);
            MyBagScript.OpenClose();
        }
    }
}
