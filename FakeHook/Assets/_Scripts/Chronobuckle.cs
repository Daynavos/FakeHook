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
        _chargeLevel = fullyChargedBuckle;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _slowTime = actionAsset.FindAction("Player/SlowTime");
        phaseOfBuckle = BucklePhase.FullyCharged;
    }

    void Update()
    {
        if (GameController.Instance.gameOver) return;

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
    }

    private void DepletingTimer()
    {
        UpdateTimerBar();
        _chargeLevel = Mathf.Clamp(_chargeLevel - Time.deltaTime, 0f, fullyChargedBuckle);

        if (_chargeLevel < 0)
        {
            if (!_slowTime.IsPressed())
            {
                phaseOfBuckle = BucklePhase.Charging;
            }
            Time.timeScale = 1f;
            phaseOfBuckle = BucklePhase.CoolDown;
        }
    }

    private void ChargingTimer()
    {
        UpdateTimerBar();
        _chargeLevel = Mathf.Clamp(_chargeLevel + Time.deltaTime, 0f, fullyChargedBuckle);

        if (_chargeLevel > fullyChargedBuckle)
        {
            phaseOfBuckle = BucklePhase.FullyCharged;
        }
    }

    private void UpdateTimerBar()
    {
        timer.SetActive(true);
        timerFill.fillAmount = _chargeLevel/fullyChargedBuckle;
    }
    
}
