using UnityEngine;
using UnityEngine.Serialization;

public class Laser : MonoBehaviour
{
    public GameObject beamOn;
    public GameObject beamOff;
    
    private Chronobuckle.BucklePhase _phase;
    void Start()
    {
        _phase = GameObject.FindGameObjectWithTag("Player").GetComponent<Chronobuckle>().phaseOfBuckle;
    }

    void Update()
    {
        _phase = GameObject.FindGameObjectWithTag("Player").GetComponent<Chronobuckle>().phaseOfBuckle;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            
        }
        if (_phase == Chronobuckle.BucklePhase.Depleting)
        {
            LaserOff();
        }
        else
        {
            LaserOn();
        }
    }

    private void LaserOn()
    {
        beamOn.SetActive(true);
        beamOff.SetActive(false);
    }

    private void LaserOff()
    {
        beamOn.SetActive(false);
        beamOff.SetActive(true);
    }
}
