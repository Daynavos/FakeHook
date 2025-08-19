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
        public 

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            deathPanel.SetActive(false);
        }

        public void ShowDeathScreen(float timer)
        {
            deathPanel.SetActive(true);
            finalScoreText.text = "Your time was: " + timer;
        }

        public void UpdateTime(float timer)
        {
            timerText.text = Math.Round(timer, 2)  + "";
        }
        
    }
}