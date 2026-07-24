using UnityEngine;

/// <summary>
/// Cac kieu animation popup. UIManager quyet dinh dung kieu nao.
/// </summary>
public enum PopupAnim
{
    Bounce,   // scale nho -> to, nay nhe o cuoi
    Fade      // chi mo dan, giu nguyen size
}

/// <summary>
/// Kho hieu ung mo/dong popup bang LeanTween.
/// KHONG tu chay - UIManager goi PlayOpen() / PlayClose().
/// Gan len ROOT cua prefab popup, keo Panel vao field ben duoi.
/// Dung setIgnoreTimeScale(true) de chay duoc ca khi timeScale = 0 (Game Over).
/// </summary>
public class PopupAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform panel;      // khung noi dung, cai duoc scale
    [SerializeField] private CanvasGroup canvasGroup;  // de fade, nam tren root

    [Header("Settings")]
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float startScale = 0.7f;

    private void Awake()
    {
        // Tu tao / tu tim - khong can gan gi trong prefab.
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Root cua popup chinh la panel duoc scale.
        if (panel == null) panel = transform as RectTransform;

        if (panel == null)
            Debug.LogError($"[PopupAnimator] {name} khong phai RectTransform - popup phai nam trong Canvas");
    }

    // ==================== MO ====================

    /// <summary> Chay animation mo theo kieu chi dinh. </summary>
    public void PlayOpen(PopupAnim anim)
    {
        CancelAll();

        switch (anim)
        {
            case PopupAnim.Bounce: OpenBounce(); break;
            case PopupAnim.Fade:   OpenFade();   break;
        }
    }

    private void OpenBounce()
    {
        panel.localScale = Vector3.one * startScale;
        canvasGroup.alpha = 0f;

        LeanTween.scale(panel, Vector3.one, duration)
                 .setEase(LeanTweenType.easeOutBack)
                 .setIgnoreTimeScale(true);

        LeanTween.alphaCanvas(canvasGroup, 1f, duration * 0.6f)
                 .setIgnoreTimeScale(true);
    }

    private void OpenFade()
    {
        panel.localScale = Vector3.one;   // giu nguyen size
        canvasGroup.alpha = 0f;

        LeanTween.alphaCanvas(canvasGroup, 1f, duration)
                 .setIgnoreTimeScale(true);
    }

    // ==================== DONG ====================

    /// <summary> Chay animation dong, xong thi goi onComplete de UIManager Destroy. </summary>
    public void PlayClose(PopupAnim anim, System.Action onComplete)
    {
        CancelAll();

        switch (anim)
        {
            case PopupAnim.Bounce: CloseBounce(onComplete); break;
            case PopupAnim.Fade:   CloseFade(onComplete);   break;
            default:               onComplete?.Invoke();    break;
        }
    }

    private void CloseBounce(System.Action onComplete)
    {
        LeanTween.scale(panel, Vector3.one * startScale, duration * 0.7f)
                 .setEase(LeanTweenType.easeInBack)
                 .setIgnoreTimeScale(true);

        LeanTween.alphaCanvas(canvasGroup, 0f, duration * 0.7f)
                 .setIgnoreTimeScale(true)
                 .setOnComplete(() => onComplete?.Invoke());
    }

    private void CloseFade(System.Action onComplete)
    {
        LeanTween.alphaCanvas(canvasGroup, 0f, duration)
                 .setIgnoreTimeScale(true)
                 .setOnComplete(() => onComplete?.Invoke());
    }

    // ==================== NOI BO ====================

    /// <summary> Huy tween tren ca panel lan root - 2 object khac nhau. </summary>
    private void CancelAll()
    {
        if (panel != null)       LeanTween.cancel(panel.gameObject);
        if (canvasGroup != null) LeanTween.cancel(canvasGroup.gameObject);
    }
}