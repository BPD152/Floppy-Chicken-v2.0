using UnityEngine;

/// <summary>
/// Base class cho cac Manager Singleton.
/// Cach dung: public class SaveManager : Singleton<SaveManager> { ... }
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Bien tinh (static) giu ban duy nhat cua Manager.
    // "static" = thuoc ve class, khong thuoc ve tung object -> chi co 1 ban chung.
    private static T _instance;

    // Cong khai de moi noi goi: SaveManager.Instance, SoundManager.Instance...
    public static T Instance => _instance;

    // Kiem tra da co Instance chua, ma khong lo tao moi.
    // SceneInitializer se dung cai nay de biet Managers da san sang chua.
    public static bool HasInstance => _instance != null;

    // Awake chay khi object vua duoc tao, truoc Start.
    // "virtual" = cho phep Manager con override them logic rieng.
    protected virtual void Awake()
    {
        // Neu da ton tai mot Instance khac roi -> ban nay la thua -> huy di.
        // Case: Play thang Gameplay Scene trong Editor, SceneInitializer load
        // Bootstrap sinh Manager moi, quay lai thi ban cu bi huy, giu ban tu Bootstrap.
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Chua co ai -> ban nay chinh la Instance duy nhat.
        _instance = this as T;

        // KHONG goi DontDestroyOnLoad o day.
        // Vi cac Manager la CON cua [Managers]. DontDestroyOnLoad chi goi 1 lan
        // tren root [Managers] (do Manager Root lam) -> con tu song theo.
    }

    // Khi object bi huy, don dep tham chieu tinh de tranh giu rac.
    protected virtual void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
}