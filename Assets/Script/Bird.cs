using UnityEngine;

public class Bird : MonoBehaviour
{
    public float JumpForce;
    public float UpFace;
    public float DownFace;
    public float DownFaceSpeed;
    public float UpFaceSpeed;
    public Rigidbody2D Rigidbody;

    [Header("Fall After Hit (chạy độc lập với Time.timeScale)")]
    public float FallGravity = 20f;   // gia tốc rơi mô phỏng thủ công
    public float MaxFallSpeed = 15f;  // giới hạn tốc độ rơi tối đa
    public float OffscreenViewportY = -0.15f; // dưới ngưỡng này coi như đã ra khỏi màn hình

    public LogicScript Logic;

    private float TargetAngle;
    private float AngleSpeed;
    private bool hasHit = false;
    private float fallVelocity;
    private float currentZRotation;

    void Start()
    {
        Rigidbody.rotation = 0;
        TargetAngle = DownFace;
        Logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    void Update()
    {
        if (hasHit)
        {
            FallAfterHit();
            return;
        }

        Rigidbody.rotation = Mathf.MoveTowards(Rigidbody.rotation, TargetAngle, AngleSpeed * Time.deltaTime);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || IsTouchingScreen()))
        {
            Rigidbody.linearVelocityY = JumpForce;
            TargetAngle = UpFace;
            AngleSpeed = UpFaceSpeed;
            SoundManager.Instance.PlayRandomFlap();
        }
        else if (Rigidbody.linearVelocityY < 0)
        {
            TargetAngle = DownFace;
            AngleSpeed = DownFaceSpeed;
        }
    }

    private void FallAfterHit()
    {
        // Rơi bằng unscaledDeltaTime nên không bị đứng lại khi Time.timeScale = 0
        fallVelocity -= FallGravity * Time.unscaledDeltaTime;
        fallVelocity = Mathf.Max(fallVelocity, -MaxFallSpeed);

        // QUAN TRỌNG: dùng transform.position trực tiếp, KHÔNG dùng Rigidbody.position,
        // vì Rigidbody2D chỉ đồng bộ hiển thị vào lần FixedUpdate tiếp theo,
        // mà FixedUpdate bị đứng khi Time.timeScale = 0 -> hình ảnh sẽ bị đứng yên dù giá trị nội bộ có đổi.
        transform.position += Vector3.up * fallVelocity * Time.unscaledDeltaTime;

        currentZRotation = Mathf.MoveTowards(currentZRotation, DownFace, DownFaceSpeed * Time.unscaledDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, currentZRotation);

        // Kiểm tra chim đã rơi ra khỏi khung hình (dưới đáy màn hình) chưa
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.y < OffscreenViewportY)
        {
            enabled = false; // dừng hẳn Update của Bird, không cần tính tiếp
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return;
        hasHit = true;

        fallVelocity = Rigidbody.linearVelocity.y; // giữ nguyên vận tốc hiện tại để rơi tiếp mượt, không giật cục
        currentZRotation = Rigidbody.rotation;      // giữ nguyên góc xoay hiện tại làm điểm bắt đầu

        Rigidbody.simulated = false; // tắt hẳn physics engine của chim, chuyển hoàn toàn sang điều khiển thủ công

        Logic.OnBirdHit();
    }

    bool IsTouchingScreen()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }
}