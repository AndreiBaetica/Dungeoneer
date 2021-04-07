using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    private Bag bag;

    [SerializeField] private Sprite full, empty, open;

    public Bag MyBag
    {
        get => bag;
        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full; // TODO: change this to full if the player starts with the bag closed instead of open
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }
    
    public void Update()
    {
        //Open close inventory
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.MyInstance.OpenClose();
            updateSprite();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (bag != null)
        {
            bag.MyBagScript.OpenClose();
            updateSprite();
        }
    }

    public void updateSprite()
    {
        if (bag != null)
        {
            if (bag.MyBagScript.isOpen)
            {
                GetComponent<Image>().sprite = open;
            }
            else
            {
                GetComponent<Image>().sprite = full; // "full" acts as a closed bag
            }
        }
        else
        {
            GetComponent<Image>().sprite = empty;
        }
    }
}
