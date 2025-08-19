using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        private float _timer;
        private die _die;
        public bool gamePaused;
        public int mainMenuSceneIndex = 0;
        public int finalLevelSceneIndex = 2;
        public int timedLevelSceneIndex = 2;
        public InputActionAsset actionAsset;
        private InputAction _pauseAction;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _timer = 0;
            gamePaused = false;
            _die = GameObject.FindGameObjectWithTag("Player").GetComponent<die>();
            _pauseAction = actionAsset.FindAction("Player/Pause");
        }

        private void OnEnable()
        {
            _die.OnPlayerDeath += OnDeath;
            nextLevel.NextLevel += NextLevel;
        }

        public void NextLevel()
        {
            if (SceneManager.GetActiveScene().buildIndex==finalLevelSceneIndex)
            {
                OnWin();
                return;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OnDeath()
        {
            gamePaused = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            Time.timeScale = 0f;
            if (SceneManager.GetActiveScene().buildIndex!=timedLevelSceneIndex) _timer = 0f;
            UIController.Instance.ShowDeathScreen(_timer);
        }
        public void OnWin()
        {
            gamePaused = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            Time.timeScale = 0f;
            UIController.Instance.ShowWinScreen(_timer);
        }

        public void OnMainMenu()
        {
            SceneManager.LoadScene(mainMenuSceneIndex);
        }

        void OnPause()
        {
            gamePaused = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            Time.timeScale = 0f;
            UIController.Instance.ShowPauseScreen();
        }

        public void OnUnpause()
        {
            gamePaused = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            Time.timeScale = 1f;
            UIController.Instance.HidePauseScreen();
        }

        private void OnDisable()
        {
            _die.OnPlayerDeath -= OnDeath;
            nextLevel.NextLevel -= NextLevel;
        }

        private void Update()
        {
            if (_pauseAction.IsPressed())
            {
                OnPause();
            }
            if (gamePaused) return;
            if (SceneManager.GetActiveScene().buildIndex!=timedLevelSceneIndex) return;
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