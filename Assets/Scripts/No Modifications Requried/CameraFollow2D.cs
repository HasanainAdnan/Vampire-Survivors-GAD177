using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public float rightOffset = 3f;
    public float lookAhead = 1.5f;
    public float smoothTime = 0.25f;

    public bool lockY = true;
    public float fixedY = 1f;

    public bool useLeftLimit = false;
    public float leftLimitX = -10f;

    private Rigidbody2D targetRb;
    private float velX;
    private float velY;
    private float currentLook;

    private void Start()
    {
        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        float moveDir = 0f;
        if (targetRb != null)
        {
            moveDir = Mathf.Clamp(targetRb.linearVelocity.x / 5f, -1f, 1f);
        }

        currentLook = Mathf.Lerp(currentLook, moveDir * lookAhead, Time.deltaTime * 2f);

        float targetX = target.position.x + rightOffset + currentLook;
        if (useLeftLimit)
        {
            targetX = Mathf.Max(targetX, leftLimitX);
        }

        float targetY = lockY ? fixedY : target.position.y;

        Vector3 pos = transform.position;
        pos.x = Mathf.SmoothDamp(pos.x, targetX, ref velX, smoothTime);
        pos.y = Mathf.SmoothDamp(pos.y, targetY, ref velY, smoothTime);
        transform.position = pos;
    }
}