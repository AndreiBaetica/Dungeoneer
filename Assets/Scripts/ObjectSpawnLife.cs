using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroy the gameObject or component after a timer
public class ObjectSpawnLife : MonoBehaviour
{

    [SerializeField] private float secondsToDespawn = 1f;

    // Start is called before the first frame update
     void Start(){
      // Default is the gameObject

          // Destroy works with GameObjects and Components
          Destroy(gameObject, secondsToDespawn);
     }


}