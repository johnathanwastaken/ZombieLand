using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public int currentRound = 1;
    public int enemiesPerRound = 5;
    public float spawnInterval = 3f;
    public float roundStartDelay = 5f;

    public Vector3 spawnAreaSize = new Vector3(20, 1, 20); // Adjust as needed

    public Text zombieCounterText; // UI element for tracking zombies
    public Transform player; // Reference to the player's position
    public float minSpawnDistance = 8f; // Minimum spawn distance from player

    public Text roundText; // Reference to UI displaying the round
    public GameObject enemyPrefab;
    public GameObject bossPrefab; // Assign this in the Inspector

    public Transform[] spawnPoints; // Array of spawn locations

    public float roundCooldown = 5f; // Cooldown between rounds

    private int enemiesRemaining;
    private bool roundActive = false;

    void Start()
    {
        UpdateRoundUI();
        StartCoroutine(StartNextRound());
    }

    IEnumerator StartNextRound()
    {
        roundActive = false;

        // Clear all remaining enemies before the next round
        yield return new WaitForEndOfFrame();
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in remainingEnemies)
        {
            Destroy(enemy);
        }

        // Display countdown before the round starts
        for (int i = 5; i > 0; i--)
        {
            if (roundText != null)
            {
                roundText.text = $"Round {currentRound} starting in {i}...";
            }
            yield return new WaitForSeconds(1);
        }

        // Ensure UI updates after countdown finishes
        if (roundText != null)
        {
            roundText.text = $"Round: {currentRound}";
        }

        Debug.Log($"Starting Round {currentRound}");
        roundActive = true;

        // Check if it's a boss round (every 5 rounds)
        if (currentRound % 5 == 0)
        {
            SpawnBossZombie();
        }
        else
        {
            enemiesRemaining = enemiesPerRound;
            UpdateZombieCounter(); //Update UI before spawning enemies

            for (int i = 0; i < enemiesPerRound; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            UpdateZombieCounter(); //Ensure counter updates after all enemies are spawned
        }
    }

    void SpawnEnemy()
    {
        if (enemiesRemaining <= 0) return; // Stop spawning if max is reached

        Vector3 spawnPosition;
        int maxAttempts = 10;
        int attempts = 0;

        // Ensure enemies spawn far enough from the player
        do
        {
            spawnPosition = GetRandomSpawnPosition();
            attempts++;
        }
        while (Vector3.Distance(spawnPosition, player.position) < minSpawnDistance && attempts < maxAttempts);

        // Spawn the enemy and ensure it has the "Enemy" tag
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.tag = "Enemy"; // Ensure the spawned enemy is tagged

        // Apply difficulty scaling
        WanderingAI enemyAI = enemy.GetComponent<WanderingAI>();
        if (enemyAI != null)
        {
            enemyAI.IncreaseDifficulty(currentRound);
        }

        // Ensure enemy death is tracked correctly
        ReactiveTarget target = enemy.GetComponent<ReactiveTarget>();
        if (target != null)
        {
            target.OnEnemyDeath += EnemyDefeated;
        }
    }

    void SpawnBossZombie()
    {
        Vector3 spawnPosition;
        int maxAttempts = 10;
        int attempts = 0;

        // Ensure boss spawns far from the player
        do
        {
            spawnPosition = GetRandomSpawnPosition();
            attempts++;
        }
        while (Vector3.Distance(spawnPosition, player.position) < minSpawnDistance && attempts < maxAttempts);

        // Spawn the boss
        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        boss.tag = "Boss"; // Ensure the boss is tagged correctly

        // Track the boss as the only remaining enemy
        enemiesRemaining = 1;
        UpdateZombieCounter(); //Ensure the UI updates when the boss spawns

        // Ensure boss death is tracked
        boss.GetComponent<ReactiveTarget>().OnEnemyDeath += EnemyDefeated;

        Debug.Log("Boss Zombie Spawned!");
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomZ = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        return new Vector3(randomX, 1, randomZ); // Keep them above the ground
    }

    public void EnemyDefeated()
    {
        if (enemiesRemaining > 0) // Prevent counter from going negative
        {
            enemiesRemaining--;
            UpdateZombieCounter();
        }

        Debug.Log($"Enemy defeated! Zombies remaining: {enemiesRemaining}");

        if (enemiesRemaining <= 0 && roundActive)
        {
            roundActive = false;
            currentRound++;
            enemiesPerRound += 2; // Increase enemy count per round
            spawnInterval = Mathf.Max(1f, spawnInterval - 0.2f); // Reduce spawn time for faster rounds
            StartCoroutine(StartNextRound());
        }
    }

    void UpdateZombieCounter()
    {
        if (zombieCounterText != null)
        {
            zombieCounterText.text = $"Zombies Left: {enemiesRemaining}";
        }
        Debug.Log($"Updated Zombie Counter: {enemiesRemaining}");
    }

    void UpdateRoundUI()
    {
        if (roundText != null)
        {
            roundText.text = $"Round: {currentRound}";
        }
    }
}
