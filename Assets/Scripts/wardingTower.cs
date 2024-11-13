using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wardingTower : MonoBehaviour
{
    public float damageMultiplier = 2.0f; // The multiplier to apply to the enemies' damage

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is a Goblin
        if (other.CompareTag("Goblin"))
        {
            // Get the enemyHealth component from the Goblin
            enemyHealth healthScript = other.GetComponent<enemyHealth>();

            // Apply the damage multiplier to the enemy's damage
            healthScript.damageMult *= damageMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is a Goblin
        if (other.CompareTag("Goblin"))
        {
            // Get the enemyHealth component from the Goblin
            enemyHealth healthScript = other.GetComponent<enemyHealth>();

            // Reset the damage multiplier to its original value
            healthScript.damageMult /= damageMultiplier;
        }
    }
}
