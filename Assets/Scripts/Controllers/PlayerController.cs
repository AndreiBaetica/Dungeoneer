using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : CharController
{
    private LayerMask NPCMask;
    protected new void Start()
    {
        NPCMask= LayerMask.GetMask("NPC");
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
            if (Input.GetKeyDown("q")) MeleeAttack(NPCMask);
    }
}