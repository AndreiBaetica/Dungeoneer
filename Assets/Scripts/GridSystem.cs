using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 origin;
    private int[,] gridArray; //right now it is an array of int, but we will make it an array of some object/generics in the future i think
    private TextMesh[,] debugTextArray;
    
    public GridSystem(int width, int height, float cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        
        //Debug.Log(width + "," +height);
        
        for (int x = 0; x < gridArray.GetLength(0); x ++) {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {

                //debugTextArray[x,y] =GameUtilities.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize,0,cellSize) * 0.5f, 5, Color.white,TextAnchor.MiddleCenter);
                
                Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x,y+1),Color.white,100f);
                Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x+1,y),Color.white,100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0,height),GetWorldPosition(width,height),Color.white,100f);
        Debug.DrawLine(GetWorldPosition(width,0),GetWorldPosition(width,height),Color.white,100f);
        
        SetValue(1,1,1);
    }

    private Vector3 GetWorldPosition(float x, float y)
    {
        //we are actually using the x and z plane
        return new Vector3(x,0,y) * cellSize + origin;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).z / cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x;
        int y;
        GetXY(worldPosition, out x, out y);
        SetValue(x,y,value);
    }
}
