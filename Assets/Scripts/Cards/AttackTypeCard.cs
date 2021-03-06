﻿using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Cards/Attack Card", order = 1)]
public class AttackTypeCard : CardSchema
{
    public int damage;
    public int distance;
    public int radius;
    public string typeOfDamage;
    [CanBeNull] public int[] damageOverTime;
    public bool instantTravel = true;

    public override void cardAction()
    {
        PlayerController.attackCardAction(damage, distance, radius, typeOfDamage, damageOverTime, instantTravel);
    }
}