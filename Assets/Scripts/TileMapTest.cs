using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapTest : MonoBehaviour
{

    [SerializeField]
    //private TileBase[] tileBase;
    private Tilemap tileMap;
    
    private float[,] blockTempDataMap = new float[50, 50];

    private bool isTileShown = false;
 
    void Start ()
    {
        tileMap = GameObject.Find("Tilemap").GetComponent("Tilemap") as Tilemap;
     
        for (int y = -10; y < blockTempDataMap.GetLength(0); y++)
        {
            for (int x = -10; x < blockTempDataMap.GetLength(1); x++)
            {
                //tileMap.SetTile(new Vector3Int(y, x, 0), tileBase[0]);
                //tileMap.SetColor(new Vector3Int(1, 0, 1), Color.red);
                
                tileMap.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                tileMap.SetColor(new Vector3Int(x, y, 0), Color.clear);
            }
        }

    }
    
    
    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyUp("p"))
        {
            
            if (isTileShown)
            {
                
                for (int y = -10; y < blockTempDataMap.GetLength(0); y++)
                {
                    for (int x = -10; x < blockTempDataMap.GetLength(1); x++)
                    {

                        tileMap.SetColor(new Vector3Int(x, y, 0), Color.clear);
                    }
                }

                isTileShown = false;
            }
            else
            {
                
                for (int y = -10; y < blockTempDataMap.GetLength(0); y++)
                {
                    for (int x = -10; x < blockTempDataMap.GetLength(1); x++)
                    {

                        tileMap.SetColor(new Vector3Int(x, y, 0), Color.white);
                    }
                }

                isTileShown = true;
            }
        }
    }
}
