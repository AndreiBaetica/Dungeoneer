using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private int[,] gridArray;
    
    public GridSystem(int width, int height)
    {
        this.width = width;
        this.height = height;

        gridArray = new int[width, height];
        
        Debug.Log(width + "," +height);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
