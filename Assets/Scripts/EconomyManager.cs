using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    public GameObject economyMenuCanvas;    //Link to the economy upgrades menu canvas
    public int playerMoney = 100; //Starting amount of money
    public int population = 100; //Starting population (equal to base income)
    public int baseIncome = 100; //Base income of the player

    public int miningLevel = 0; //Current level of mining
    public int miningUpgradeCost = 50; //Mining upgrade cost
    public int miningIncomeIncrease = 30; //How much each mining upgrade adds to the base income

    public int agricultureLevel = 0; //Current level of agriculture sector
    public int agricultureUpgradeCost = 50; //Agriculture upgrade cost

    public int tradeLevel = 0; //Current level of trade sector
    public int tradeUpgradeCost = 50; //Trade upgrade cost
    //This is a fixed amount (grows by constantly by level increase). It is added after each wave before growth rate is applied
    public int tradeFixedPopulationIncrease = 10;


    public Slider miningLevelSlider;//Link to the mining level slider
    public Slider agricultureLevelSlider; //Link to the agriculture level slider
    public Slider tradeLevelSlider; //Link to the trade level slider

    public TextMeshProUGUI populationGrowthText; //Text for population growth rate
    public TextMeshProUGUI fixedPopulationIncreaseText; //Text for fixed population increase
    public TextMeshProUGUI miningIncomeText; //Text for mining income

    public TextMeshProUGUI miningUpgradeCostText;   //Link to text for upgrading mining to the next level
    public TextMeshProUGUI agricultureUpgradeCostText;  //Link to text for upgrading agriculture to the next level
    public TextMeshProUGUI tradeUpgradeCostText; //Link to text for upgrading trade to the next level

    private int[] agricultureAndTradeCosts = { 50, 100, 150, 200, 275, 350, 450, 600 }; //Array of cost of trade and agriculture upgrades per level
    private int[] miningCosts = { 25, 40, 60, 90, 130, 180, 240, 310 }; //Array of cost of mining upgrades per level

    public TextMeshProUGUI playerMoneyText; //Text that shows player money
    public TextMeshProUGUI populationText; // Text that shows the population above the canvas


    private bool isBuildPhase = true; //Are we in the build phase?

    // Start is called before the first frame update
    void Start()
    {
        baseIncome = population; //Base income starts as the population
        UpdateUI(); //Update the UI to get the initial values
        economyMenuCanvas.SetActive(false); //Economy upgrade menu is initially disabled
    }

    // Update is called once per frame
    void Update()
    {
        //If we are in the build phase, when the player presses B, activate the economy menu/or deactivate if it's already active
        if (isBuildPhase && Input.GetKeyDown(KeyCode.B))
        {
            ToggleEconomyMenu();    //Toggle the economy menu (Show it if it's off, hide it if it's on)
        }

        //Check for upgrade inputs only if the economy menu is active
        if (economyMenuCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) //If Key 1 is pressed when in the economy menu try to upgrade Mining
            {
                UpgradeMining();    //Invoke Upgrade Mining to try to upgrade mining by one level
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) //If Key 2 is pressed when in the economy menu try to upgrade Agriculture
            {
                UpgradeAgriculture();    //Invoke Upgrade Agriculture to try to upgrade agriculture by one level
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) //If Key 3 is pressed when in the economy menu try to upgrade Trade
            {
                UpgradeTrade();    //Invoke Upgrade Trade to try to upgrade trade by one level
            }
        }
    }

    //This method toggles the economy menu. If it's on, it'll hide it, if not it will show it.
    //This method also checks if the player is in a tower building zone and prevents opening the economy menu
    //in a build zone
    public void ToggleEconomyMenu()
    {
        //Prevent the economy menu from opening if the player is in a build zone
        BuildZone[] buildZones = FindObjectsOfType<BuildZone>(); //Find all build zones
        foreach (var buildZone in buildZones)   //Check for all buildzones
        {
            if (buildZone.playerInRange)    //If the player is in that build zone
            {
                Debug.Log("Cannot open the economy menu while in a build zone.");   //Report to console
                return; //Don't allow economy menu to be opened
            }
        }

        //Toggle the activeness and visibility of the economy menu to the opposite of what it is right now
        economyMenuCanvas.SetActive(!economyMenuCanvas.activeSelf); 
    }

    //This method Closes the economy menu
    public void CloseEconomyMenu()
    {
        economyMenuCanvas.SetActive(false); //Close the economy menu
    }

    //This method handles upgrading the mining sector one level (cost depends on level)
    public void UpgradeMining()
    {
        //If the player has enough money for the mining upgrade (level dependent)
        if (miningLevel < miningCosts.Length && playerMoney >= miningCosts[miningLevel])
        {
            playerMoney -= miningCosts[miningLevel];   //Deduct upgrade cost for that level from the player's current money
            miningLevel++;  //Increase the level of the mining sector one level
            miningLevelSlider.value = miningLevel;  //Update Mining level slider
            UpdateUI(); //Update the UI after applying changes
        }
    }

    //This method handles upgrading the agriculture sector one level (cost depends on level)
    public void UpgradeAgriculture()
    {
        //If the player has enough money for the agriculture upgrade (level dependent)
        if (agricultureLevel < agricultureAndTradeCosts.Length && playerMoney >= agricultureAndTradeCosts[agricultureLevel])
        {
            playerMoney -= agricultureAndTradeCosts[agricultureLevel]; //Deduct upgrade cost for that level from the player's current money
            agricultureLevel++; //Increase the level of the agriculture sector one level
            agricultureLevelSlider.value = agricultureLevel; //Update agriculture level slider
            UpdateUI(); //Update the UI after applying changes
        }
    }

    //This method handles upgrading the trade sector one level (cost depends on level)
    public void UpgradeTrade()
    {
        //If the player has enough money for the trade upgrade (level dependent)
        if (tradeLevel < agricultureAndTradeCosts.Length && playerMoney >= agricultureAndTradeCosts[tradeLevel])
        {
            playerMoney -= agricultureAndTradeCosts[tradeLevel]; //Deduct Upgrade cost for that level from the player's current money
            tradeLevel++;   //Increase the level of the trade sector one level
            tradeLevelSlider.value = tradeLevel;  //Update Trade level slider
            UpdateUI();     //Update the UI after applying changes
        }
    }

    //This method calculates the player's money after each wave
    public void HandleMoneyAfterWave()
    {
        //Population defines the base income for after the next wave. It is calculated as follows based on the upgrades:
        //New Population = (Current population + Trade Bonus) * (1.01 + Agriculture Bonus), where:
        //Trade Bonus = tradeLevel * tradeFixedPopulationIncrease AND Agriculture Bonus = agricultureLevel * 0.02
        // Calculate population growth after the upcoming wave
        int populationAfterTradeBonus = population + (tradeLevel * tradeFixedPopulationIncrease);   //Calculating trade bonus addition
        int populationGrowthPercentage = 1 + (2 * agricultureLevel); //Population growth is 1% base + 2% per agriculture level
        int newPopulation = Mathf.FloorToInt(population * (1 + (populationGrowthPercentage / 100f)));   //Calculating new population based on bonuses

        
        population = newPopulation; // Update population and base income
        baseIncome = population;    //Set the base income to the current population

        // Calculate income at the end of the wave (Adding mining income to the income from the population)
        playerMoney += baseIncome + (miningLevel * miningIncomeIncrease);
        UpdateUI(); //Call this method to update the info on the Economy Canvas
    }

    //This method updates the UI by updating the levels and the current money of the player on the Economy Canvas
    private void UpdateUI()
    {
        //Update the player's current money and population stats
        playerMoneyText.text = playerMoney.ToString() + " ducats";
        populationText.text = population.ToString();
        miningLevelSlider.value = miningLevel;  //Set the mining level
        agricultureLevelSlider.value = agricultureLevel; //Set the agriculture level
        tradeLevelSlider.value = tradeLevel; //Set the trade level

        int populationGrowthPercentage = 1 + (2 * agricultureLevel); //Next wave's population growth rate
        int fixedPopulationIncrease = tradeLevel * tradeFixedPopulationIncrease; //Next wave's fixed population growth
        int miningIncome = miningLevel * miningIncomeIncrease;  //Next wave's income from mining

        populationGrowthText.text = "Population growth rate:\n" + populationGrowthPercentage + "%";   //Calculating relative growth
        fixedPopulationIncreaseText.text = "Fixed Population increase:\n" + fixedPopulationIncrease; //Calculating fixed growth
        miningIncomeText.text = "Mining income:\n" + miningIncome + " ducats";   //Calculating mining income

        miningUpgradeCostText.text = GetUpgradeCostText(miningLevel, miningCosts);  //Update the cost for next mining level
        agricultureUpgradeCostText.text = GetUpgradeCostText(agricultureLevel, agricultureAndTradeCosts); //Update the cost for next agriculture level
        tradeUpgradeCostText.text = GetUpgradeCostText(tradeLevel, agricultureAndTradeCosts); //Update the cost for the next trade level
    }

    // Helper function to get upgrade cost text for the Update UI function
    private string GetUpgradeCostText(int level, int[] costs)
    {
        if (level < costs.Length)   //If there is any more levels left
        {
            return "Upgrade cost: " + costs[level] + " ducats"; //Return cost label
        }
        else //If max level reached
        {
            return "Max level reached"; //Return report text
        }
    }

    //This method can be called to set whether it is currently the build phase right now or not.
    public void SetBuildPhase(bool isBuildPhaseRightNow)
    {
        isBuildPhase = isBuildPhaseRightNow;    //Set the isBuildPhase flag to the given value
    }

    //This menu subtracts a cost from the player money (e.g., for buying something) and returns true if successful, false if not enough money is available
    public bool DecreasePlayerMoney (int amount)
    {

        if (playerMoney >=amount) //If the player has enough money for the purchase
        {
            playerMoney -= amount;  //Deduct the cost from the player's money
            UpdateUI(); //Update UI to show the new money value
            return true; //Successfully deducted the amount
        }

        return false; //Player does not have enough money so return false
    }



    //This method return the current player money for use in other modules
    public int GetPlayerMoney()
    {
        return playerMoney; //return the current amount of money the player has
    }
}
