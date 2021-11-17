using UnityEngine;

public abstract class CardSchema : ScriptableObject
{
    [SerializeField]
    public new string name;
    public string manaCost;
    public string powerCost;
    public string description;
    public Sprite rarity;
    public Sprite type;
    public Sprite powerType;
    public Sprite artwork;
    
    public void Print()
    {
        Debug.Log(name + ": " + description + " The card costs: " + manaCost);
    }

    public abstract bool cardAction();
}