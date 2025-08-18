using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private InputAction _hook;
    private Rigidbody2D _rb;
    private Animator _animator;
    public InputActionAsset actionAsset;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _moveValue;
    public bool maintainMomentumOnGround;
    private bool _jumping = false;
    
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
    public float hookReachedDistance = 1f;
    private bool _hookReached = false;
    public float HookCooldown = 1f;
    private float _timer;
    private bool onCooldown = false;
    
    private LineRenderer _lineRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = actionAsset.FindAction("Player/Move");
        _jumpAction = actionAsset.FindAction("Player/Jump");
        _hook = actionAsset.FindAction("Player/ThrowHook");
        _rb = GetComponent<Rigidbody2D>();
        if (_rb) _rb.freezeRotation = true;
        _lineRenderer = GetComponent<LineRenderer>();
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
        if (_jumpAction.IsPressed())
        {
            _jumpPressed = true;
        }
        _hookPressed = _hook.IsPressed();
    }

    void FixedUpdate()
    {
        if (_hook.WasReleasedThisFrame())
        {
            StartHookCooldown();
        }
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
        
        //Jump
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, ground);
        if (_isGrounded)
        {
            _jumping = false;
        }
        if (_jumpPressed && _isGrounded)
        {
            Jump();
        }
         
        //Hook
        if (_hookPressed && (Hooked() != null) && !onCooldown)
        {
            Hook();
            LineRender(Hooked().gameObject);
            cursor.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            cursor.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
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
    }

    private void LineRender(GameObject hookTarget)
    {
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, hookTarget.transform.position);
        Transform childTransform = hookTarget.transform.GetChild(0);
        GameObject childGameObject = childTransform.gameObject;
        childGameObject.SetActive(true);
    }

    private void Hook()
    {
        Vector2 direction = (cursor.transform.position - transform.position).normalized;
        _rb.linearVelocity = direction * hookForce;
    }

    private void Jump()
    {
        if (_jumping)
        {
            return;
        }
        _jumping = true;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
    }

    private void StartHookCooldown()
    {
        onCooldown = true;
        _timer = HookCooldown;
    }

    // private bool Hooked()
    // {
    //     Vector2 origin = transform.position;
    //     Vector2 direction = ((Vector2)cursor.transform.position - origin).normalized;
    //     RaycastHit2D hit = Physics2D.Raycast(origin, direction, cursor.GetComponent<Cursor>().maxDistanceToPlayer, hooks);
    //     if (hit.distance < hookReachedDistance && !onCooldown)
    //     {
    //         Debug.Log("inRange");
    //         StartHookCooldown();
    //     }
    //     return hit.collider != null && !(onCooldown);
    // }
    
    private Collider2D Hooked()
    {
        Vector2 origin = transform.position;
        Vector2 direction = ((Vector2)cursor.transform.position - origin).normalized;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, cursor.GetComponent<Cursor>().maxDistanceToPlayer, hooks);
        if (hit.distance < hookReachedDistance && !onCooldown)
        {
            Debug.Log("inRange");
            StartHookCooldown();
        }
        return hit.collider;
    }
}
