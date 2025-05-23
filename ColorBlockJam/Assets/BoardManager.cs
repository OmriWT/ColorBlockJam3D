using UnityEngine;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; set; }

    public int rows = 5;
    public int cols = 5;

    private Block3D[,] gridBlocks;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gridBlocks = new Block3D[cols, rows];
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
