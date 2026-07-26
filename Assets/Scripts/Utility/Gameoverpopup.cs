using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gắn vào prefab Popup_GameOver (node gốc).
/// - Hiển thị điểm lượt này + high score (ĐỌC, không lưu lại; LogicScript đã lưu rồi).
/// - Play Again -> đóng popup ngay (CloseNow) rồi GameManager.LoadGameplay()
/// - Home       -> đóng popup ngay (CloseNow) rồi GameManager.LoadHome()
/// </summary>
public class GameOverPopup : MonoBehaviour
{
    [Header("Text hiển thị")]
    [SerializeField] private TMP_Text scoreText;      // điểm lượt vừa chơi
    [SerializeField] private TMP_Text bestScoreText;  // high score

    [Header("Buttons")]
    [SerializeField] private Button btnPlayAgain;
    [SerializeField] private Button btnHome;

    private void Start()
    {
        int lastScore = 0;
        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
        {
            LogicScript logic = logicObj.GetComponent<LogicScript>();
            if (logic != null) lastScore = logic.PlayerScore;
        }

        int best = (SaveManager.Instance != null) ? SaveManager.Instance.HighScore : 0;

        if (scoreText != null)     scoreText.text = lastScore.ToString();
        if (bestScoreText != null) bestScoreText.text = best.ToString();

        if (btnPlayAgain != null) btnPlayAgain.onClick.AddListener(OnPlayAgain);
        if (btnHome != null)      btnHome.onClick.AddListener(OnHome);
    }

    private void OnPlayAgain()
    {
        // Đóng popup ngay trước khi đổi scene (popup sống xuyên scene nên phải chủ động đóng).
        if (UIManager.Instance != null)
            UIManager.Instance.CloseNow();

        if (GameManager.Instance != null)
            GameManager.Instance.ReloadGameplayDirect();   // chơi lại thẳng, KHÔNG qua Loading
    }

    private void OnHome()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.CloseNow();

        if (GameManager.Instance != null)
            GameManager.Instance.LoadHome();
    }
}