using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTesting : MonoBehaviour
{
    //private GridSystem<TestGridObject> grid;
    
    [SerializeField] private MovementTilemapVisual movementTilemapVisual;
    
    private MovementTilemap movementTilemap;
    
    void Awake()
    {
        
        // int mapWidth = 40;
        // int mapHeight = 25;
        // float cellSize = 1f;
        // Vector3 origin = new Vector3(0.5f, 0 , 0.5f);
        //
        // grid = new GridSystem<TestGridObject>(mapWidth, mapHeight, cellSize, origin,
        //     (GridSystem<TestGridObject> g, int x, int y) => new TestGridObject(g, x, y));
        //
        // movementTilemap = new MovementTilemap(mapWidth, mapHeight, cellSize, origin);
        
    }

    private void Start()
    {
        movementTilemap = new MovementTilemap(20, 10, 1f, Vector3.zero);
    }

    void Update()
    { 
        Vector3 origin = new Vector3(0.5f, 0 , 0.5f);
       movementTilemap.SetTilemapSprite(origin, MovementTilemap.TilemapObject.TilemapSprite.Move);
    }
    //
    //
    // public class TestGridObject
    // {
    //     private GridSystem<TestGridObject> grid;
    //     private int value;
    //     private int x;
    //     private int y;
    //
    //     public TestGridObject(GridSystem<TestGridObject> grid, int x, int y)
    //     {
    //         this.grid = grid;
    //     }
    //
    //     public void SetValue(int newValue)
    //     {
    //         value = newValue;
    //         grid.TriggerGridObjectChanged(x,y);
    //     }
    //     public override string ToString()
    //     {
    //         return value.ToString();
    //     }
    //}

}
