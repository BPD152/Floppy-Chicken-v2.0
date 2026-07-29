using UnityEngine;

public class ScreenBoundAnchor : MonoBehaviour
{
    public enum Edge { Top, Bottom }

    [SerializeField] private Edge edge;
    [Tooltip("Số dương = lún vào trong màn hình. Số âm = đẩy ra ngoài.")]
    [SerializeField] private float inset = 0f;

    void Start()
    {
        Camera cam = Camera.main;

        // viewportY: 1 = đỉnh màn hình, 0 = đáy màn hình
        float viewportY = (edge == Edge.Top) ? 1f : 0f;

        // Khoảng cách từ camera tới mặt phẳng game (2D thường để z = 0)
        float depth = -cam.transform.position.z;

        Vector3 edgeWorld = cam.ViewportToWorldPoint(
            new Vector3(0.5f, viewportY, depth));

        // Top thì lún xuống (trừ), Bottom thì lún lên (cộng)
        float direction = (edge == Edge.Top) ? -1f : 1f;
        edgeWorld.y += inset * direction;

        Vector3 pos = transform.position;
        pos.y = edgeWorld.y;
        transform.position = pos;
    }
}