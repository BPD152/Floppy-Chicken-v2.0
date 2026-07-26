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
/// UIManager quyet dinh popup nao dung animation nao (xem GetAnim),
/// va popup nao cho phep click overlay de dong (xem CanCloseByOverlay).
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("Popup Prefabs (keo tung prefab vao day)")]
    [SerializeField] private GameObject settingPopupPrefab;
    [SerializeField] private GameObject tapToStartPopupPrefab;
    [SerializeField] private GameObject gameOverPopupPrefab;

    [Header("Scene References")]
    [SerializeField] private GameObject overlay;         // tam den dung chung
    [SerializeField] private Transform popupContainer;   // noi Instantiate popup

    // Popup dang mo (null = khong co popup nao).
    private GameObject currentPopup;

    // Kieu animation cua popup dang mo - nho de luc dong dung dung kieu.
    private PopupAnim currentAnim;

    // Loai popup dang mo - nho de biet co cho dong bang overlay khong.
    private PopupType currentType;

    protected override void Awake()
    {
        base.Awake();                       // Singleton + DontDestroyOnLoad

        if (overlay != null)
            overlay.SetActive(false);       // dam bao luon tat luc khoi dong
    }

    // ==================== API ====================

    /// <summary> Mo 1 popup theo loai. Neu dang co popup khac -> huy ngay khong animation. </summary>
    public void OpenPopup(PopupType type)
    {
        // Dong popup cu (neu co) truoc khi mo cai moi -> luon chi 1 popup.
        if (currentPopup != null)
        {
            LeanTween.cancel(currentPopup);
            Destroy(currentPopup);
            currentPopup = null;
        }

        GameObject prefab = GetPrefab(type);
        if (prefab == null)
        {
            Debug.LogError($"[UIManager] Chua gan prefab cho PopupType: {type}");
            return;
        }

        overlay.SetActive(true);                             // bat nen toi
        currentPopup = Instantiate(prefab, popupContainer);  // sinh popup vao container
        currentAnim = GetAnim(type);                         // chot kieu animation
        currentType = type;                                  // nho loai popup dang mo

        var animator = currentPopup.GetComponent<PopupAnimator>();
        if (animator == null)
            animator = currentPopup.AddComponent<PopupAnimator>();   // tu gan luc runtime

        animator.PlayOpen(currentAnim);
    }

    /// <summary> Dong popup dang mo (co animation), tat nen toi khi animation xong. </summary>
    public void ClosePopup()
    {
        if (currentPopup == null)
        {
            overlay.SetActive(false);
            return;
        }

        // Luu lai truoc khi clear, vi callback chay sau vai frame.
        GameObject closing = currentPopup;
        PopupAnim closingAnim = currentAnim;

        currentPopup = null;                 // clear ngay de tranh double-close

        var animator = closing.GetComponent<PopupAnimator>();
        if (animator != null)
        {
            animator.PlayClose(closingAnim, () =>
            {
                if (closing != null) Destroy(closing);
                overlay.SetActive(false);
            });
        }
        else
        {
            Destroy(closing);
            overlay.SetActive(false);
        }
    }

    /// <summary>
    /// Nut Overlay goi ham NAY (thay vi goi thang ClosePopup).
    /// Chi dong khi popup hien tai cho phep dong bang overlay.
    /// </summary>
    public void OnOverlayClicked()
    {
        if (currentPopup == null) return;

        // Popup khong cho dong bang overlay (VD Game Over, TapToStart) -> bo qua.
        if (!CanCloseByOverlay(currentType)) return;

        ClosePopup();
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

    // Anh xa PopupType -> kieu animation. Sua o day, khong sua trong prefab.
    private PopupAnim GetAnim(PopupType type)
    {
        switch (type)
        {
            case PopupType.Setting:    return PopupAnim.Bounce;
            case PopupType.TapToStart: return PopupAnim.Fade;
            case PopupType.GameOver:   return PopupAnim.Bounce;
            default:                   return PopupAnim.Fade;
        }
    }

    // Popup nao cho phep click overlay de dong.
    private bool CanCloseByOverlay(PopupType type)
    {
        switch (type)
        {
            case PopupType.Setting:    return true;   // click overlay -> dong
            case PopupType.TapToStart: return false;  // khong dong bang overlay
            case PopupType.GameOver:   return false;  // khong dong bang overlay
            default:                   return true;
        }
    }// ==================== THÊM vào UIManager.cs ====================
// Dán method này trong class UIManager (ví dụ ngay dưới ClosePopup()).

/// <summary>
/// Đóng popup NGAY LẬP TỨC, không animation. Dùng khi chuyển scene
/// (Play Again / Home) - không cần chờ hiệu ứng vì scene sắp đổi.
/// </summary>
public void CloseNow()
{
    if (currentPopup != null)
    {
        LeanTween.cancel(currentPopup);
        Destroy(currentPopup);
        currentPopup = null;
    }

    if (overlay != null)
        overlay.SetActive(false);
}
}