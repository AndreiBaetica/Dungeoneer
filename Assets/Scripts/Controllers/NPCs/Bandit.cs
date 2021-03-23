using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : EnemyController
{
    // Start is called before the first frame update
    protected new void Start()
    {
        maxHealth = 20;
        name = "Bandit";
        base.Start();
    }

    // Update is called once per frame
    /*protected new void Update()
    {
        base.Update();
    }*/
}
