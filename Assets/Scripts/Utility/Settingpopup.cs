using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gắn vào node GỐC của prefab Popup_Setting.
/// Popup gọi THẲNG tới SoundManager (không đi qua UIManager) vì SoundManager
/// đã có sẵn setter public, và nó luôn sống xuyên scene qua .Instance.
///
///   - Kéo slider  -> SoundManager.Instance.SetXxxVolume()
///   - Bấm Close    -> UIManager.Instance.ClosePopup()  (đóng có animation, tắt overlay)
///   - Lúc mở       -> load volume đã lưu, đổ vào slider
///
/// Không cần nối OnClick / OnValueChanged trong Inspector — script tự nối ở Awake().
/// Chỉ cần kéo 3 tham chiếu bên dưới vào.
/// </summary>
public class SettingPopup : MonoBehaviour
{
    [Header("Nút đóng")]
    [SerializeField] private Button btnClose;      // Btn_Close

    [Header("Slider âm lượng")]
    [SerializeField] private Slider sliderMusic;   // Slider_Music
    [SerializeField] private Slider sliderSFX;     // Slider_SFX

    private void Awake()
    {
        // Nút Close -> nhờ UIManager đóng popup
        if (btnClose != null)
            btnClose.onClick.AddListener(OnCloseClicked);

        // Kéo slider -> gọi thẳng SoundManager
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
        // Đọc volume đã lưu từ SaveManager (nó có sẵn LoadSettings trả về music + sfx).
        SaveManager.Instance.LoadSettings(out float music, out float sfx);

        // SetValueWithoutNotify: gán giá trị mà KHÔNG kích onValueChanged.
        // Nếu không, lúc mở popup slider sẽ tự gọi lại SetXxxVolume -> lưu thừa.
        if (sliderMusic != null) sliderMusic.SetValueWithoutNotify(music);
        if (sliderSFX != null)   sliderSFX.SetValueWithoutNotify(sfx);
    }

    private void OnCloseClicked()
    {
        UIManager.Instance.ClosePopup();
    }
}