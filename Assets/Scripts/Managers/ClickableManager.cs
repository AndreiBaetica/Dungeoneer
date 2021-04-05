using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.TerrainAPI;

public class ClickableManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private Camera camera;

    public LayerMask interactableLayerMask;

    void Start()
    {
        interactableLayerMask = LayerMask.GetMask("Interactable");
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetKeyDown(KeyCode.E)) // TODO: Refactor with better logic at some point
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, interactableLayerMask)){
                player.Interact();
            }
        }
    }
}