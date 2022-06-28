using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    public GameObject TilePrefab;
    public int Rows;
    public int Columns;
    public float Spacing;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Generates the Grid
    void CreateGrid()
    {   
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                // Spawn Tile and Set Parent to this Object
                GameObject tile = Instantiate(TilePrefab, this.transform);
                tile.name += i + "." + j;

                // Set Coordinates based on Spacing
                float x = i * Spacing;
                float y = j * -Spacing;

                // Set Position
                tile.transform.position = new Vector2(x, y);
            }
        }
    }
}
