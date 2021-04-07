using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardScript : MonoBehaviour, IPointerClickHandler
{
    private Card card;

    [SerializeField] private Sprite visible, notVisible;
    
    public Card MyCard
    {
        get => card;
        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = visible; // TODO: change this to full if the player starts with the bag closed instead of open
            }
            /*else
            {
                GetComponent<Image>().sprite = notVisible;
            }*/
            card = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData) // When the card is clicked
    {
        //TODO: ROSS: CALL CARD ABILITY FUNCTION HERE
        updateSprite();
    }
    
    public void updateSprite()
    {
        GetComponent<Image>().sprite = notVisible;
    }
}
