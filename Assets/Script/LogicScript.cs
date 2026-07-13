using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LogicScript : MonoBehaviour
{
    public int PlayerScore;
    public TMP_Text ScoreText;

    [Header("Game Over")]
    public GameOverAnimator GameOverScreen;
    public GameObject ScoreCountUI;
    public TMP_Text ScoreLogText;   // ← kéo Score Log vào đây

    [Header("Camera Shake Settings")]
    public float ShakeDuration = 0.2f;
    public float ShakeMagnitude = 0.15f;

    private bool isGameOverTriggered = false;

    [ContextMenu("Increase Score")]
    public void AddScore()
    {
        PlayerScore = PlayerScore + 1;
        ScoreText.text = PlayerScore.ToString();
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayRandomPoint();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnBirdHit()
    {
        if (isGameOverTriggered) return;
        isGameOverTriggered = true;

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(ShakeDuration, ShakeMagnitude);
        }

        ShowGameOverUI();
        Time.timeScale = 0f;
    }

    public void ShowGameOverUI()
    {
        // Đẩy điểm cuối vào Score Log trước khi hiện
        if (ScoreLogText != null)
        {
            ScoreLogText.text = PlayerScore.ToString();
        }

        GameOverScreen.Show();
        ScoreCountUI.SetActive(false);
    }
}