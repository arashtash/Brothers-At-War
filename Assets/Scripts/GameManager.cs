using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] waves;
    public GameObject spawnPoint;

    private int waveCounter = 0;
    private bool inWave = false;
    private MasterGameManager masterGameManager;



    public void SetMasterGameManager(MasterGameManager master)
    {
        masterGameManager = master;
    }

    public void SpawnWave()
    {
        if (waveCounter < waves.Length)
        {
            Instantiate(waves[waveCounter], spawnPoint.transform.position, Quaternion.identity);
            inWave = true;
            waveCounter++;
        }
        else
        {
            masterGameManager.OnLevelComplete();
        }
    }

    public void WaveOver()
    {
        inWave = false;

        // Notify the MasterGameManager when this GameManager's wave is done
        masterGameManager.OnWaveOver();
    }


    public bool HasMoreWaves()
    {
        return waveCounter < waves.Length;
    }

}
