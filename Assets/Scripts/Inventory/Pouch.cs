using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouch : MonoBehaviour, IInteractable
{
    private bool isOpen;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private void Start()
    {
        GameObject pouchUI = GameObject.Find("PouchUI");
        canvasGroup = pouchUI.GetComponent<CanvasGroup>();
    }

    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            isOpen = true;
            Debug.Log("Opening Pouch!");
            canvasGroup.alpha = 1; // sets to visible
            canvasGroup.blocksRaycasts = true; // allows clicking on it
        }
    }

    public void StopInteract()
    {
        isOpen = false;
        Debug.Log("Closing Pouch!");
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
}
