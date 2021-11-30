using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaPotion", menuName = "Items/Potion/ManaPotion", order = 2)]
public class ManaPotion : Item, IUsable
{
    [SerializeField] private int mana;
    public void Use()
    {
        if (PlayerController.instance.CurrentMana < PlayerController.instance.MaxMana)
        {
            Remove();
            PlayerController.instance.RegenMana(mana);
        }
    }
}
