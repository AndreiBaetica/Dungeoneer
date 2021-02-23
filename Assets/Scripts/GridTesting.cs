using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTesting : MonoBehaviour
{
    private GridSystem<bool> grid;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridSystem<bool>(20, 20, 1f, new Vector3(0.5f, 0 , 0.5f));
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

}
