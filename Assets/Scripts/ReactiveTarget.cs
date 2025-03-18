using System.Collections;
using UnityEngine;
using System; // Required for Action

public class ReactiveTarget : MonoBehaviour
{
    public event Action OnEnemyDeath; // Event to notify RoundManager when enemy dies
    private int health = 3; // Default enemy health, scales in later rounds
    private bool isDead = false; // Prevents multiple calls to `EnemyDefeated()`

    void Start()
    {
        if (gameObject.CompareTag("Boss"))
        {
            health = 20; // Boss has more health
        }
        else
        {
            health = 3; // Normal zombie health
        }
    }

    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        health -= damage; // Subtract damage from health

        if (health <= 0 && !isDead) // Only trigger death if health reaches zero & enemy isn't already dead
        {
            isDead = true;
            StartCoroutine(Die()); // Start death animation
        }
    }

    // Called when enemy is hit by player
    public void ReactToHit()
    {
        TakeDamage(1); // Default player attack deals 1 damage
    }

    // Death animation coroutine
    private IEnumerator Die()
    {
        // Award points when enemy dies
        if (gameObject.CompareTag("Boss"))
        {
            GameManager.Instance.AddScore(100); // Boss gives 100 points
        }
        else
        {
            GameManager.Instance.AddScore(10); // Regular zombies give 10 points
        }

        // Ensure `EnemyDefeated()` is only called once
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath.Invoke();
            OnEnemyDeath = null; // Prevent multiple calls
        }

        // Disable movement to prevent further actions
        WanderingAI ai = GetComponent<WanderingAI>();
        if (ai != null)
        {
            ai.enabled = false; // Disable AI script
        }

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = true; // Stop movement
            agent.enabled = false; // Disable NavMeshAgent
        }

        // Rotate enemy to simulate falling down
        this.transform.Rotate(-75, 0, 0);

        // Wait before removing the enemy
        yield return new WaitForSeconds(1.5f);

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }
}
