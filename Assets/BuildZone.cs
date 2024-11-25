using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildZone : MonoBehaviour
{
    public GameObject buildPromptUI; // Assign the UI in the Inspector
    public GameObject[] towerPrefabs; // Array of tower prefabs, assign them in the Inspector
    private int[] towerCosts = { 100, 150, 200, 300, 400, 500 }; //Array of tower costs corresponding to each tower prefab
    private string[] towerNames = { "Archer Tower", "Cannon", "Cathedral", "Archer Tower", "Archer Tower", "Archer Tower" };

    public bool playerInRange = false; //Is the player in the tower building range?

    public TextMeshProUGUI currentMoneyText; //Text to display current money in the UI
    public TextMeshProUGUI notEnoughMoneyText; //Text to inform the player that they don't have enough money to build tower

    private EconomyManager economyManager;  //Link to the economy manager module that controls money

    // Start is called before the first frame update
    void Start(){
        //Find the economy manager in the current scene
        economyManager = FindObjectOfType<EconomyManager>();

        //buildPromptUI.SetActive(true);
        buildPromptUI.SetActive(false);

        //Checking for the possible error occuring from mismatching number of towers/tower costs/tower names
        if ((towerCosts.Length != towerPrefabs.Length) && (towerCosts.Length != towerNames.Length))
        {
            Debug.LogError("Number of towers and the number of either costs or names in the arrays don't match."); //Report the error
        }

        UpdateMoneyText(); //Fetch the current player money
    }

    //This method is executed when a collision occurs
    void OnTriggerEnter(Collider other)
    {
        //If the collided item is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = true;   //Set the player in range of the building spot flag to true
            buildPromptUI.SetActive(true);  //Activate the build prompt canvas

            // Close the economy menu if it's open
            if (economyManager != null)
            {
                economyManager.CloseEconomyMenu();  //Close the economy menu
            }

            UpdateMoneyText(); //Fetch the current player money
        }
    }

    //This method is executed when a collision is no longer being detected
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) //Once the player is no longer in range
        {
            playerInRange = false;  //Reset the flag to false
            buildPromptUI.SetActive(false); //Disable the building prompt canvas
            notEnoughMoneyText.text = ""; //Reset not enough money text
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If the player is in range
        if (playerInRange)
        {
            for (int i = 1; i <= towerPrefabs.Length; i++)  //Check for each possible valid number inputs
            {
                if (Input.GetKeyDown(i.ToString())) //If they press that key
                {
                    BuildTower(i - 1); //Build the corresponding tower (i - 1 because arrays start at i = 0)
                    
                }
            }
        }
    }

    //This method handles building the tower and destroying the 
    private void BuildTower(int towerIndex)
    {
        //If the given tower index is not in bound, do not proceed
        if (towerIndex < 0 || towerIndex >= towerPrefabs.Length)
        {
            Debug.LogError("Invalid tower index."); //Report the invalidity problem
            return; //Abort execution of this method
        }

        int towerCost = towerCosts[towerIndex]; //Get the cost of the selected tower

        //Check if the player has enough money, if they do, decrement the money in the economy module
        if (economyManager.DecreasePlayerMoney(towerCost))
        {
            //Create the tower
            Instantiate(towerPrefabs[towerIndex], transform.position, Quaternion.identity);
            //Report the tower creation
            Debug.Log($"Tower {towerIndex + 1} created at {transform.position} for {towerCost} ducats.");

            buildPromptUI.SetActive(false); //hide the build prompt
            Destroy(gameObject);    //destroy the build zone
        }
        else //If the player does not have enough money
        {
            notEnoughMoneyText.text = "Not enough money to build " + towerNames[towerIndex]; //Report to player that they don't have enough money
            Debug.Log("Not enough money to build this tower."); //Report it! Don't do anything
        }

        UpdateMoneyText();  //Update the current money shown in UI
    }

    //This menu fetches the current money from the economy manager and updates the UI text field showing the current money
    private void UpdateMoneyText()
    {
        // Fetch the current money from the EconomyManager and update the text
        currentMoneyText.text = economyManager.GetPlayerMoney() + " ducats";
    }
}