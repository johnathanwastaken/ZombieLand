using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // What prefab to spawn
    [SerializeField] GameObject enemyPrefab;

    // Maximum number of enemies at once
    // [SerializeField] int maxEnemies = 5;

    // Spawn area boundaries
    [SerializeField] float spawnAreaMinX = -20f;
    [SerializeField] float spawnAreaMaxX = 20f;
    [SerializeField] float spawnAreaMinZ = -20f;
    [SerializeField] float spawnAreaMaxZ = 20f;
    [SerializeField] float spawnHeight = 1f;

    // Time between enemy spawns
    // [SerializeField] float spawnInterval = 3f;

    private List<GameObject> enemies = new List<GameObject>();

    // Start is called before the first frame update
    // void Start()
    // {
    //     StartCoroutine(SpawnEnemies());
    // }

    // Coroutine to spawn enemies over time
    // private IEnumerator SpawnEnemies()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(spawnInterval);

    //         // Continuously spawn until reaching maxEnemies
    //         while (enemies.Count < maxEnemies)
    //         {
    //             Vector3 spawnPosition = GetRandomSpawnPosition();
    //             GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    //             float angle = Random.Range(0, 360);
    //             enemy.transform.Rotate(0, angle, 0);

    //             enemies.Add(enemy);

    //             // Subscribe to enemy death event
    //             ReactiveTarget target = enemy.GetComponent<ReactiveTarget>();
    //             if (target != null)
    //             {
    //                 target.OnEnemyDeath += () => RemoveEnemy(enemy);
    //             }
    //         }
    //     }
    // }

    // Generate a random position within the defined area
    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float z = Random.Range(spawnAreaMinZ, spawnAreaMaxZ);
        return new Vector3(x, spawnHeight, z);
    }

    // Removes enemy from the list after it dies
    private void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}
