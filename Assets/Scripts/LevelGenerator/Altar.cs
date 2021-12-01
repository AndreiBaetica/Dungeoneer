using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Player found the altar!");
        StopInteract();
    }

    public void StopInteract()
    {
        Debug.Log("Generating next floor...");
    }
}