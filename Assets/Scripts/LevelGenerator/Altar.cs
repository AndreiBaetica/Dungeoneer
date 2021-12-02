using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Altar : MonoBehaviour, IInteractable
{
    [SerializeField] private bool HomeAltar = false;

    /*private void Update() // testing only TODO : remove
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Interact();
        }
    }*/

    public void Interact()
    {
        Debug.Log("Player found the altar!");
        if (HomeAltar)
        {
            SceneManager.LoadScene("TestDungeon");
        }
        else
        {
            DungeonInitializer.MyInstance.DestroyAndRepopulate();
        }
        UIManager.MyInstance.IncrementFinalRoomScore();
        FloorLevel.MyInstance.UpdateFloorText();
        StopInteract();
    }

    public void StopInteract()
    {
        Debug.Log("Generating next floor...");
    }
}