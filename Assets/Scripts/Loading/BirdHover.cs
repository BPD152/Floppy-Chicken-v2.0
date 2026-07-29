using UnityEngine;

// Hover nhấp nhô cho Bird ở Loading Scene.
// Bird ở đây là UI Image (RectTransform) nên di chuyển bằng anchoredPosition (đơn vị pixel),
// khác Bird ở Gameplay dùng transform.position (world units).
// Công thức dao động sin giữ y hệt bản Gameplay.
public class BirdHoverUI : MonoBehaviour
{
    [Header("Hover")]
    [Tooltip("Biên độ nhấp nhô, đơn vị PIXEL (vì đây là UI). Gameplay để 0.25 world unit; UI cần lớn hơn nhiều.")]
    [SerializeField] private float hoverAmplitude = 15f;

    [Tooltip("Tần số nhấp nhô — để giống Gameplay dùng 2.")]
    [SerializeField] private float hoverFrequency = 2f;

    private RectTransform rect;
    private float startY;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        startY = rect.anchoredPosition.y;
    }

    private void Update()
    {
        float y = startY + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        Vector2 pos = rect.anchoredPosition;
        pos.y = y;
        rect.anchoredPosition = pos;
    }
}