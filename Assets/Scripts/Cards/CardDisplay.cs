﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    public CardSchema card;

    public Text nameText;
    public Text manaText;
    public Text powerText;
    public Text descriptionText;
    public Image rarityBorder;
    public Image background;
    public Image powerGem;
    public Image artworkImage;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = card.name;
        manaText.text = card.manaCost;
        powerText.text = card.powerCost;
        descriptionText.text = card.description;
        rarityBorder.sprite = card.rarity;
        background.sprite = card.type;
        powerGem.sprite = card.powerType;
        artworkImage.sprite = card.artwork;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        card.Print();
        card.cardAction();
    }
    
}
