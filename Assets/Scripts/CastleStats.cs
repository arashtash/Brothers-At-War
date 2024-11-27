using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CastleStats : MonoBehaviour
{
    public int maxHealth = 100; // The maximum health of the castle
    private int currentHealth;  //The current health of the castle

    public TextMeshProUGUI healthText;  //Link to the castle health UI text
    public GameObject gameOverCanvas;  //Link to Game Over Canvas

    private bool isGameOver = false;    //Flag to check if the game is over

    //Start runs when the script is executed
    void Start()
    {
        // Initialize the castle's health
        currentHealth = maxHealth;

        //Update the UI with the initial health value
        UpdateHealthText();

        //Hide the Game over canvas when the game begins
        gameOverCanvas.SetActive(false);
    }

    // Function for applying damage to the castle.
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;    //Subtract the damage from the current castle health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //Make sure health doesn't become negative
        Debug.Log("Castle Health: " + currentHealth);   //Logging the current health to the console

        //Update the UI with the new health value
        UpdateHealthText();

        // Check if the castle's health reaches zero or bellow and the game hasn't been set to over yet
        if (currentHealth <= 0 && !isGameOver)
        {
            //Invoke game over mechanism
            GameOver();
        }
    }

    //This method updates the health text on the UI with the current text
    private void UpdateHealthText()
    {
        healthText.text = currentHealth + "%";  //Update text
    }

    //This function handles the game over mechanism
    private void GameOver()
    {
        isGameOver = true; //Set the game over flag to true
        gameOverCanvas.SetActive(true); //Show the Game Over Canvas
        Time.timeScale = 0f; //Freeze the game
    }

    //This method gets executed every frame
    void Update()
    {
        if (isGameOver) //If game is over, wait for instruction to be given as keyboard input
        {
            // Restart level when ENTER is pressed
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isGameOver = false; //Set game over to false again
                gameOverCanvas.SetActive(false);    //Deactivate the game over canvas
                Time.timeScale = 1f; //Unfreeze and resume the time
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reload the level
            }

            //Quit the game when ESC is pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit(); //Quit the game
            }
        }
    }
}
