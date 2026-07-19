using UnityEngine;

/// <summary>
/// Giu ca cum [Managers] song xuyen scene.
/// Goi DontDestroyOnLoad 1 lan tren root -> tat ca con (GameManager,
/// SoundManager, SaveManager, UIManager, EventSystem) tu song theo.
/// Chong nhan doi neu quay lai Bootstrap lan nua.
/// </summary>
public class ManagersRoot : MonoBehaviour
{
    private static ManagersRoot instance;

    private void Awake()
    {
        // Da co 1 cum [Managers] tu truoc -> ban nay la thua -> huy di.
        // Dong bo voi cach Singleton<T> chong trung tung Manager con.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);   // giu ca cum + toan bo con
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}