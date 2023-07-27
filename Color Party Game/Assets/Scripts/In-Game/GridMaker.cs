using UnityEngine;

// Generates a Grid of Tiles
public class GridMaker : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;                               // Tile Prefab
    [SerializeField] private int rows;                                            // Number of Rows
    [SerializeField] private int columns;                                         // Number of Columns
    [SerializeField] private float spacing;                                       // Spacing Between Objects
    
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    /// <summary>
    /// Create the Grid
    /// </summary>
    void CreateGrid()
    {   
        // Rows
        for (int i = 0; i < rows; i++)
        {
            // Columns
            for (int j = 0; j < columns; j++)
            {
                // Spawn Tile, Set Layer to 'Tile', and Set Parent to this Object
                GameObject tile = Instantiate(tilePrefab, this.transform);
                tile.name += i + "." + j;

                // Set Coordinates based on Spacing
                float x = i * spacing;
                float y = j * -spacing;

                // Set Position
                tile.transform.position = new Vector2(x, y);
            }
        }
    }
}
