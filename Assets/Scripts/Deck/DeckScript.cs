using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    private static DeckScript instance;

    public static DeckScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DeckScript>();
            }

            return instance;
        }
    }

    [SerializeField]
    private List<CardSchema> heldCards = new List<CardSchema>(3);
    [SerializeField]
    private CardSchema[] deckCards;

    public CardSchema getCardOne()
    {
        return heldCards[0];
    }
    
    public CardSchema getCardTwo()
    {
        return heldCards[1];
    }
    
    public CardSchema getCardThree()
    {
        return heldCards[2];
    }
    
    public void useCardOne()
    {
        CardSchema cardOneCopy = heldCards[0];
        CardSchema newCard = DrawCard();
        heldCards[0] = newCard;
        deckCards[0] = cardOneCopy;
        Shuffle(ref deckCards);
    }
    
    public void useCardTwo()
    {
        CardSchema cardOneCopy = heldCards[1];
        CardSchema newCard = DrawCard();
        heldCards[1] = newCard;
        deckCards[0] = cardOneCopy;
        Shuffle(ref deckCards);
    }
    
    public void useCardThree()
    {
        CardSchema cardOneCopy = heldCards[2];
        CardSchema newCard = DrawCard();
        heldCards[2] = newCard;
        deckCards[0] = cardOneCopy;
        Shuffle(ref deckCards);
    }
    
    public CardSchema DrawCard()
    {
        return deckCards[0];
    }
    
    void Shuffle<CardSchema>(ref CardSchema[] array)
    {
        for (int i = array.Length - 1; i >= 0; --i)
        {
            int j = Random.Range(0, i + 1);

            CardSchema tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    } 
}
