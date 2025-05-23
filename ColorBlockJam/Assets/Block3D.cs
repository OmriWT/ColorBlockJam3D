using UnityEngine;

public class Block3D : MonoBehaviour
{
    public Vector2Int gridPosition;
    private float blockHeight = 0.5f;
    private Vector3 offset;
    private Plane dragPlane;

    void Start()
    {
        // Baþlangýçta grid'e yerleþtir
        MoveTo(gridPosition);
        BoardManager.Instance.RegisterBlock(this);
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
    }

    void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter) + offset;
            // Grid sýnýrlarý içinde tut
            float x = Mathf.Clamp(hitPoint.x, 0, BoardManager.Instance.cols - 1);
            float z = Mathf.Clamp(hitPoint.z, 0, BoardManager.Instance.rows - 1);
            transform.position = new Vector3(x, blockHeight, z);
        }
    }

    void OnMouseUp()
    {
        // Býrakýnca en yakýn hücreye yapýþ ve kaydet
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);
        Vector2Int newGridPos = new Vector2Int(x, z);

        if (!BoardManager.Instance.MoveBlockTo(this, newGridPos))
        {
            // Taþýnamazsa geri al
            MoveTo(gridPosition);
            BoardManager.Instance.RegisterBlock(this);
        }
    }

    public void MoveTo(Vector2Int newPos)
    {
        gridPosition = newPos;
        transform.position = new Vector3(newPos.x, blockHeight, newPos.y);
    }
}
