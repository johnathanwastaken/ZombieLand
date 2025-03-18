using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Required for pathfinding

public class WanderingAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3.5f; // Speed at which the enemy moves
    public float acceleration = 8f; // Controls how quickly the enemy reaches full speed
    public float stoppingDistance = 1.2f; // Distance at which the enemy stops moving toward the player

    [Header("Detection Settings")]
    public float detectionRange = 15.0f; // Increased range so zombies see the player sooner
    public float attackRange = 1.5f; // Distance at which the enemy will attack the player
    public float attackCooldown = 1.0f; // Time between attacks
    public int damage = 1; // Damage dealt to the player per attack

    private bool isAlive = true; // Tracks whether the enemy is alive
    private Transform player; // Reference to the player's transform
    private NavMeshAgent agent; // Reference to the NavMeshAgent component for movement
    private float nextAttackTime = 0f; // Timer to track when the next attack is allowed

    [Header("Sound Settings")]
    private AudioSource audioSource;
    public AudioClip zombieGrowl;
    public AudioClip attackSound;

    private static float lastGrowlTime = 0f; // Global cooldown for zombie growls
    private static float growlCooldown = 5f; // Time before another zombie can growl

    private void Start()
    {
        isAlive = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = speed;
            agent.acceleration = acceleration;
            agent.stoppingDistance = stoppingDistance;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(TryGrowl());
    }

    private void Update()
    {
        if (!isAlive || player == null || agent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            agent.SetDestination(player.position);
        }

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void AttackPlayer()
    {
        // Check if the player is still active in the game
        PlayerCharacter playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerCharacter>();

        // Stop attack if the player is already dead
        if (playerHealth == null || playerHealth.IsGameOver()) 
        {
            return;
        }

        // Deal damage to the player
        playerHealth.TakeDamage(damage);

        // Play attack sound if conditions are met
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    private IEnumerator TryGrowl()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(Random.Range(3, 8)); // Random delay before checking

            // Only allow a growl if enough time has passed since the last one
            if (Time.time - lastGrowlTime >= growlCooldown)
            {
                lastGrowlTime = Time.time; // Update the last growl time
                if (audioSource != null && zombieGrowl != null)
                {
                    audioSource.PlayOneShot(zombieGrowl);
                }
            }
        }
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;

        if (!alive && agent != null)
        {
            agent.isStopped = true;
        }
    }

    public void IncreaseDifficulty(int round)
    {
        speed += round * 0.2f;
        Debug.Log($"Enemy difficulty increased for round {round}. New Speed: {speed}");
    }

   public void StopSounds()
    {
        if (audioSource != null)
        {
            Debug.Log("Stopping enemy sounds...");
            audioSource.Stop();
            audioSource.enabled = false;
        }
    }

}
