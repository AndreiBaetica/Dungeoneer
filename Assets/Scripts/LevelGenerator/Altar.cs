using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Altar : MonoBehaviour, IInteractable
{
    [SerializeField] private bool HomeAltar = false;
    public void Interact()
    {
        Debug.Log("Player found the altar!");
        if (HomeAltar)
        {
            SceneManager.LoadScene("TestDungeon");
        }
        else
        {
            UIManager.MyInstance.IncrementFinalRoomScore();
        }
        DungeonInitializer.MyInstance.DestroyAndRepopulate();
        StopInteract();
    }

    public void StopInteract()
    {
        Debug.Log("Generating next floor...");
    }
}