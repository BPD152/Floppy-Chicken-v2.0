using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Tu dong gan tieng click cho MOI Button trong game.
/// Dat script nay 1 lan trong Bootstrap (vi du: 1 object rong trong [Managers]).
/// Moi khi 1 scene load xong, no quet tat ca Button va them PlayClick() vao OnClick
/// - CHI THEM VAO, khong ghi de OnClick san co cua button.
///
/// Luu y: quet ca button dang inactive (pop-up chua mo) bang FindObjectsInactive.
/// </summary>
public class ButtonClickSound : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Gan cho cac button co san trong scene dau tien (khi Bootstrap vua chay).
        HookAllButtons();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HookAllButtons();
    }

    private void HookAllButtons()
    {
        // Tim ca button dang inactive (vi du button trong pop-up chua bat).
        Button[] buttons = FindObjectsByType<Button>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            // Bo listener cu (neu da gan tu lan quet truoc) roi gan lai -> tranh gan trung.
            btn.onClick.RemoveListener(PlayClickSound);
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    private void PlayClickSound()
    {
        SoundManager.Instance?.PlayClick();
    }
}