using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class die : MonoBehaviour
{
    public event Action OnPlayerDeath;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("beam"))
        {
            OnPlayerDeath?.Invoke();
        }
    }

    
}
