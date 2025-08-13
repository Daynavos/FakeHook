using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Chronobuckle : MonoBehaviour
{
    public InputActionAsset actionAsset;

    private InputAction _slowTime;
    
    private enum BucklePhase
    {
        FullyCharged,
        Depleting,
        Charging
    }

    [SerializeField] private BucklePhase phaseOfBuckle;
    
    [SerializeField] private float fullyChargedBuckle;

    //public GameObject volume;
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
        _chargeLevel = Mathf.Clamp(_chargeLevel, 0f, fullyChargedBuckle);
        _chargeLevel = fullyChargedBuckle;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _slowTime = actionAsset.FindAction("Player/SlowTime");
        phaseOfBuckle = BucklePhase.FullyCharged;
    }

    void Update()
    {
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
           // volume.SetActive(false);
            _spriteRenderer.color = Color.red;
            Time.timeScale = 1f;
            timer.SetActive(false);
        }
        
        if (phaseOfBuckle == BucklePhase.Depleting)
        {
           // volume.SetActive(true);
            _spriteRenderer.color = Color.green;
            Time.timeScale = 0.5f;
            DepletingTimer();
            UpdateTimerBar();
        }
        
        if (phaseOfBuckle == BucklePhase.Charging)
        {
           // volume.SetActive(false);
            _spriteRenderer.color = Color.blue;
            Time.timeScale = 1f;
            ChargingTimer();
            UpdateTimerBar();
        }
    }

    private void DepletingTimer()
    {
        UpdateTimerBar();
        _chargeLevel -= Time.deltaTime;
        if (_chargeLevel == 0)
        {
            phaseOfBuckle = BucklePhase.Charging;
        }
    }

    private void ChargingTimer()
    {
        UpdateTimerBar();
        _chargeLevel += Time.deltaTime;
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
