using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public float damageMult = 1.0f;

    [SerializeField] FloatingHealthBar healthBar;

    private void Awake()
    {
        // Ensures healthBar is found as a child object
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        // Ensure healthBar is not null before trying to update it
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(health, maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        // Apply the damage multiplier after subtracting the damage
        health -= damage * damageMult;

        // Ensure health doesn't go below 0
        health = Mathf.Max(health, 0f);

        // Update health bar if the health bar exists
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(health, maxHealth);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Optionally, you could add some delay here for animations or effects
        Destroy(gameObject);
    }
}
