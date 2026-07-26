using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Dieu phoi chuyen scene toan game. Moi lenh doi scene deu qua day.
/// Moi chuyen scene deu di qua Loading Scene (nhat quan).
/// Tu reset Time.timeScale = 1 truoc khi load.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [Header("Scene Names (khop ten trong Build Settings)")]
    [SerializeField] private string homeSceneName = "Home";
    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField] private string loadingSceneName = "Loading";

    // Loading Scene doc bien nay de biet can load scene nao tiep theo.
    public string NextSceneName { get; private set; }

    // ==================== API CHUYEN SCENE ====================

    /// <summary> Vao man choi. Nut Start / Play Again goi ham nay. </summary>
    public void LoadGameplay()
    {
        GoToSceneViaLoading(gameplaySceneName);
    }

    /// <summary> Ve man Home. Nut Home goi ham nay. </summary>
    public void LoadHome()
    {
        GoToSceneViaLoading(homeSceneName);
    }

    // ==================== LOGIC CHUNG ====================

    // Set scene dich roi vao Loading Scene. Loading se lo phan con lai.
    private void GoToSceneViaLoading(string sceneName)
    {
        NextSceneName = sceneName;      // Nho scene dich
        Time.timeScale = 1f;            // Reset phong khi scene truoc dang pause/gameover
        SceneManager.LoadScene(loadingSceneName);
    }
    // ==================== THÊM vào GameManager.cs ====================
// Dán method này trong class GameManager (ví dụ dưới LoadHome()).
// Cần using UnityEngine.SceneManagement; (GameManager đã có sẵn).

/// <summary>
/// Load lại Gameplay THẲNG, KHÔNG qua Loading Scene.
/// Dùng cho nút Play Again -> chơi lại ngay, không chờ màn loading.
/// Vẫn reset Time.timeScale phòng khi scene trước đang pause.
/// </summary>
public void ReloadGameplayDirect()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(gameplaySceneName);
}
}