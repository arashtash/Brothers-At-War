using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject[] waves;
    public GameObject buildPhaseUI;
    public GameObject spawnPoint;

    private bool inWave = false;
    private int waveCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        buildPhaseUI.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        //when press enter transition to next wave
        if (Input.GetKeyDown(KeyCode.Return) && !inWave)
        {
            buildPhaseUI.SetActive(false);
            spawnWave();
        }

        if (!inWave)
        {
            buildPhase();
        }
    }

    void buildPhase()
    {
        //pop up canvas
        buildPhaseUI.SetActive(true);
    }
    //spawns the game object to start specified wave based on waveCounter
    void spawnWave()
    {
        Instantiate(waves[waveCounter], spawnPoint.transform.position, Quaternion.identity);
        inWave = true;
        Debug.Log("InWave set to true");
        waveCounter++;
    }

    public void waveOver()
    {
        inWave = false;
        Debug.Log("InWave set to false");
    }
}
