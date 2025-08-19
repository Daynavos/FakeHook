using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        private float _timer;
        private die _die;
        public bool gameOver;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _timer = 0;
            gameOver = false;
            _die = GameObject.FindGameObjectWithTag("Player").GetComponent<die>();
        }

        private void OnEnable()
        {
            _die.OnPlayerDeath += OnDeath;
        }

        public void OnDeath()
        {
            gameOver = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            UIController.Instance.ShowDeathScreen(_timer);
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            _die.OnPlayerDeath -= OnDeath;
        }

        private void Update()
        {
            if (gameOver) return;
            _timer += Time.unscaledDeltaTime;
            UIController.Instance.UpdateTime(_timer);
        }
        void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void OnRestart()
        {
            Restart();
        }
        
    }
}