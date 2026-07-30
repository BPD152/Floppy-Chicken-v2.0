using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gắn vào node GỐC của prefab Popup_Pause.
/// Cùng khuôn với SettingPopup: slider gọi thẳng SoundManager,
/// nạp giá trị đã lưu qua SaveManager. Thêm nút Resume / Home.
///
///   - Kéo slider  -> SoundManager.Instance.SetXxxVolume()
///   - Resume       -> LogicScript.ResumeGame() + UIManager.ClosePopup()
///   - Home         -> UIManager.CloseNow() + GameManager.LoadHome()
///   - Lúc mở       -> load volume đã lưu, đổ vào slider
/// </summary>
public class PausePopup : MonoBehaviour
{
    [Header("Nút")]
    [SerializeField] private Button btnResume;     // Resume_Button
    [SerializeField] private Button btnHome;       // Home_Button

    [Header("Slider âm lượng")]
    [SerializeField] private Slider sliderMusic;   // Row_Music/Slider
    [SerializeField] private Slider sliderSFX;     // Row_SFX/Slider

    private LogicScript logic;

    private void Awake()
    {
        // Tìm Logic để gọi ResumeGame().
        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
            logic = logicObj.GetComponent<LogicScript>();

        // Nút
        if (btnResume != null)
            btnResume.onClick.AddListener(OnResumeClicked);
        if (btnHome != null)
            btnHome.onClick.AddListener(OnHomeClicked);

        // Kéo slider -> gọi thẳng SoundManager (giống SettingPopup)
        if (sliderMusic != null)
            sliderMusic.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
        if (sliderSFX != null)
            sliderSFX.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
    }

    private void OnEnable()
    {
        // Mỗi lần popup bật lên: đổ giá trị đã lưu vào slider.
        LoadCurrentValues();
    }

    private void LoadCurrentValues()
    {
        SaveManager.Instance.LoadSettings(out float music, out float sfx);
        if (sliderMusic != null) sliderMusic.SetValueWithoutNotify(music);
        if (sliderSFX != null)   sliderSFX.SetValueWithoutNotify(sfx);
    }

    // ==================== NÚT ====================

    private void OnResumeClicked()
    {
        if (logic != null)
            logic.ResumeGame();                  // timeScale = 1, state -> Playing
        UIManager.Instance.ClosePopup();         // đóng có animation
    }

    private void OnHomeClicked()
    {
        UIManager.Instance.CloseNow();           // đóng ngay, không animation
        GameManager.Instance.LoadHome();         // GameManager tự reset timeScale = 1
    }
}