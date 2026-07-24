using UnityEngine;

/// <summary>
/// Cau noi giua button cua Home Scene va cac Manager o Bootstrap.
/// Button khong keo thang toi Manager duoc (khac scene), nen goi qua day.
/// </summary>
public class UIEventRaiser : MonoBehaviour
{
    /// <summary> Start Button -> vao Gameplay. </summary>
    public void StartGame()
    {
        GameManager.Instance.LoadGameplay();
    }

    /// <summary> Setting Button -> mo popup Setting. </summary>
    public void OpenSetting()
    {
        UIManager.Instance.OpenPopup(PopupType.Setting);
    }
}