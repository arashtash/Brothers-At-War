using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public GameObject[] towers; // Array to hold tower prefabs
    private BuildZone currentBuildZone;

    void Update()
    {
        if (currentBuildZone != null && Input.anyKeyDown)
        {
            for (int i = 1; i <= towers.Length; i++)
            {
                if (Input.GetKeyDown(i.ToString())) // Check number keys 1-6
                {
                    BuildTower(i - 1);
                    break;
                }
            }
        }
    }

    private void BuildTower(int index)
    {
        if (currentBuildZone != null && index >= 0 && index < towers.Length)
        {
            Instantiate(towers[index], currentBuildZone.transform.position, Quaternion.identity);
            currentBuildZone = null; // Clear the zone to prevent duplicate builds
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BuildZone>())
        {
            currentBuildZone = other.GetComponent<BuildZone>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BuildZone>())
        {
            currentBuildZone = null;
        }
    }
}
