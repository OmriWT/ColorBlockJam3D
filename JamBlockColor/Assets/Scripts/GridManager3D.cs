
using UnityEngine;

public class GridManager3D : MonoBehaviour
{
    public int columns = 6;
    public int rows = 6;
    public GameObject tilePrefab;
    public float spacing = 0.1f; // Hücreler arasý boþluk

    void Start() => GenerateGrid();

    void GenerateGrid()
    {
        for (int x = 0; x < columns; x++)
            for (int y = 0; y < rows; y++)
            {
                Vector3 position = new Vector3(
                    x * (1f + spacing),
                    0f,
                    y * (1f + spacing)
                );

                Instantiate(tilePrefab, position, Quaternion.identity, transform);
            }
    }
}
