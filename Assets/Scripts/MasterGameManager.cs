using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterGameManager : MonoBehaviour
{
    public GameManager[] gameManagers; //array of GameManager instances (one per lane of enemies)
    public EconomyManager economyManager; //reference to economy manager
    public GameObject buildPhaseUI; // Global build phase UI
    public GameObject levelTransitionCanvas; //level transition canvas
    public GameObject gameCompleteCanvas; //Game complete Canvas

    private int activeGameManagers; //Active game mangers
    private int completedGameManagers;
    private bool isLevelTransitioning = false;
    private bool isGameComplete = false;
    private bool inWave = false;

    void Start()
    {
        activeGameManagers = gameManagers.Length;
        completedGameManagers = 0;

        buildPhaseUI.SetActive(false);
        levelTransitionCanvas.SetActive(false);
        gameCompleteCanvas.SetActive(false);

        foreach (GameManager gm in gameManagers)
        {
            gm.SetMasterGameManager(this);
        }

        StartBuildPhase();
    }

    void StartBuildPhase()
    {
        buildPhaseUI.SetActive(true);
    }


    void Update()
    {
        if (isGameComplete)
        {
            HandleGameCompleteInput();
            return;
        }

        if (isLevelTransitioning)
        {
            HandleLevelTransitionInput();
        }

        if (!inWave && Input.GetKeyDown(KeyCode.Return))
        {
            OnWaveStart(); //start the wave when Enter is pressed
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //Press Escape to quit the game
        {
            QuitGame();
        }
    }

    public void OnWaveStart()
    {
        buildPhaseUI.SetActive(false);
        foreach (GameManager gm in gameManagers)
        {
            gm.SpawnWave();
        }

        inWave = true;
        Debug.Log("wave started");
    }

    public void OnWaveOver()
    {
        completedGameManagers++;

        //all GameManagers finished the wave
        if (completedGameManagers == activeGameManagers)
        {
            completedGameManagers = 0;
            inWave = false;

            //check if any GameManager still has remaining waves
            bool hasMoreWaves = false;

            foreach (GameManager gm in gameManagers)
            {
                if (gm.HasMoreWaves()) //add a method in GameManager to check this
                {
                    hasMoreWaves = true;
                    break;
                }
            }

            if (hasMoreWaves)
            {
                //call EconomyManager's money handling logic
                if (economyManager != null)
                {
                    economyManager.HandleMoneyAfterWave();
                }

                StartBuildPhase();
            }
            else
            {
                //if no more waves, go directly to level/game complete
                OnLevelComplete();
            }
        }
    }

    public void OnLevelComplete()
    {
        if (isGameComplete || isLevelTransitioning) return;

        isLevelTransitioning = true;

        // Check if the current level is the last
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            ShowGameCompleteCanvas();
        }
        else
        {
            ShowLevelTransitionCanvas();
        }
    }

    void ShowLevelTransitionCanvas()
    {
        levelTransitionCanvas.SetActive(true);
        Debug.Log("Level Transition: Waiting for player input.");
    }

    void ShowGameCompleteCanvas()
    {
        gameCompleteCanvas.SetActive(true);
        isGameComplete = true;
        Debug.Log("Game Complete: Congratulations!");
    }

    void HandleLevelTransitionInput()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) //Press Escape to quit the game
        {
            QuitGame();
        }
    }

    void HandleGameCompleteInput()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }


    void LoadNextLevel()
    {
        levelTransitionCanvas.SetActive(false);
        isLevelTransitioning= false;

        string currentSceneName= SceneManager.GetActiveScene().name;
        string nextLevelName= GetNextLevelName(currentSceneName);

        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Debug.Log("Loading next level: " + nextLevelName);
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.LogError("No next level found!");
        }
    }

    string GetNextLevelName(string currentLevel)
    {
        if (currentLevel == "Level1") return "Level2";
        if (currentLevel == "Level2") return "Level3";
        return ""; // No more levels
    }

    public void QuitGame()
    {
        Debug.Log("exiting game");
        Application.Quit();
    }
}
