using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gắn vào prefab Popup_GameOver (node gốc).
/// - Hiển thị điểm lượt này + high score (ĐỌC từ dữ liệu, KHÔNG lưu lại
///   vì LogicScript.GameOver() đã lưu qua SaveManager.SaveRun rồi).
/// - Nút Play Again -> GameManager.LoadGameplay()
/// - Nút Home       -> GameManager.LoadHome()
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
        // Lấy điểm lượt vừa rồi từ Logic (còn sống trong scene lúc này).
        int lastScore = 0;
        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
        {
            LogicScript logic = logicObj.GetComponent<LogicScript>();
            if (logic != null) lastScore = logic.PlayerScore;
        }

        // High score đọc từ SaveManager (đã được cập nhật trước khi popup mở).
        int best = (SaveManager.Instance != null) ? SaveManager.Instance.HighScore : 0;

        if (scoreText != null)     scoreText.text = lastScore.ToString();
        if (bestScoreText != null) bestScoreText.text = best.ToString();

        // Nối nút.
        if (btnPlayAgain != null) btnPlayAgain.onClick.AddListener(OnPlayAgain);
        if (btnHome != null)      btnHome.onClick.AddListener(OnHome);
    }

    private void OnPlayAgain()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LoadGameplay();   // tự reset timeScale + qua Loading
    }

    private void OnHome()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LoadHome();
    }
}