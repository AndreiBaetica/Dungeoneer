using UnityEngine;
 using System.Collections.Generic;
 
 public class HideWalls : MonoBehaviour
 {
     //The player to shoot the ray at
     public Transform player;
     //The camera to shoot the ray from
     public Transform camera;
 
     //List of all objects that we have hidden.
     public List<Transform> hiddenObjects;
 
     //Layers to hide
     public LayerMask layerMask;
 
     private void Start()
     {
         //Initialize the list
         hiddenObjects = new List<Transform>();
     }
 
     void Update()
     {
         //Find the direction from the camera to the player
         Vector3 direction = player.position - camera.position;
 
         //The magnitude of the direction is the distance of the ray
         float distance = direction.magnitude;
 
         //Raycast and store all hit objects in an array. Also include the layermaks so we only hit the layers we have specified
         RaycastHit[] hits = Physics.RaycastAll(camera.position, direction, distance, layerMask);
 
         //Go through the objects
         for (int i = 0; i < hits.Length; i++)
         {
             Transform currentHit = hits[i].transform;
 
             //Only do something if the object is not already in the list
             if (!hiddenObjects.Contains(currentHit))
             {
                 //Add to list and disable renderer
                 hiddenObjects.Add(currentHit);
                 currentHit.GetComponent<Renderer>().enabled = false;
             }
         }
 
         //clean the list of objects that are in the list but not currently hit.
         for (int i = 0; i < hiddenObjects.Count; i++)
         {
             bool isHit = false;
             //Check every object in the list against every hit
             for (int j = 0; j < hits.Length; j++)
             {
                 if (hits[j].transform == hiddenObjects[i])
                 {
                     isHit = true;
                     break;
                 }
             }
 
             //If it is not among the hits
             if (!isHit)
             {
                 //Enable renderer, remove from list, and decrement the counter because the list is one smaller now
                 Transform wasHidden = hiddenObjects[i];
                 wasHidden.GetComponent<Renderer>().enabled = true;
                 hiddenObjects.RemoveAt(i);
                 i--;
             }
         }
     }
 }