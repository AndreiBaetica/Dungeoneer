using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MovementController
{
    void Start()
    { }

    void Update()
    {
            if (moving) snapToGridSquare();
            
            if (Input.GetKeyDown("w")) MoveForward();
            if (Input.GetKeyDown("a")) MoveLeft();
            if (Input.GetKeyDown("s")) MoveBack();
            if (Input.GetKeyDown("d")) MoveRight();
    }
}