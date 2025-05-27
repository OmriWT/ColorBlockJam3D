using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GoalZone : MonoBehaviour
{
    public Color zoneColor;
           
    private void OnTriggerEnter(Collider other)
    {
        Block3D block = other.GetComponent<Block3D>();
        if (block != null)
        {
            if (ColorsMatch(zoneColor, block.blockColor))
            {
                Debug.Log("✅ Renk eşleşti: Blok yok ediliyor");
                Destroy(block.gameObject);
            }
            else
            {
                Debug.Log("❌ Renk uyuşmadı: Blok yok edilmiyor");
            }
        }
    }

    bool ColorsMatch(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) &&
               Mathf.Approximately(a.g, b.g) &&
               Mathf.Approximately(a.b, b.b);
    }
}
