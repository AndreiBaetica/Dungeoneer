using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCombatSystem : MonoBehaviour
{
    public static GridCombatSystem Instance { get; private set; }
    
    [SerializeField] private MovementTilemapVisual movementTilemapVisual;

    private GridSystem<EmptyGridObject> grid;
    private MovementTilemap movementTilemap;

    private void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        int mapWidth = 40;
        int mapHeight = 25;
        float cellSize = 1f;
        Vector3 origin = new Vector3(0.5f, 0 , 0.5f);

        grid = new GridSystem<EmptyGridObject>(mapWidth, mapHeight, cellSize, origin,
            (GridSystem<EmptyGridObject> g, int x, int y) => new EmptyGridObject(g, x, y));

        //movementTilemap = new MovementTilemap(mapWidth, mapHeight, cellSize, origin);
        
        //movementTilemap.SetTilemapVisual(movementTilemapVisual);
        //movementTilemap.SetAllTilemapSprite(MovementTilemap.TilemapObject.TilemapSprite.Move);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GridSystem<EmptyGridObject> GetGrid()
    {
        return grid;
    }

    public class EmptyGridObject {

        private GridSystem<EmptyGridObject> grid;
        private int x;
        private int y;

        public EmptyGridObject(GridSystem<EmptyGridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;

            Vector3 worldPos00 = grid.GetWorldPosition(x, y);
            Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
            Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
            Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

            Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
            Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
            Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
            Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
        }

        public override string ToString()
        {
            return "test";
        }
    }
    
}
