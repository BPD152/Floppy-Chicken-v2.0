using UnityEngine;

/// <summary>
/// Sinh Pipe theo chu kỳ - CHỈ khi game đang ở trạng thái Playing.
/// Sửa so với bản cũ:
///  - Không spawn trong Start() nữa (trước đây pipe hiện ngay từ màn chờ).
///  - Mỗi frame kiểm tra Logic.IsPlaying, chỉ chạy timer khi đang chơi.
///  - Random.Range truyền đúng thứ tự (min, max).
///  - Pipe spawn ra được gom vào container (nếu có) cho Hierarchy gọn.
/// </summary>
public class PipeSpawner : MonoBehaviour
{
    public GameObject Item;        // Pipe prefab
    public float SpawnRate;        // giây giữa 2 lần spawn
    public float Offset;           // biên độ ngẫu nhiên độ cao khe

    [Tooltip("Object rỗng gom các Pipe (tùy chọn). Bỏ trống cũng được.")]
    public Transform pipeContainer;

    private float timer = 0f;
    private LogicScript logic;

    private void Start()
    {
        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
            logic = logicObj.GetComponent<LogicScript>();
    }

    private void Update()
    {
        // Chưa vào Playing thì không spawn (đang ở Tap to Start hoặc đã Game Over).
        if (logic == null || !logic.IsPlaying) return;

        timer += Time.deltaTime;
        if (timer >= SpawnRate)
        {
            SpawnPipe();
            timer = 0f;
        }
    }

    private void SpawnPipe()
    {
        float lowestPoint = transform.position.y - Offset;
        float highestPoint = transform.position.y + Offset;
        float y = Random.Range(lowestPoint, highestPoint);   // đúng thứ tự (min, max)

        Vector3 spawnPos = new Vector3(transform.position.x, y, 0f);
        Instantiate(Item, spawnPos, transform.rotation, pipeContainer);
    }
}