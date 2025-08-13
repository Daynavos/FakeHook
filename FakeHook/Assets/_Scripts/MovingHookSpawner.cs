using UnityEngine;

public class MovingHookSpawner : MonoBehaviour
{
    public GameObject hookPrefab;
    public float delay;
    
    private float _timer;

    private void SpawnHook()
    {
        _timer += Time.deltaTime;
        Instantiate<GameObject>(hookPrefab, transform.position, transform.rotation);
    }
    
}
