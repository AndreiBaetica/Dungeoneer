﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    public LayerMask Layer;    
    Shader seeThroughShader;    
    private Collider[] listCollidersInside;

    // Start is called before the first frame update
    void Start()
    {


        listCollidersInside = this.GetColliderInsideBox();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Collider[] GetColliderInsideBox()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, gameObject.GetComponent<Renderer>().bounds.extents, Quaternion.identity, Layer);
        if (hitColliders.Length > 0)
        {
            listCollidersInside = hitColliders;
            return hitColliders;
        }
        listCollidersInside = null;
        return null;
    }

    public void enableAllInsideColliders()
    {
        foreach (Collider item in listCollidersInside)
        {
            item.enabled = true;
        }
    }

}
