using UnityEngine;

/// <summary>
/// Gắn vào prefab Popup_TapToStart (node gốc).
/// Tap bất kỳ đâu trên màn hình -> bắt đầu chơi:
///   - Logic.StartPlaying()   (chuyển sang Playing -> PipeSpawner bắt đầu)
///   - Bird.EnableGravity()   (bật trọng lực -> Bird thôi lơ lửng, rơi bình thường)
///   - Đóng popup này.
/// Tự tìm Logic và Bird qua tag (popup là prefab runtime, không kéo tay được).
/// </summary>
public class TapToStartPopup : MonoBehaviour
{
    private LogicScript logic;
    private Bird bird;

    private void Start()
    {
        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null) logic = logicObj.GetComponent<LogicScript>();

        GameObject birdObj = GameObject.FindGameObjectWithTag("Bird");
        if (birdObj != null) bird = birdObj.GetComponent<Bird>();
    }

    private void Update()
    {
        // Tap / click / phím Space bất kỳ đâu -> bắt đầu.
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || IsTouchingScreen())
        {
            BeginGame();
        }
    }

    private void BeginGame()
    {
        if (logic != null) logic.StartPlaying();
        if (bird != null)  bird.EnableGravity();

        // Đóng popup qua UIManager (có animation, tắt overlay).
        if (UIManager.Instance != null)
            UIManager.Instance.ClosePopup();
    }

    private bool IsTouchingScreen()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }
}