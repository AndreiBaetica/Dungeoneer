using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Card", order = 1)]
public class Card : ScriptableObject
{
    [SerializeField]
    private Sprite image;
    //template
    public new string name;
    public string description;
    public Sprite artwork;
    public int manaCost;

    //add card ability here

    public CardScript MyCardScript
    {
        get;
        set;
    }
    
    //template
    public void Print()
    {
        Debug.Log(name + ": " + description + " The card costs: " + manaCost);
    }
    
}