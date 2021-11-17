using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Cards/Shielding Card", order = 1)]
public class ShieldingTypeCard : CardSchema
{
    public int shield;

    public override bool cardAction()
    {
        bool cardUsed = false;
        int mana = int.Parse(manaCost);
        cardUsed = PlayerController.shieldingCardAction(shield, mana);
        return cardUsed;
    }
}