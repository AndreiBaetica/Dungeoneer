using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTesting : MonoBehaviour
{
    private GridSystem<TestGridObject> grid;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridSystem<TestGridObject>(20, 20, 1f, new Vector3(0.5f, 0 , 0.5f),
            (GridSystem<TestGridObject> g, int x, int y) => new TestGridObject(g, x, y));
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestGridObject testGridObject = grid.GetGridObject(2, 2);
            if (testGridObject != null)
            {
                //modify object values here
                testGridObject.SetValue(69);
            }
        }
    }

    
    public class TestGridObject
    {
        private GridSystem<TestGridObject> grid;
        private int value;
        private int x;
        private int y;

        public TestGridObject(GridSystem<TestGridObject> grid, int x, int y)
        {
            this.grid = grid;
        }

        public void SetValue(int newValue)
        {
            value = newValue;
            grid.TriggerGridObjectChanged(x,y);
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }

}
