using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUi;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                Time.timeScale = 1f;
            }
            else
            {
                Pause();
                Time.timeScale = 0f;
            }

        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


    public void Restart()
    {
        SceneManager.LoadScene("1_TutorialLevel");
        Time.timeScale = 1.0f;
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("Langa");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("APP IS QUITTING");
    }
}
