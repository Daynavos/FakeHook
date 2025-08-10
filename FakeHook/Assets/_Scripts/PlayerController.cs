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
    private bool _hookPressed;

    [Header("Hook")]
    public GameObject cursor;
    public float hookForce = 10;
    
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
        _hookPressed = Input.GetButton("Fire1");
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector2 currentVelocity = _rb.linearVelocity;
        float newX = moveInput != 0 ? moveInput * moveSpeed : currentVelocity.x;
        _rb.linearVelocity = new Vector2(newX, currentVelocity.y);
        
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
         if (_jumpPressed && _isGrounded)
         {
             _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
         }
         if (_hookPressed && cursor.GetComponent<Cursor>().HookAvailable())
         {
             Hook();
         }
         _jumpPressed = false;
    }

    private void Hook()
    {
        Vector2 direction = (cursor.transform.position - transform.position).normalized;
        _rb.linearVelocity = direction * hookForce;
    }
}
