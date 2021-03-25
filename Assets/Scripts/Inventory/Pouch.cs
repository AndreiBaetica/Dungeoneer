using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouch : MonoBehaviour, IInteractable
{
    private bool isOpen;

    [SerializeField]
    private CanvasGroup canvasGroup;
    
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
            canvasGroup.alpha = 1; // sets to visible
            canvasGroup.blocksRaycasts = true; // allows clicking on it
        }
    }

    public void StopInteract()
    {
        isOpen = false;
        Debug.Log("Closing Pouch!"); // TODO : Remove
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
}
