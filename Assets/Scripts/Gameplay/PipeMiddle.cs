using UnityEngine;

/// <summary>
/// Trigger cộng điểm khi Bird bay qua khe giữa 2 ống.
/// - Chỉ cộng khi vật chạm đúng là Bird (lọc tag).
/// - Chỉ cộng 1 lần cho mỗi Pipe (cờ scored).
/// Tự tìm Logic qua tag "Logic" lúc Start (prefab không cần nối tay).
/// </summary>
public class PipeMiddle : MonoBehaviour
{
    private LogicScript logic;
    private bool scored = false;   // chống cộng điểm 2 lần

    private void Start()
    {
        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
            logic = logicObj.GetComponent<LogicScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Chỉ Bird mới được tính điểm.
        if (!collision.CompareTag("Bird")) return;

        // Đã cộng rồi thì thôi.
        if (scored) return;
        scored = true;

        if (logic != null)
            logic.AddScore();
    }
}