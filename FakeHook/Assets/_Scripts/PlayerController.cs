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
    public InputActionAsset actionAsset;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _moveValue;
    public bool maintainMomentumOnGround;
    
    [Header("Ground check")]
    public Transform groundCheck;
    public LayerMask ground;
    public float groundCheckRadius = 0.1f;
    private bool _isGrounded;
    private bool _jumpPressed;
    private bool _hookPressed;

    [Header("Hook")]
    public GameObject cursor;
    public float hookForce = 10;
    public LayerMask hooks;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = actionAsset.FindAction("Player/Move");
        _jumpAction = actionAsset.FindAction("Player/Jump");
        _rb = GetComponent<Rigidbody2D>();
        if (_rb) _rb.freezeRotation = true;
    }
    private void OnEnable()
    {
        actionAsset.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        actionAsset.FindActionMap("Player").Disable();
    }

    // Update is called once per frame
    void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
        _hookPressed = Input.GetButton("Fire1");
    }

    void FixedUpdate()
    {
        Vector2 currentVelocity = _rb.linearVelocity;
        float newX = _moveValue.x != 0 ? _moveValue.x * moveSpeed : currentVelocity.x;
        if (!maintainMomentumOnGround)
        {
            _rb.linearVelocity = !_isGrounded ? new Vector2(newX, currentVelocity.y) : new Vector2(_moveValue.x * moveSpeed, currentVelocity.y);
        }
        else
        {
            _rb.linearVelocity = new Vector2(newX, currentVelocity.y);
        }
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
         if (_jumpPressed && _isGrounded)
         {
             Jump();
         }
         if (_hookPressed && Hooked())
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

    private void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }

    private bool Hooked()
    {
        Vector2 origin = transform.position;
        Vector2 direction = ((Vector2)cursor.transform.position - origin).normalized;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, cursor.GetComponent<Cursor>().maxDistanceToPlayer, hooks);
        return hit.collider != null;
    }
}
