using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D _rb;
    private Animator _animator;
    
    [Header("Ground check")]
    public Transform groundCheck;
    public LayerMask ground;
    public float groundCheckRadius = 0.3f;
    private bool _isGrounded;
    private bool _jumpPressed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb) _rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        _rb.linearVelocity = new Vector2(moveInput * moveSpeed, _rb.linearVelocity.y);
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
         if (_jumpPressed && _isGrounded)
         {
             _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
         }

         _jumpPressed = false;
    }
}
