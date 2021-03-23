using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterController
{
    protected new void Start()
    {
        name = "Player";
        maxHealth = 100;
        base.Start();
    }

    protected new void Update()
    {
            //movement
            if (moving) SnapToGridSquare();
            if (Input.GetKeyDown("w")) MoveForward();
            if (Input.GetKeyDown("a")) MoveLeft();
            if (Input.GetKeyDown("s")) MoveBack();
            if (Input.GetKeyDown("d")) MoveRight();
            
            //attack
            if (Input.GetKeyDown("q")) MeleeAttack();
    }
}