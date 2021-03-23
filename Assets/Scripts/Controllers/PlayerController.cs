using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterController
{
    protected override void Start()
    {
        name = "Player";
        maxHealth = 100;
        base.Start();
    }

    protected override void Update()
    {
            //movement
            if (moving) snapToGridSquare();
            if (Input.GetKeyDown("w")) MoveForward();
            if (Input.GetKeyDown("a")) MoveLeft();
            if (Input.GetKeyDown("s")) MoveBack();
            if (Input.GetKeyDown("d")) MoveRight();
            
            //attack
            if (Input.GetKeyDown("q")) MeleeAttack();
    }
}