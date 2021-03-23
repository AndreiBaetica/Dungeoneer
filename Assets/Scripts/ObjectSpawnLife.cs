using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destroy the gameObject or component after a timer
public class ObjectSpawnLife : MonoBehaviour
{
    // Object can be a GameObject or a component
    public Object myGameObjectOrComponent;
    public float timer;

    // Start is called before the first frame update
 void Start(){
  // Default is the gameObject
  if (myGameObjectOrComponent == null)
   myGameObjectOrComponent = gameObject;

  // Destroy works with GameObjects and Components
  Destroy(myGameObjectOrComponent, timer);
 }

}

public class SetLifeSpawn : MonoBehaviour {




}