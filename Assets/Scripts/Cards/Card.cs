using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Card", order = 1)]
public class Card : ScriptableObject
{
    [SerializeField]
    private Sprite image;
    
    //add card ability here

    public CardScript MyCardScript
    {
        get;
        set;
    }
}