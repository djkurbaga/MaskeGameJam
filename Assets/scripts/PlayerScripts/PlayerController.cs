using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f; // Zıplama gücü

    [Header("Ground Check")]
    public Transform groundCheckPoint; // Karakterin ayak ucu
    public float checkRadius = 0.2f;   // Kontrol dairesinin çapı
    public LayerMask groundLayer;      // Neresi "yer" (Yer objelerini bu layer'a almalısın)

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 1. Yatay Hareket Girişi (A ve D)
        if (Keyboard.current != null)
        {
            float left = Keyboard.current.aKey.isPressed ? -1 : 0;
            float right = Keyboard.current.dKey.isPressed ? 1 : 0;
            moveInput = left + right;

            // 2. Zıplama Girişi (Space)
            // Sadece yerdeyken zıplamaya izin veriyoruz
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                Jump();
            }
        }

        // Karakteri Çevirme
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        // 3. Yerde miyiz kontrolü?
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, groundLayer);

        // Yatay Hareket Uygulama
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        // Direkt yukarı yönde bir kuvvet uyguluyoruz
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Editor'de yer kontrol dairesini görebilmek için:
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
            Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
    }
}