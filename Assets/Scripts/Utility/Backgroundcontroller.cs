using UnityEngine;

/// <summary>
/// Cuộn nền vô tận theo kiểu parallax.
/// Gắn script này lên TỪNG bản ảnh nền (mỗi lớp thường có 2 bản nối đuôi nhau).
/// Đặt MoveSpeed khác nhau cho từng lớp để tạo chiều sâu (lớp gần nhanh, lớp xa chậm).
///
/// Cơ chế tự khít: khi một bản trôi hết sang trái (ra khỏi mép trái màn hình),
/// nó nhảy ra SAU LƯNG bản đang ở xa nhất bên phải, cách đúng 1 chiều rộng ảnh.
/// Nhờ vậy không cần căn EndX/RespawnX bằng tay.
///
/// YÊU CẦU: các bản của cùng 1 lớp phải là anh em cùng cha
/// (ví dụ object cha 'Sea_Front' chứa 2 ảnh con).
/// </summary>
public class BackgroundControler : MonoBehaviour
{
    [Header("Tốc độ cuộn (đặt khác nhau cho từng lớp để có parallax)")]
    public float MoveSpeed = 1f;

    // Chiều rộng của 1 ảnh nền, tính theo world units. Tự đo ở Start.
    private float spriteWidth;

    // Nửa chiều rộng khung nhìn camera (world units) - để biết khi nào ảnh ra hết mép trái.
    private float cameraHalfWidth;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        // Tự đo chiều rộng ảnh từ SpriteRenderer (đã tính cả scale).
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            spriteWidth = sr.bounds.size.x;
        else
            Debug.LogError($"[BackgroundControler] {name} thiếu SpriteRenderer.");

        // Nửa bề ngang màn hình theo world units (camera orthographic).
        if (cam != null)
            cameraHalfWidth = cam.orthographicSize * cam.aspect;
    }

    private void Update()
    {
        // Đẩy sang trái.
        transform.position += Vector3.left * MoveSpeed * Time.deltaTime;

        // Khi mép PHẢI của ảnh đã đi qua hẳn mép TRÁI màn hình -> ảnh khuất hoàn toàn.
        // Lúc đó nhảy ra sau lưng bản xa nhất bên phải.
        float rightEdgeOfSprite = transform.position.x + (spriteWidth / 2f);
        float leftEdgeOfScreen = (cam != null ? cam.transform.position.x : 0f) - cameraHalfWidth;

        if (rightEdgeOfSprite < leftEdgeOfScreen)
        {
            RecycleToRight();
        }
    }

    // Đặt bản này ra sau lưng bản đang ở xa nhất bên phải (trong cùng lớp).
    private void RecycleToRight()
    {
        float maxX = transform.position.x;

        // Tìm anh em cùng cha có x lớn nhất (bản đang ở xa nhất bên phải).
        Transform parent = transform.parent;
        if (parent != null)
        {
            foreach (Transform sibling in parent)
            {
                if (sibling == transform) continue;
                if (sibling.position.x > maxX)
                    maxX = sibling.position.x;
            }
        }

        // Đặt mình ngay sau bản xa nhất, cách đúng 1 chiều rộng -> nối khít.
        Vector3 pos = transform.position;
        pos.x = maxX + spriteWidth;
        transform.position = pos;
    }
}