using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generates a Grid of Tiles
public class GridMaker : MonoBehaviour
{
    public GameObject TilePrefab;                               // Tile Prefab
    public int Rows;                                            // Number of Rows
    public int Columns;                                         // Number of Columns
    public float Spacing;                                       // Spacing Between Objects
    
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Create the Grid
    void CreateGrid()
    {   
        // Rows
        for (int i = 0; i < Rows; i++)
        {
            // Columns
            for (int j = 0; j < Columns; j++)
            {
                // Spawn Tile, Set Layer to 'Tile', and Set Parent to this Object
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
