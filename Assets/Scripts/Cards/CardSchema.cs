using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Card", order = 1)]
public abstract class CardSchema : ScriptableObject
{
    [SerializeField]
    public new string name;
    public new string manaCost;
    public new string powerCost;
    public string description;
    public Sprite rarity;
    public Sprite type;
    public Sprite powerType;
    public Sprite artwork;

    //template
    public void Print()
    {
        Debug.Log(name + ": " + description + " The card costs: " + manaCost);
    }

    public abstract void cardAction();
}