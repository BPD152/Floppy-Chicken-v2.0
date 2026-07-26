using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Chạy màn Loading: bar đầy dần trong minLoadTime, text % bám theo bar,
/// đồng thời nạp scene đích (GameManager.NextSceneName) ở nền.
/// Chỉ nhảy sang scene khi CẢ HAI xong: bar đầy VÀ scene đã nạp (progress >= 0.9).
///
/// Gắn vào node LoadingController trong Loading Scene.
/// </summary>
public class LoadingController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider loadingBar;      // node 'Loading Bar' (có component Slider)
    [SerializeField] private TMP_Text percentText;   // node 'Text (TMP)'

    [Header("Settings")]
    [SerializeField] private float minLoadTime = 1f; // thời gian tối thiểu bar chạy (giây)

    private void Start()
    {
        // Đảm bảo bar bắt đầu từ 0
        if (loadingBar != null)
        {
            loadingBar.minValue = 0f;
            loadingBar.maxValue = 1f;
            loadingBar.value = 0f;
        }
        if (percentText != null)
            percentText.text = "0%";

        StartCoroutine(LoadRoutine());
    }

    private IEnumerator LoadRoutine()
    {
        // Scene đích do GameManager quyết định trước khi vào Loading.
        string target = GameManager.Instance.NextSceneName;

        // Bắt đầu nạp scene ở nền, chưa cho nhảy sang ngay.
        AsyncOperation op = SceneManager.LoadSceneAsync(target);
        op.allowSceneActivation = false;

        float elapsed = 0f;

        // Chạy tới khi bar đầy (đủ minLoadTime) VÀ scene nạp xong.
        // Unity async chỉ chạy progress tới 0.9 rồi chờ activation -> dùng >= 0.9f.
        while (elapsed < minLoadTime || op.progress < 0.9f)
        {
            elapsed += Time.deltaTime;

            // Tỉ lệ bar theo thời gian (0 -> 1), chặn không vượt quá 1.
            float barValue = Mathf.Clamp01(elapsed / minLoadTime);

            if (loadingBar != null)
                loadingBar.value = barValue;
            if (percentText != null)
                percentText.text = Mathf.RoundToInt(barValue * 100f) + "%";

            yield return null;   // chờ frame sau
        }

        // Chốt hiển thị 100% cho gọn trước khi chuyển.
        if (loadingBar != null)   loadingBar.value = 1f;
        if (percentText != null)  percentText.text = "100%";

        // Cả hai điều kiện thỏa -> cho phép nhảy sang scene đích.
        op.allowSceneActivation = true;
    }
}