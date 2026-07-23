using UnityEngine;

/// <summary>
/// Cac loai popup trong game. Dat chung day cho gon.
/// </summary>
public enum PopupType
{
    Setting,
    TapToStart,
    GameOver
}

/// <summary>
/// Sinh & huy popup vao PopupContainer. Moi luc chi 1 popup.
/// Overlay den dung chung: bat khi co popup, tat khi dong.
/// Overlay phai nam TREN PopupContainer trong Hierarchy de popup ve de len overlay.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("Popup Prefabs (keo tung prefab vao day)")]
    [SerializeField] private GameObject settingPopupPrefab;
    [SerializeField] private GameObject tapToStartPopupPrefab;
    [SerializeField] private GameObject gameOverPopupPrefab;

    [Header("Scene References")]
    [SerializeField] private GameObject overlay;            // tam den dung chung
    [SerializeField] private Transform popupContainer;      // noi Instantiate popup

    // Popup dang mo (null = khong co popup nao).
    private GameObject currentPopup;

    // ==================== API ====================

    /// <summary> Mo 1 popup theo loai. Neu dang co popup khac -> dong truoc. </summary>
    public void OpenPopup(PopupType type)
    {
        // Dong popup cu (neu co) truoc khi mo cai moi -> luon chi 1 popup.
        if (currentPopup != null)
            Destroy(currentPopup);

        GameObject prefab = GetPrefab(type);
        if (prefab == null)
        {
            Debug.LogError($"[UIManager] Chua gan prefab cho PopupType: {type}");
            return;
        }

        overlay.SetActive(true);                              // bat nen toi
        currentPopup = Instantiate(prefab, popupContainer);  // sinh popup vao container
    }

    /// <summary> Dong popup dang mo, tat nen toi. </summary>
    public void ClosePopup()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
        }
        overlay.SetActive(false);                            // tat nen toi
    }

    // ==================== NOI BO ====================

    // Anh xa PopupType -> prefab tuong ung.
    private GameObject GetPrefab(PopupType type)
    {
        switch (type)
        {
            case PopupType.Setting:    return settingPopupPrefab;
            case PopupType.TapToStart: return tapToStartPopupPrefab;
            case PopupType.GameOver:   return gameOverPopupPrefab;
            default:                   return null;
        }
    }
}