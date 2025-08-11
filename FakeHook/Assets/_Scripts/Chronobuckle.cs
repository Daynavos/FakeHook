using UnityEngine;
using UnityEngine.InputSystem;
public class Chronobuckle : MonoBehaviour
{
    public InputActionAsset actionAsset;

    private InputAction SlowTime;
    
    public bool timeIsSlowed;

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
        SlowTime = actionAsset.FindAction("SlowTime");
    }

    void Update()
    {
        if (SlowTime.WasPressedThisFrame())
        {
            timeIsSlowed = true;
        }

        if (SlowTime.WasReleasedThisFrame())
        {
            timeIsSlowed = false;
        }
        
        if (timeIsSlowed)
        {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    private void SlowDownActiveTimer()
    {
        
    }
}
