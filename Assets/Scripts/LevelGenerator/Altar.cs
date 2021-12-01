using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Altar : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Player found the altar!");
        UIManager.MyInstance.IncrementFinalRoomScore();
        DungeonInitializer.MyInstance.DestroyAndRepopulate();
        //Scene scene = SceneManager.GetActiveScene(); // Not the best implementation for when we will want to have saves working, but it works for now.
        //SceneManager.LoadScene(scene.name);
        StopInteract();
    }

    public void StopInteract()
    {
        Debug.Log("Generating next floor...");
    }
}