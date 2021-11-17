using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    public CardSchema card;
    
    public int cardNumber;
    public Text nameText;
    public Text manaText;
    public Text powerText;
    public Text descriptionText;
    public Image rarityBorder;
    public Image background;
    public Image powerGem;
    public Image artworkImage;

    // Start is called before the first frame update
    void Update()
    {
        if (cardNumber == 1)
        {
            card = DeckScript.MyInstance.getCardOne();
        } else if (cardNumber == 2)
        {
            card = DeckScript.MyInstance.getCardTwo();
        } else if (cardNumber == 3)
        {
            card = DeckScript.MyInstance.getCardThree();
        }
        
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
        bool cardUsed = card.cardAction();
        
        //Replace card with one from deck
        if (cardUsed)
        {
            if (cardNumber == 1)
            {
                DeckScript.MyInstance.useCardOne();
            } else if (cardNumber == 2)
            {
                DeckScript.MyInstance.useCardTwo();
            } else if (cardNumber == 3)
            {
                DeckScript.MyInstance.useCardThree();
            }
        }
    }
    
}
