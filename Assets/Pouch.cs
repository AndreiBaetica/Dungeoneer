using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouch : MonoBehaviour, IInteractable
{
    private bool isOpen;
    
    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            isOpen = true;
            Debug.Log("Opening Pouch!"); // TODO : Remove
            // Open Pouch slots UI
        }
    }

    public void StopInteract()
    {
        // Close Pouch slots UI
    }
}
