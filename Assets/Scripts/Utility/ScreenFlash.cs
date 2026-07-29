using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Nhay sang toan man hinh (flash) khi Bird die.
/// Gan vao 1 Image trang phu kin man hinh trong Canvas (alpha ban dau = 0).
/// Goi Flash() de chop 1 cai roi tat.
///
/// Chay bang unscaled time (setIgnoreTimeScale) de van chay ke ca khi timeScale = 0.
/// </summary>
[RequireComponent(typeof(Image))]
public class ScreenFlash : MonoBehaviour
{
    [Tooltip("Mau flash (de trang cho chop choi).")]
    [SerializeField] private Color flashColor = Color.white;

    [Tooltip("Do sang cao nhat khi loe (0 = khong thay, 1 = chan kin).")]
    [Range(0f, 1f)]
    [SerializeField] private float peakAlpha = 0.8f;

    [Tooltip("Thoi gian loe len (giay).")]
    [SerializeField] private float fadeInTime = 0.04f;

    [Tooltip("Thoi gian tat di (giay).")]
    [SerializeField] private float fadeOutTime = 0.18f;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();

        // Bat dau trong suot, khong chan click.
        SetAlpha(0f);
        image.raycastTarget = false;
    }

    /// <summary>Chop 1 cai roi tat.</summary>
    public void Flash()
    {
        // Huy tween cu phong goi trung.
        LeanTween.cancel(gameObject);
        SetAlpha(0f);

        // Loe len nhanh -> roi tat.
        LeanTween.value(gameObject, 0f, peakAlpha, fadeInTime)
            .setOnUpdate(SetAlpha)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                LeanTween.value(gameObject, peakAlpha, 0f, fadeOutTime)
                    .setOnUpdate(SetAlpha)
                    .setIgnoreTimeScale(true);
            });
    }

    private void SetAlpha(float a)
    {
        Color c = flashColor;
        c.a = a;
        image.color = c;
    }
}