using TMPro;
using UnityEngine;

/// <summary>
/// Trung tâm Gameplay: giữ điểm số + trạng thái game.
/// Trạng thái: Waiting (chờ tap) -> Playing (đang chơi) -> Paused (tạm dừng) -> GameOver (đã chết).
/// </summary>
public class LogicScript : MonoBehaviour
{
    public enum GameState { Waiting, Playing, Paused, GameOver }

    public GameState State { get; private set; } = GameState.Waiting;
    public bool IsPlaying => State == GameState.Playing;

    [Header("Score")]
    public int PlayerScore;
    public TMP_Text ScoreText;

    [Header("Effects")]
    [Tooltip("Flash trắng toàn màn hình khi die. Kéo object ScreenFlash vào đây. Bỏ trống vẫn chạy.")]
    public ScreenFlash screenFlash;

    private bool isGameOverTriggered = false;
    private float playTime = 0f;   // đếm thời gian từ lúc bắt đầu chơi

    // ==================== TRẠNG THÁI ====================

    public void StartPlaying()
    {
        if (State != GameState.Waiting) return;
        State = GameState.Playing;
    }

    // ==================== PAUSE ====================

    public void PauseGame()
    {
        if (State != GameState.Playing) return;   // chỉ pause khi đang chơi
        State = GameState.Paused;
        Time.timeScale = 0f;                       // đóng băng pipe, background, physics

        if (UIManager.Instance != null)
            UIManager.Instance.OpenPopup(PopupType.Pause);
    }

    public void ResumeGame()
    {
        if (State != GameState.Paused) return;
        Time.timeScale = 1f;
        State = GameState.Playing;
    }

    private void Update()
    {
        // Đếm thời gian lượt chơi khi đang Playing (để lưu vào record).
        if (State == GameState.Playing)
            playTime += Time.deltaTime;
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
        if (isGameOverTriggered) return;
        isGameOverTriggered = true;

        State = GameState.GameOver;

        // Flash trắng toàn màn hình.
        if (screenFlash != null)
            screenFlash.Flash();

        // Lưu kết quả lượt chơi (điểm + thời gian), cập nhật high score.
        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveRun(PlayerScore, playTime);

        // Mở popup Game Over qua UIManager.
        if (UIManager.Instance != null)
            UIManager.Instance.OpenPopup(PopupType.GameOver);
    }
}