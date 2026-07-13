using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPos;
    private float shakeTimer;
    private float shakeMagnitude;

    void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = originalPos + (Vector3)(Random.insideUnitCircle * shakeMagnitude);
            // dùng unscaledDeltaTime để shake vẫn chạy được kể cả khi Time.timeScale = 0
            shakeTimer -= Time.unscaledDeltaTime;
        }
        else if (transform.localPosition != originalPos)
        {
            transform.localPosition = originalPos;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeTimer = duration;
        shakeMagnitude = magnitude;
    }
}