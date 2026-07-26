using UnityEngine;

/// <summary>
/// Chạy khi Gameplay Scene khởi động: mở popup Tap to Start.
/// Gắn vào một object trong Gameplay (ví dụ Logic Manager, hoặc object riêng).
/// </summary>
public class GameplayStarter : MonoBehaviour
{
    private void Start()
    {
        // UIManager sống xuyên scene (từ Bootstrap) -> gọi được ngay.
        if (UIManager.Instance != null)
            UIManager.Instance.OpenPopup(PopupType.TapToStart);
    }
}