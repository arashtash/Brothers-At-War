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

    void Start()
    {
        offset = spawnOffset;
    }

    void Update()
    {
        offset--;
        if (offset <= 0 && enemyCounter < enemies.Length)
        {
            offset = spawnOffset;
            generateEnemy();
        }
    }

    private void generateEnemy()
    {
        Instantiate(enemies[enemyCounter], spawnPoint, Quaternion.identity);
        enemyCounter++;
    }
}
