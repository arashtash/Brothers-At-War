using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject[] waves;  //This array holds the waves
    public GameObject buildPhaseUI;
    public GameObject spawnPoint;
    public EconomyManager economyManager;   //economy manager script
    public GameObject levelTransitionCanvas; //The level complete canvas
    public GameObject gameCompleteCanvas;   //The game complete canvas

    private bool inWave = false;    //Flag to check if we are currently in a wave
    private int waveCounter = 0; //The iterator for waves
    private bool isLevelTransitioning = false; //Flag to check if the level is transitioning
    private bool isGameComplete = false; // Flag to check if the game is complete


    // Start is called before the first frame update
    void Start()
    {
        buildPhaseUI.SetActive(false);
        levelTransitionCanvas.SetActive(false);
        gameCompleteCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //when press enter transition to next wave
        if (Input.GetKeyDown(KeyCode.Return) && !inWave)
        {
            buildPhaseUI.SetActive(false);
            spawnWave();
        }

        if (isGameComplete)
        {
            HandleGameCompleteInput();
            return;
        }

        //If level is transitioning
        if (isLevelTransitioning)
        {
            HandleLevelTransitionInput();   //Call this method to check if the level is transitioning
            return;
        }

        if (!inWave)
        {
            buildPhase();
            
        }
    }

    void buildPhase()
    {
        //pop up canvas
        buildPhaseUI.SetActive(true);
    }
    //spawns the game object to start specified wave based on waveCounter
    void spawnWave()
    {
        if (waveCounter < waves.Length) //If there are more waves
        {
            Instantiate(waves[waveCounter], spawnPoint.transform.position, Quaternion.identity);
            inWave = true;
            waveCounter++;
        }
        else
        {
            //Check if it's the last level to decide next action
            if (SceneManager.GetActiveScene().name == "Level3") //If it's currently level 3
            {
                ShowGameCompleteCanvas();   //Show game complete canvas
            }
            else //If it's not yet level 3
            {
                ShowLevelTransitionCanvas(); //Show level transition canvas
            }
        }
    }

    public void waveOver()
    {
        inWave = false;
        Debug.Log("InWave set to false");
        economyManager.HandleMoneyAfterWave();
    }




    //This method handles showing the level transition canvas
    void ShowLevelTransitionCanvas()
    {
        Time.timeScale = 0;
        levelTransitionCanvas.SetActive(true);  //Show level transition canvas
        isLevelTransitioning = true;    //Set the flag
    }

    //Show the game complete canvas
    void ShowGameCompleteCanvas()
    {
        Time.timeScale = 0;
        gameCompleteCanvas.SetActive(true); //Show game complete canvas
        isGameComplete = true; // Set the flag
    }


    //This method handles inputs for level transitioning or quiting the game
    void HandleLevelTransitionInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))   //If they press ENTER
        {
            LoadNextLevel ();    //Go to the next level
        }
        else if  (Input.GetKeyDown(KeyCode.Escape))  //If they press ESC
        {
            QuitGame ();    //Quit the game
        }
    }

    //This method handles the inputs when the game is complete
    void HandleGameCompleteInput()
    {
        //If the player presses ENTER or ESC
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame(); //Exit the game
        }
    }

    //This method handles loading the next level
    void LoadNextLevel()
    {
        levelTransitionCanvas.SetActive(false); //Deactivate level complete canvas
        isLevelTransitioning = false;   //Set the flag to false
        Time.timeScale = 1;

        string currentSceneName = SceneManager.GetActiveScene().name;   //Get the name of the current scene
        string nextLevelName = GetNextLevelName(currentSceneName);//pass it to the GetNextLevelName method

        if (!string.IsNullOrEmpty(nextLevelName))   //If level exists
        {
            SceneManager.LoadScene(nextLevelName); //load it
        }
        else //If no more level exists
        {
            Debug.Log("No next level to load!");    //Report it
        }
    }

    //This method handles getting the next level
    string GetNextLevelName(string currentLevel)
    {
        if (currentLevel == "Level1") return "Level2";  //If currently on level 1 return level 2
        if (currentLevel == "Level2") return "Level3";//If currently on level 2 return level 3
        return ""; // No more levels, return emptyu string
    }

    //This method handles quitting the game
    void QuitGame()
    {
        Application.Quit(); //Quit the game
    }
}
