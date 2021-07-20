using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Cards/Shielding Card", order = 1)]
public class ShieldingTypeCard : CardSchema
{
    public int shield;

    public override void cardAction()
    {
        PlayerController.shieldingCardAction(shield);
    }
}