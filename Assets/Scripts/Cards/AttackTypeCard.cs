using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Cards/Attack Card", order = 1)]
public class AttackTypeCard : CardSchema
{
    public int damage;
    public int distance;
    public int radius;

    public override bool cardAction()
    {
        bool cardUsed = false;
        int mana = int.Parse(manaCost);
        cardUsed = PlayerController.attackCardAction(damage, distance, radius, mana);
        return cardUsed;
    }
}