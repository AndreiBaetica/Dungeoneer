using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Card", menuName = "Items/Card/Shield", order = 1)]
public class ShieldCard : CardSchema 
{
    public override void cardAction()
    {
        // ABILITY ACTION GOES HERE (ROSS)
        Debug.Log("used shield card");
    }
}
