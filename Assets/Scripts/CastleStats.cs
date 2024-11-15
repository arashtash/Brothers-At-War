using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CastleStats : MonoBehaviour
{
    public int maxHealth = 100; // The maximum health of the castle
    private int currentHealth;  //The current health of the castle

    //Start runs when the script is executed
    void Start()
    {
        // Initialize the castle's health
        currentHealth = maxHealth;
    }

    // Function for applying damage to the castle.
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;    //Subtract the damage from the current castle health
        Debug.Log("Castle Health: " + currentHealth);   //Logging the current health to the console

        // Check if the castle's health reaches zero or bellow
        if (currentHealth <= 0)
        {
            //GAME OVER HANDLING LOGIC GOES HERE
            Debug.Log("Castle has been destroyed!");
        }
    }
}
