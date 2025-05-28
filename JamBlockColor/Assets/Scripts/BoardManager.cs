using UnityEngine;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; set; }

    public int rows = 6;
    public int cols = 6;

    public float spacing = 0.1f;

    private Block3D[,] gridBlocks;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gridBlocks = new Block3D[cols, rows];
    }

    public Vector3 GetWorldPosition(Vector2Int gridPos)
    {

        return new Vector3(
            gridPos.x * (1f + spacing),
            0.5f, // blockHeight
            gridPos.y * (1f + spacing)
        );
    }

    public Vector2Int GetNearestGridPosition(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / (1f + spacing));
        int z = Mathf.RoundToInt(worldPos.z / (1f + spacing));
        return new Vector2Int(x, z);
    }

    public bool isCellAvailable(Vector2Int pos)
    {
        if(pos.x < 0 || pos.x >= cols || pos.y < 0 || pos.y >  rows)
            return false;                     
        return gridBlocks[pos.x, pos.y] == null;
    }

    public void RegisterBlock(Block3D block)
    {
        Vector2Int p = block.gridPosition;
        if(isCellAvailable(p))
            gridBlocks[p.x, p.y] = block;
    }

    public void UnregisterBlock(Block3D block)
    {
        Vector2Int p = block.gridPosition;
        if (p.x >= 0 && p.x < cols && p.y >= 0 && p.y < rows && gridBlocks[p.x, p.y] == block)
            gridBlocks[p.x, p.y] = null;
    }

    public bool MoveBlockTo(Block3D block, Vector2Int newPos)
    {
        if(!isCellAvailable(newPos)) return false;
        UnregisterBlock(block);
        block.MoveTo(newPos);
        RegisterBlock(block);
        return true;
    }
}
