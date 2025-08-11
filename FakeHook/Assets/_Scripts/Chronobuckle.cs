using UnityEngine;
using UnityEngine.InputSystem;
public class Chronobuckle : MonoBehaviour
{
    public InputActionAsset actionAsset;

    private InputAction _slowTime;

    private bool _resetTimer;

    public enum BucklePhase
    {
        CanUse,
        InUse,
        CoolDown
    }

    [SerializeField] private BucklePhase _phaseOfBuckle;
    
    [SerializeField] private float activeTime;
    [SerializeField] private float cooldownTime;
    private float _time = 0f;
    
    private SpriteRenderer _spriteRenderer;
    
    void OnEnable()
    {
        actionAsset.FindActionMap("Player").Enable();
    }

    void OnDisable()
    {
        actionAsset.FindActionMap("Player").Disable();
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _slowTime = actionAsset.FindAction("Player/SlowTime");
        _phaseOfBuckle = BucklePhase.CanUse;
    }

    void Update()
    {
        if (_resetTimer)
        {
            ResetTimer();
        }
        
        if (_slowTime.WasPressedThisFrame())
        {
            if (_phaseOfBuckle == BucklePhase.CanUse)
            {
                _phaseOfBuckle = BucklePhase.InUse;
            }
        }

        if (_slowTime.WasReleasedThisFrame())
        {
            _resetTimer = true;
            if (_phaseOfBuckle == BucklePhase.InUse)
            {
                _phaseOfBuckle = BucklePhase.CanUse;
            }

            if (_phaseOfBuckle == BucklePhase.CoolDown)
            {
                return;
            }
        }
        
        if (_phaseOfBuckle == BucklePhase.CanUse)
        {
            _spriteRenderer.color = Color.red;
            Time.timeScale = 1f;
        }

        if (_phaseOfBuckle == BucklePhase.InUse)
        {
            _spriteRenderer.color = Color.green;
            Time.timeScale = 0.5f;
            SlowDownActiveTimer();
            
        }

        if (_phaseOfBuckle == BucklePhase.CoolDown)
        {
            _spriteRenderer.color = Color.blue;
            Time.timeScale = 1f;
            CooldownTimer();
        }
        
    }

    private void SlowDownActiveTimer()
    {
        _time += Time.unscaledDeltaTime;
        if (_time > activeTime)
        {
            _time = 0f;
            _phaseOfBuckle = BucklePhase.CoolDown;
            Debug.Log("TimeOut");
            
        }
    }

    private void CooldownTimer()
    {
        _time += Time.unscaledDeltaTime;
        if (_time > cooldownTime)
        {
            _time = 0f;
            _phaseOfBuckle = BucklePhase.CanUse;
            Debug.Log("Go Again");
            
        }
    }

    private void ResetTimer()
    {
        _time = 0f;
        _resetTimer = false;
    }
    
}
