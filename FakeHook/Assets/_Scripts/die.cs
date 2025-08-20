using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class die : MonoBehaviour
{
    public GameObject audioManager;
    private AudioManager audManscript;
    public event Action OnPlayerDeath;

    void Start()
    {
        audManscript = audioManager.GetComponent<AudioManager>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("beam"))
        {
            audManscript.PlaySFX(audManscript.hitObstacle);
            OnPlayerDeath?.Invoke();
        }
    }

    
}
