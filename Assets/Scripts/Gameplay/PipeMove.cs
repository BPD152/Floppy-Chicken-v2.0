using UnityEngine;

/// <summary>
/// Đẩy Pipe sang trái. Khi qua DeadZone (mốc bên trái màn hình) thì tự hủy.
/// </summary>
public class PipeMove : MonoBehaviour
{
    public float MoveSpeed;
    public float DeadZone;

    private void Update()
    {
        transform.position += Vector3.left * MoveSpeed * Time.deltaTime;

        if (transform.position.x < DeadZone)
            Destroy(gameObject);
    }
}