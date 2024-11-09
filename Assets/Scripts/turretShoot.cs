using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    public float shootSpeed = 3f;           // Time between shots
    public float shootCooldownLeft = 3f;    // Time left on cooldown
    public GameObject bulletPrefab;         // Bullet prefab
    public float bulletSpeed = 10f;         // Bullet speed
    private GameObject target;              // The current target (enemy)
    private List<GameObject> enemies;       // List of enemies in range

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        shootCooldownLeft -= Time.deltaTime;

        // Shoot a bullet if cooldown is over and there are enemies in range
        if (shootCooldownLeft <= 0 && enemies.Count > 0)
        {
            ShootBullet();
            shootCooldownLeft = shootSpeed;  // Reset cooldown
        }

        // Assign the first enemy in range as the target
        if (enemies.Count > 0)
        {
            target = enemies[0];
        }

        // Remove null or dead enemies from the list
        if (enemies.Count > 0 && enemies[0] == null)
        {
            enemies.RemoveAt(0);
        }
    }

    // Add enemy to the list when it enters the turret's trigger range
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goblin"))
        {
            enemies.Add(other.gameObject);
        }
    }

    // Remove the enemy from the list when it exits the trigger range
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Goblin"))
        {
            enemies.Remove(other.gameObject);
        }
    }

    // Shoot a bullet at the current target
    void ShootBullet()
    {
        // Instantiate bullet at the turret's position
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        bullet.transform.parent = this.transform;

        // Add logic to move the bullet towards the target
        Bullet bulletScript = bullet.AddComponent<Bullet>();
        bulletScript.SetTarget(target);
        bulletScript.SetSpeed(bulletSpeed);
    }

    // Bullet script that makes the bullet move towards the target
    public class Bullet : MonoBehaviour
    {
        private GameObject target;
        private float speed;

        // Set the target for the bullet
        public void SetTarget(GameObject newTarget)
        {
            target = newTarget;
        }

        // Set the speed of the bullet
        public void SetSpeed(float bulletSpeed)
        {
            speed = bulletSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                // Move the bullet towards the target
                Vector3 direction = target.transform.position - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

                // If the bullet reaches the target, destroy it and call TakeDamage
                if (transform.position == target.transform.position)
                {
                    // Call the TakeDamage function on the target (enemy)
                    enemyHealth healthScript = target.GetComponent<enemyHealth>();
                    if (healthScript != null)
                    {
                        healthScript.TakeDamage(10);  // Pass the damage amount (e.g., 10)
                    }

                    // Destroy the bullet after hitting the target
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);  // Destroy bullet if target is lost
            }
        }

        // When the bullet collides with an enemy, apply damage
        void OnCollisionEnter(Collision collision)
        {
            // Check if the collision is with an enemy
            if (collision.gameObject.CompareTag("Goblin"))
            {
                // Get the enemyHealth script and call TakeDamage
                enemyHealth healthScript = collision.gameObject.GetComponent<enemyHealth>();
                if (healthScript != null)
                {
                    healthScript.TakeDamage(10);  // Apply damage, adjust the value as needed
                }

                // Destroy the bullet after dealing damage
                Destroy(gameObject);
            }
        }
    }
}
