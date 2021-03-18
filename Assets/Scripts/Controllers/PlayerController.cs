using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MovementController
{
    void Start()
    { }

    void Update()
    {
            if (moving) snapToGridSquare();
            
            if (Input.GetKeyDown("w")) moveUp();
            if (Input.GetKeyDown("a")) moveLeft();
            if (Input.GetKeyDown("s")) moveDown();
            if (Input.GetKeyDown("d")) moveRight();
    }
}