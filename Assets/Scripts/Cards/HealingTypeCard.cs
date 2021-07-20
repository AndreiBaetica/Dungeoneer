using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Cards/Healing Card", order = 1)]
public class HealingTypeCard : CardSchema
{
    public int heal;

    public override void cardAction()
    {
        PlayerController.healingCardAction(heal);
    }
}