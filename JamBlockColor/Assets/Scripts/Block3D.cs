using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Block3D : MonoBehaviour
{
    public Vector2Int gridPosition;
    private float blockHeight = 0.5f;
    private Vector3 offset;
    private Plane dragPlane;

    public Material normalMaterial;
    public Material highlightMaterial;
    private Renderer blockRenderer;

    public Color blockColor;



    void Start()
    {
        
        // Başlangıçta grid'e yerleştir
        MoveTo(gridPosition);
        BoardManager.Instance.RegisterBlock(this);

        blockRenderer = GetComponent<Renderer>();
        blockRenderer.material = normalMaterial;

    }

    

    void OnMouseDown()
    {
        // Sürükleme düzlemi: Y = blockHeight
        dragPlane = new Plane(Vector3.up, new Vector3(0, blockHeight, 0));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            offset = transform.position - hitPoint;
            BoardManager.Instance.UnregisterBlock(this);
        }

        blockRenderer.material = highlightMaterial;

       
    }

    void OnMouseDrag()
    {
    
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter) + offset;
            // Grid sınırları içinde tut
            float x = Mathf.Clamp(hitPoint.x, 0, BoardManager.Instance.cols-0.5f);
            float z = Mathf.Clamp(hitPoint.z, 0, BoardManager.Instance.rows-0.5f);
            Vector3 desiredPosition = new Vector3(x, blockHeight, z);

            Vector2Int targetCell = new Vector2Int(
            Mathf.RoundToInt(x),
            Mathf.RoundToInt(z)
            );

            bool isCanMove = BoardManager.Instance.isCellAvailable(targetCell);
            Debug.Log(isCanMove);
            if (isCanMove)
                transform.position = desiredPosition;
        }
                 
    }
    
    void OnMouseUp()
    {
        // Bırakınca en yakın hücreye yapış ve kaydet
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);
        Vector2Int newGridPos = BoardManager.Instance.GetNearestGridPosition(transform.position);

        if (!BoardManager.Instance.MoveBlockTo(this, newGridPos))
        {
            // Taşınamazsa geri al
            MoveTo(gridPosition);
            BoardManager.Instance.RegisterBlock(this);
        }

        blockRenderer.material = normalMaterial;

        

    }

    public void MoveTo(Vector2Int newPos)
    {
        gridPosition = newPos;
        transform.position = BoardManager.Instance.GetWorldPosition(newPos);
    }
}
