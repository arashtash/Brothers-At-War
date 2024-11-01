using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public int maxHealth = 50; // Max health of the tower
    private int currentHealth; //Current health of the tower

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the tower's health to full health
        currentHealth = maxHealth;
    }

    // Function to take damage for the towers
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;    //Subtract damage taken from current health
        Debug.Log("Tower Health:" + currentHealth); //print out tower health to the console

        // Check if the tower's health reaches zero or below
        if (currentHealth <= 0)
        {
            // Destroy the tower
            Destroy(gameObject);
        }
    }

    //Detecting collisions with enemies
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided is an enemy
        if (other.CompareTag("Enemy"))
        {
            // Defining the damage. This could be different with different enemies
            int damageAmount = 10;

            // Take damage
            TakeDamage(damageAmount);
        }
    }
}
