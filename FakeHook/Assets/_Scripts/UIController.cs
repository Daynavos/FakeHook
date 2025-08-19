using System;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance;
        public TextMeshProUGUI timerText;
        public GameObject deathPanel;
        public TextMeshProUGUI finalScoreText;
        public GameObject winPanel;
        public TextMeshProUGUI winText;
        public GameObject pausePanel;
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            timerText.text = "";
            deathPanel.SetActive(false);
            winPanel.SetActive(false);
            pausePanel.SetActive(false);
        }

        public void ShowDeathScreen(float timer)
        {
            deathPanel.SetActive(true);
            finalScoreText.text = "You died.";
            if (timer != 0f)
            {
                finalScoreText.text += "\nYour time was: " + timer;
            }
            else
            {
                finalScoreText.text += ".. on the tutorial...";
            }
        }
        public void ShowPauseScreen()
        {
            pausePanel.SetActive(true);
        }
        public void HidePauseScreen()
        {
            pausePanel.SetActive(false);
        }

        public void ShowWinScreen(float timer)
        {
            winPanel.SetActive(true);
            winText.text = "Congrats!\nYour final time was: " + timer;
        }

        public void UpdateTime(float timer)
        {
            timerText.text = Math.Round(timer, 2)  + "";
        }
        
    }
}