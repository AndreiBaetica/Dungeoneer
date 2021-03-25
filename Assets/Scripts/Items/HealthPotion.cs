using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item, IUsable
{
    [SerializeField] private int health;
    public void Use()
    {
        if (PlayerController.instance.CurrentHealth < PlayerController.instance.MaxHealth)
        {
            Remove();
            PlayerController.instance.Heal(health);
        }
    }
}
