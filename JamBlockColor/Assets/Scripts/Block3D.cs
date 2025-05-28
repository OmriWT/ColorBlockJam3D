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

    public LayerMask blockLayerMask;
    BoxCollider boxCollider;
    Vector3 halfExtents;


    void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        // Başlangıçta grid'e yerleştir
        MoveTo(gridPosition);
        BoardManager.Instance.RegisterBlock(this);
        halfExtents = boxCollider.bounds.extents * 1.2f;
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
        // 1) Ray / clamp ile hedef pozisyonu hesapla
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!dragPlane.Raycast(ray, out float enter)) return;
        Vector3 hit = ray.GetPoint(enter) + offset;
        float x = Mathf.Clamp(hit.x, 0, BoardManager.Instance.cols - 0.5f);
        float z = Mathf.Clamp(hit.z, 0, BoardManager.Instance.rows - 0.5f);
        Vector3 desiredPos = new Vector3(x, blockHeight, z);

        // 2) OverlapBox ile çarpışma testini yap, ama yalnızca “Blocks” layer’ındaki objeleri al
        Collider[] hits = Physics.OverlapBox(
            desiredPos,
            halfExtents,
            Quaternion.identity,
            blockLayerMask,
            QueryTriggerInteraction.Ignore
        );

        // 3) Hit’ler arasında kendimiz dışındaki bir blok varsa ENGEL VAR demektir
        bool blocked = false;
        foreach (var col in hits)
        {
            if (col.gameObject == gameObject)
                continue;    // bu bizim kendi collider’ımız, yoksay
            blocked = true;
            break;
        }

        // 4) Eğer engel yoksa pozisyonu uygula
        if (!blocked)
            transform.position = desiredPos;
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
