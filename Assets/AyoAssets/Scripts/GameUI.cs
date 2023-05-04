using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using systems;

public class GameUI : MonoBehaviour
{
    public enum GameState { MainMenu, Systems, Paused, Playing, GameOver };
    public GameState currentState;
    public TextMeshProUGUI healthText;
    public GameObject allGameUI, mainMenuPanel, systemsPanel, pauseMenuPanel, gameOverPanel, titleText;

    // KH
    [SerializeField] Text scoreDisplay;

    // Start is called before the first frame update
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            CheckGameState(GameState.MainMenu);
        }
        else if (SceneManager.GetActiveScene().name == "Systems")
        {
            CheckGameState(GameState.Systems);
        }
        else 
        {
            CheckGameState(GameState.Playing);
        }
    }

    public void CheckGameState(GameState newGameState)
    {
        currentState = newGameState;
        switch (currentState)
        {
            case GameState.MainMenu:
                MainMenuSetup();
                break;
            case GameState.Systems:
                SystemsSetup();
                Debug.Log("Systems");
                break;
            case GameState.Paused:
                GamePaused();
                systems.GameManager.gamePaused = true;
                Time.timeScale = 0f;
                break;
            case GameState.Playing:
                GameActive();
                Time.timeScale = 1f;
                systems.GameManager.gamePaused = false;
                break;
            case GameState.GameOver:
                GameOver();
                Time.timeScale = 0f;
                systems.GameManager.gamePaused = true;
                break;
        }

    }

    public void MainMenuSetup()
    {
        allGameUI.SetActive(false);
        mainMenuPanel.SetActive(true);
        systemsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        titleText.SetActive(true);
    }

    public void SystemsSetup()
    {
        allGameUI.SetActive(false);
        mainMenuPanel.SetActive(false);
        systemsPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        titleText.SetActive(true);
    }

    public void GameActive()
    {
        allGameUI.SetActive(true);
        mainMenuPanel.SetActive(false);
        systemsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        titleText.SetActive(false);
    }

    public void GamePaused()
    {
        allGameUI.SetActive(true);
        mainMenuPanel.SetActive(false);
        systemsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        titleText.SetActive(true);
    }

    public void GameOver()
    {
        allGameUI.SetActive(false);
        mainMenuPanel.SetActive(false);
        systemsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        titleText.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        checkInputs();
    }

    void checkInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("ESC");
            if (currentState == GameState.Playing)
            {
                CheckGameState(GameState.Paused);
            }
            else if (currentState == GameState.Paused)
            {
                CheckGameState(GameState.Playing);
            }
        }
    }

    public void StartGame()
    {
        // KH - Reset statistics from the previous session before starting a new one.
        GameManager.control.ResetSessionStatistics();

        // Ayo
        SceneManager.LoadScene("Game");
        CheckGameState(GameState.Playing);
    }

    public void PauseGame()
    {
        CheckGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        CheckGameState(GameState.Playing);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        CheckGameState(GameState.MainMenu);
    }

    public void GoToSystems()
    {
        SceneManager.LoadScene("Systems");
        CheckGameState(GameState.MainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateHealth()
    {
        healthText.text = systems.GameManager.health.ToString();
    }

    // KH
    public void UpdateScore()
    {
        scoreDisplay.text = GameManager.control.GetScore().ToString();
    }
}
