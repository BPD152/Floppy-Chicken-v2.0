using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Entry point cua game. Chay 1 lan trong Bootstrap Scene.
/// Cac Manager da init xong trong Awake (Unity chay het Awake truoc moi Start),
/// nen den Start o day chac chan Managers da san sang -> load thang Home.
/// Bootstrapper nam NGOAI [Managers] -> tu chet khi roi Bootstrap Scene.
/// </summary>
public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private string homeSceneName = "Home";

    private void Start()
    {
        // Toi day: GameManager, SoundManager, SaveManager, UIManager deu da Awake xong.
        SceneManager.LoadScene(homeSceneName);
    }
}