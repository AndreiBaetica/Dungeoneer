using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisual : MonoBehaviour
{
    [SerializeField] private GridVisual gridVisual;
    private GridSystem<int> grid;

    public void SetGrid(GridSystem<int> grid)
    {
        this.grid = grid;
        //gridVisual.SetGrid();
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
