using System;
using _Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private InputAction _hookAction;
    private Rigidbody2D _rb;
    public InputActionAsset actionAsset;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _moveValue;
    public bool maintainMomentumOnGround;
    private bool _jumping = false;
    public Animator animator;
    
    [Header("Ground check")]
    public Transform groundCheck;
    public LayerMask ground;
    public float groundCheckRadius = 0.1f;
    private bool _isGrounded;
    private bool _jumpPressed;
    private bool _hookPressed;
    private bool _prevHookPressed;

    [Header("Hook")]
    public GameObject cursor;
    public float hookForce = 10;
    public LayerMask hooks;
    public float hookReachedDistance = 1f;
    private bool _hookReached = false;
    public float HookCooldown = 1f;
    private float _timer;
    private bool onCooldown = false;
    public Grapple grapple;
    private static readonly int Speed = Animator.StringToHash("Speed");

    
    private LineRenderer _lineRenderer;
    
    public GameObject audioManager;
    private AudioManager audManscript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = actionAsset.FindAction("Player/Move");
        _jumpAction = actionAsset.FindAction("Player/Jump");
        _hookAction = actionAsset.FindAction("Player/ThrowHook");
        _rb = GetComponent<Rigidbody2D>();
        if (_rb) _rb.freezeRotation = true;
        _lineRenderer = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        audManscript = audioManager.GetComponent<AudioManager>();
    }
    private void OnEnable()
    {
        actionAsset.FindActionMap("Player").Enable();
        if (grapple != null)
        {
            grapple.OnAttached += OnGrappleAttached;
            grapple.OnNotAttached += OnGrappleNotAttached;
        }
    }
    private void OnDisable()
    {
        actionAsset.FindActionMap("Player").Disable();
        if (grapple != null)
        {
            grapple.OnAttached -= OnGrappleAttached;
            grapple.OnNotAttached -= OnGrappleNotAttached;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();
        if (_jumpAction.IsPressed())
        {
            _jumpPressed = true;
        }
        _hookPressed = _hookAction.IsPressed();
    }

    void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
        
        //Movement
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
        
        if (_rb.linearVelocity.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (_rb.linearVelocity.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        
        //Jump
        if (_isGrounded)
        {
            _jumping = false;
            animator.SetFloat(Speed, Mathf.Abs(_rb.linearVelocity.x));
        }
        if (_jumpPressed && _isGrounded)
        {
            Jump();
        }

        if (_hookPressed && !_prevHookPressed && !onCooldown)
        {
            grapple.Throw(cursor.transform.position);
        }
        if (grapple.IsAttached && _hookPressed)
        {
            HookTowardsAttachPoint();
        }
        if (grapple != null && grapple.IsAttached && _prevHookPressed && !_hookPressed)
        {
            // Player released hook button while attached: detach grapple
            grapple.Detach();
            StartHookCooldown();
            // do not zero player velocity; preserve momentum (we already set linearVelocity via HookTowardsAttachPoint each frame)
        }

        //Hook
        _jumpPressed = false;
         
        //Hook Timer
        if (onCooldown)
        {
            _timer -= Time.fixedUnscaledDeltaTime;
            if (_timer <= 0)
            {
                _timer = 0;
                onCooldown = false;
            }
        }
        _prevHookPressed = _hookPressed;
    }

    private void OnGrappleAttached()
    {
        
    }

    private void OnGrappleNotAttached()
    {
        StartHookCooldown();
    }

    private void HookTowardsAttachPoint()
    {
        audManscript.PlaySFX(audManscript.grapple);

        Vector2 origin = transform.position;
        Vector2 direction = ((Vector2)grapple.AttachPoint - origin).normalized;
        // Set linear velocity toward attach point
        _rb.linearVelocity = direction * hookForce;
        // If very close to attach point, start cooldown and detach to avoid jitter/pulling through the point
        float dist = Vector2.Distance(origin, grapple.AttachPoint);
        if (dist <= hookReachedDistance && !onCooldown)
        {
            // reached close enough - release and cooldown
            grapple.Detach();
            StartHookCooldown();
        }
    }

    private void Jump()
    {
        if (_jumping)
        {
            return;
        }
        audManscript.PlaySFX(audManscript.Jump);

        _jumping = true;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }


    private void StartHookCooldown()
    {
        onCooldown = true;
        _timer = HookCooldown;
    }

    
}
