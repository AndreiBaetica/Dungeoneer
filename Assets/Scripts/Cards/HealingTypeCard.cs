using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Cards/Healing Card", order = 1)]
public class HealingTypeCard : CardSchema
{
    public int heal;

    public override bool cardAction()
    {
        bool cardUsed = false;
        int mana = int.Parse(manaCost);
        cardUsed = PlayerController.healingCardAction(heal, mana);
        return cardUsed;
    }
}