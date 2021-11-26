using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;

    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }
    
    public IMoveable MyMoveable
    {
        get;
        set;
    }

    private Image icon;

    [SerializeField]
    private Vector3 offset;
    
    private void Start()
    {
        icon = GetComponent<Image>();
    }

    private void Update()
    {
        icon.transform.position = Input.mousePosition;
        
        DeleteItem();
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }

    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;

        MyMoveable = null;

        icon.color = new Color(0, 0, 0, 0);

        return tmp;
    }

    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    private void DeleteItem() // TODO : add confirmation window
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null) // left click over a non-ui object with a moveable in hand
        {
            if (MyMoveable is Item && InventoryScript.MyInstance.FromSlot != null) // im moving an item
            {
                (MyMoveable as Item).MySlot.Clear(); // clear the slot
            }

            Drop(); // remove it from my hand

            InventoryScript.MyInstance.FromSlot = null; // reset fromslot in inventory
        }
    }
}
