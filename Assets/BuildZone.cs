using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildZone : MonoBehaviour
{
    public GameObject buildPromptUI; // Assign the UI in the Inspector
    public GameObject[] towerPrefabs; // Array of tower prefabs, assign them in the Inspector

    private bool playerInRange = false;


    void Start(){
        //buildPromptUI.SetActive(true);
        buildPromptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            buildPromptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            buildPromptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange)
        {
            for (int i = 1; i <= towerPrefabs.Length; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    CreateTower(i - 1); // Subtract 1 to match the array index
                    buildPromptUI.SetActive(false);
                    Destroy(gameObject); // Destroy the build zone after tower creation
                    
                }
            }
        }
    }

    private void CreateTower(int towerIndex)
    {
        Instantiate(towerPrefabs[towerIndex], transform.position, Quaternion.identity);
        Debug.Log($"Tower {towerIndex + 1} created at {transform.position}");
    }
}