using TMPro;
using UnityEngine;

/// <summary>
/// Trung tâm Gameplay: giữ điểm số + trạng thái game.
/// Trạng thái: Waiting (chờ tap) -> Playing (đang chơi) -> GameOver (đã chết).
/// Bird và PipeSpawner đọc trạng thái này để biết có được hoạt động chưa.
/// </summary>
public class LogicScript : MonoBehaviour
{
    public enum GameState { Waiting, Playing, GameOver }

    // Trạng thái hiện tại. Bắt đầu ở Waiting (màn Tap to Start).
    public GameState State { get; private set; } = GameState.Waiting;

    // Tiện cho script khác hỏi nhanh "đang chơi không?"
    public bool IsPlaying => State == GameState.Playing;

    [Header("Score")]
    public int PlayerScore;
    public TMP_Text ScoreText;

    [Header("Game Over (tạm thời - sẽ đổi sang UIManager popup)")]
    public GameObject GameOverScreen;

    // Cờ chống gọi GameOver nhiều lần.
    private bool isGameOverTriggered = false;

    // ==================== TRẠNG THÁI ====================

    /// <summary> Tap to Start gọi khi người chơi tap lần đầu. </summary>
    public void StartPlaying()
    {
        if (State != GameState.Waiting) return;   // chỉ chuyển từ Waiting
        State = GameState.Playing;
    }

    // ==================== ĐIỂM SỐ ====================

    [ContextMenu("Increase Score")]
    public void AddScore()
    {
        PlayerScore++;
        if (ScoreText != null)
            ScoreText.text = PlayerScore.ToString();

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayRandomPoint();
    }

    // ==================== GAME OVER ====================

    public void GameOver()
    {
        // Chỉ chạy 1 lần dù Bird có báo va chạm nhiều lần.
        if (isGameOverTriggered) return;
        isGameOverTriggered = true;

        State = GameState.GameOver;

        // Tạm thời bật màn hình cũ. Bước sau đổi sang UIManager.OpenPopup(GameOver)
        // và gọi SaveManager.SaveRun(...) để lưu điểm.
        if (GameOverScreen != null)
            GameOverScreen.SetActive(true);
    }
}