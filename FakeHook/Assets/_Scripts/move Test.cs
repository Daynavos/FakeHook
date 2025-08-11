using UnityEngine;
using UnityEngine.InputSystem;

public class moveTest : MonoBehaviour
{
    public InputActionAsset actionAsset;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    
    private Vector2 _moveValue;
    
    private Rigidbody2D _playerRigidbody;
    
    public float walkSpeed;
    public float jumpForce;
    private void OnEnable()
    {
        actionAsset.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        actionAsset.FindActionMap("Player").Disable();
    }
    
    void Start()
    {
        _moveAction = actionAsset.FindAction("Player/Move");
        _jumpAction = actionAsset.FindAction("Player/Jump");
        
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();
        
        if (_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        Walk();
    }

    private void Walk()
    {
        if (_playerRigidbody != null)
            _playerRigidbody.linearVelocity = new Vector2(_moveValue.x * walkSpeed, _playerRigidbody.linearVelocity.y);
    }

    
    private void Jump()
    {
        _playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Debug.Log("Jump");
       
    }
}
