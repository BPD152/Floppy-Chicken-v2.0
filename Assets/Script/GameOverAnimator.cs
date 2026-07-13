using UnityEngine;

public class GameOverAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer overlaySprite;
    [SerializeField] private RectTransform gameOverText;
    [SerializeField] private RectTransform playAgainButton;

    [Header("Overlay")]
    [SerializeField] private float overlayTargetAlpha = 0.7f;
    [SerializeField] private float overlayDuration = 0.3f;

    [Header("Game Over Text")]
    [SerializeField] private float textDuration = 0.5f;
    [SerializeField] private float textDelay = 0.2f;

    [Header("Play Again Button")]
    [SerializeField] private float buttonDuration = 0.4f;
    [SerializeField] private float buttonDelay = 0.5f;

    private Vector3 gameOverTextScale;
    private Vector3 playAgainButtonScale;

    void Awake()
    {
        gameOverTextScale = gameOverText.localScale;
        playAgainButtonScale = playAgainButton.localScale;

        gameOverText.localScale = Vector3.zero;
        playAgainButton.localScale = Vector3.zero;
        SetOverlayAlpha(0f);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        gameOverText.localScale = Vector3.zero;
        playAgainButton.localScale = Vector3.zero;
        SetOverlayAlpha(0f);

        // 1. Overlay tối dần
        LeanTween.value(gameObject, 0f, overlayTargetAlpha, overlayDuration)
            .setOnUpdate(SetOverlayAlpha)
            .setIgnoreTimeScale(true);

        // 2. Chữ Game Over bung ra
        LeanTween.scale(gameOverText, gameOverTextScale, textDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setDelay(textDelay)
            .setIgnoreTimeScale(true);

        // 3. Nút Play Again xuất hiện sau
        LeanTween.scale(playAgainButton, playAgainButtonScale, buttonDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setDelay(buttonDelay)
            .setIgnoreTimeScale(true);
    }

    private void SetOverlayAlpha(float a)
    {
        Color c = overlaySprite.color;
        c.a = a;
        overlaySprite.color = c;
    }
}