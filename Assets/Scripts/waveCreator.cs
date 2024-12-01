using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveCreator : MonoBehaviour
{
    public Vector3 spawnPoint;
    public GameObject[] enemies;
    public int spawnOffset = 0;

    private int offset;
    private int enemyCounter = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private GameManager gameManager;

    void Start()
    {
        offset = spawnOffset;
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    void Update()
    {
        offset--;
        if (offset <= 0 && enemyCounter < enemies.Length)
        {
            offset = spawnOffset;
            generateEnemy();
        }

        //check if all spawned enemies are destroyed
        if (enemyCounter == (enemies.Length) && AllEnemiesDestroyed())
        {
            gameManager.WaveOver();
            Destroy(gameObject);
        }
    }

    private void generateEnemy()
    {
        GameObject enemy = Instantiate(enemies[enemyCounter], spawnPoint, Quaternion.identity);
        spawnedEnemies.Add(enemy);
        enemyCounter++;
    }

    private bool AllEnemiesDestroyed()
    {
        //Remove any null references from the list (destroyed enemies)
        spawnedEnemies.RemoveAll(enemy => enemy == null);
        //if the list is empty, all enemies are destroyed
        Debug.Log(spawnedEnemies.Count);
        return spawnedEnemies.Count == 0;

       
    }
}
