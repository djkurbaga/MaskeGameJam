using UnityEngine;
using UnityEngine.InputSystem; // Yeni kütüphaneyi ekledik

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Yeni sistemde klavye okuma bu kadar basit:
        if (Keyboard.current != null)
        {
            float left = Keyboard.current.aKey.isPressed ? -1 : 0;
            float right = Keyboard.current.dKey.isPressed ? 1 : 0;
            moveInput = left + right;
        }

        // Karakteri sağa sola çevirme (Flip)
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}