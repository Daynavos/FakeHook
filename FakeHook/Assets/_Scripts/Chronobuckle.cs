using _Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Chronobuckle : MonoBehaviour
{
    public InputActionAsset actionAsset;

    private InputAction _slowTime;
    
    public enum BucklePhase
    {
        FullyCharged,
        Depleting,
        Charging,
        CoolDown
    }

    public BucklePhase phaseOfBuckle;
    
    [SerializeField] private float fullyChargedBuckle;

    public GameObject volume;
    public GameObject timer;

    public Image timerFill;
    
    private float _chargeLevel = 5f;
    [SerializeField] private float cooldownDuration = 1f;
    private SpriteRenderer _spriteRenderer;
    
    private float _cooldownTimer = 0f; 
    private bool _coolDown = true; 
    
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
        _chargeLevel = fullyChargedBuckle;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _slowTime = actionAsset.FindAction("Player/SlowTime");
        phaseOfBuckle = BucklePhase.FullyCharged;
        _cooldownTimer = cooldownDuration;
    }

    void Update()
    {
        if (GameController.Instance.gamePaused) return;

        if (_slowTime.IsPressed())
        {
            phaseOfBuckle = BucklePhase.Depleting;
        }
        else
        {
            if (phaseOfBuckle != BucklePhase.FullyCharged)
            {
                phaseOfBuckle = BucklePhase.Charging;
            }
        }
        
        if (phaseOfBuckle == BucklePhase.FullyCharged)
        {
            volume.SetActive(false);
            Time.timeScale = 1f;
            timer.SetActive(false);
        }
        
        if (phaseOfBuckle == BucklePhase.Depleting)
        {
            volume.SetActive(true);
            Time.timeScale = 0.5f;
            DepletingTimer();
            UpdateTimerBar();
        }
        
        if (phaseOfBuckle == BucklePhase.Charging)
        {
            volume.SetActive(false);
            Time.timeScale = 1f;
            ChargingTimer();
            UpdateTimerBar();
        }
        
        if (phaseOfBuckle == BucklePhase.CoolDown)
        {
            volume.SetActive(false);
            Time.timeScale = 1f;
            CooldownTimer();
            ChargingTimer();   
            UpdateTimerBar();
        }

    }

    private void DepletingTimer()
    {
        UpdateTimerBar();
        _chargeLevel = Mathf.Clamp(_chargeLevel - Time.deltaTime, 0f, fullyChargedBuckle);

        if (_chargeLevel <= 0f) 
        {
            Time.timeScale = 1f;
            phaseOfBuckle = BucklePhase.CoolDown;
        }
    }


    private void ChargingTimer()
    {
        UpdateTimerBar();
        _chargeLevel = Mathf.Clamp(_chargeLevel + Time.deltaTime, 0f, fullyChargedBuckle);

        if (_chargeLevel >= fullyChargedBuckle)
        {
            phaseOfBuckle = BucklePhase.FullyCharged;
        }
    }

    private void UpdateTimerBar()
    {
        timer.SetActive(true);
        timerFill.fillAmount = _chargeLevel/fullyChargedBuckle;
    }
    
    private void CooldownTimer()
    {
        _cooldownTimer -= Time.deltaTime;
        UpdateTimerBar();

        if (_cooldownTimer <= 0f)
        {
            phaseOfBuckle = BucklePhase.Charging;
        }
    }
    
}
