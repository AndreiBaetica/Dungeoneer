using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : EnemyController
{
    // Start is called before the first frame update
    protected new void Start()
    {
        maxHealth = 5;
        name = "Bandit";
        base.Start();
    }
}
