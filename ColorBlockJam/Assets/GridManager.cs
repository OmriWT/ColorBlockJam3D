using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 5;
    public int cols = 5;

    public GameObject tilePrefab;


    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            for(int z = 0; z < cols; z++)
            {
                Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
            }
        }

    }
}
