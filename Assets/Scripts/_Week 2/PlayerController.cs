using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Feel")]
    public float acceleration = 30f;
    public float deceleration = 50f;
    public float turnAcceleration = 60f;

    private Rigidbody2D rb;
    private float moveX;

    public float MoveX => moveX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveX = 0f;

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveX -= 1f;
        }

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveX += 1f;
        }
    }

    private void FixedUpdate()
    {
        float currentX = rb.linearVelocity.x;
        float targetX = moveX * moveSpeed;
        float rate;

        if (moveX == 0f)
        {
            rate = deceleration;
        }
        else if (Mathf.Sign(moveX) != Mathf.Sign(currentX) && Mathf.Abs(currentX) > 0.1f)
        {
            rate = turnAcceleration;
        }
        else
        {
            rate = acceleration;
        }

        float newX = Mathf.MoveTowards(currentX, targetX, rate * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }
}