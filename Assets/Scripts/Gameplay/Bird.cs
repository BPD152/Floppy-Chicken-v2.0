using UnityEngine;

/// <summary>
/// Bird: lơ lửng nhấp nhô lúc chờ (Waiting) -> nhảy/xoay/rơi khi chơi (Playing).
/// Va chạm lọc tag:
///  - Pipe   -> tiếng Hit  + chết
///  - Ground -> tiếng Die  + chết
///  - Ceiling-> bỏ qua (chỉ chặn, không chết, không tiếng)
/// </summary>
public class Bird : MonoBehaviour
{
    [Header("Jump / Rotation (giữ như cũ)")]
    public float JumpForce;
    public float UpFace;
    public float DownFace;
    public float DownFaceSpeed;
    public float UpFaceSpeed;
    public Rigidbody2D Rigidbody;

    [Header("Hover lúc chờ (Waiting)")]
    public float HoverAmplitude = 0.25f;   // biên độ nhấp nhô (đơn vị world)
    public float HoverFrequency = 2f;      // tốc độ nhấp nhô

    private LogicScript logic;
    private float TargetAngle;
    private float AngleSpeed;

    private float startY;          // vị trí Y gốc để dao động quanh nó
    private float originalGravity; // nhớ gravityScale ban đầu để bật lại khi chơi

    private void Start()
    {
        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
            logic = logicObj.GetComponent<LogicScript>();

        Rigidbody.rotation = 0f;
        TargetAngle = DownFace;

        startY = transform.position.y;
        originalGravity = Rigidbody.gravityScale;

        // Bắt đầu ở trạng thái chờ: tắt trọng lực để Bird không rơi.
        Rigidbody.gravityScale = 0f;
    }

    private void Update()
    {
        if (logic == null) return;

        switch (logic.State)
        {
            case LogicScript.GameState.Waiting:
                HoverUpdate();
                break;

            case LogicScript.GameState.Playing:
                PlayingUpdate();
                break;

            // GameOver: không xử lý input nữa (Bird rơi tự do theo trọng lực đã bật).
        }
    }

    // Lơ lửng nhấp nhô quanh startY bằng hàm sin.
    private void HoverUpdate()
    {
        float y = startY + Mathf.Sin(Time.time * HoverFrequency) * HoverAmplitude;
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }

    // Chế độ chơi: xoay góc + nhảy khi có input (giữ logic cũ).
    private void PlayingUpdate()
    {
        Rigidbody.rotation = Mathf.MoveTowards(Rigidbody.rotation, TargetAngle, AngleSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || IsTouchingScreen())
        {
            Rigidbody.linearVelocityY = JumpForce;
            TargetAngle = UpFace;
            AngleSpeed = UpFaceSpeed;
            SoundManager.Instance?.PlayRandomFlap();
        }
        else if (Rigidbody.linearVelocityY < 0)
        {
            TargetAngle = DownFace;
            AngleSpeed = DownFaceSpeed;
        }
    }

    // Tap to Start gọi khi bắt đầu chơi: bật lại trọng lực để Bird rơi bình thường.
    public void EnableGravity()
    {
        Rigidbody.gravityScale = originalGravity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Chạm trần thì chỉ bị chặn, không chết, không tiếng.
        if (collision.collider.CompareTag("Ceiling")) return;

        // Chạm Pipe -> tiếng Hit rồi chết.
        if (collision.collider.CompareTag("Pipe"))
        {
            SoundManager.Instance?.PlayHit();
            if (logic != null) logic.GameOver();
            return;
        }

        // Chạm Ground -> tiếng Die rồi chết.
        if (collision.collider.CompareTag("Ground"))
        {
            SoundManager.Instance?.PlayDie();
            if (logic != null) logic.GameOver();
        }
    }

    private bool IsTouchingScreen()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }
}