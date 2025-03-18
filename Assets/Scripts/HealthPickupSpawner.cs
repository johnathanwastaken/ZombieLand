using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthPickupSpawner : MonoBehaviour
{
    public GameObject healthPickupPrefab; // Reference to the health pickup prefab
    public int maxPickups = 3; // Limit on how many can exist at once
    public float spawnInterval = 10f; // Time between spawn attempts
    public Vector3 spawnAreaSize = new Vector3(10, 1, 10); // Size of the spawn area

    private List<GameObject> activePickups = new List<GameObject>(); // Tracks active pickups

    void Start()
    {
        StartCoroutine(SpawnHealthPickups());
    }

    IEnumerator SpawnHealthPickups()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Remove null objects (destroyed pickups)
            activePickups.RemoveAll(item => item == null);

            if (activePickups.Count < maxPickups)
            {
                Vector3 randomPosition = GetRandomSpawnPosition();
                GameObject newPickup = Instantiate(healthPickupPrefab, randomPosition, Quaternion.identity);
                activePickups.Add(newPickup);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomZ = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        return new Vector3(randomX, spawnAreaSize.y, randomZ);
    }
}
