using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    public int maxHealth = 10; // Maximum player health
    private int currentHealth;
    public Slider healthBar; // Reference to the Health Bar UI Slider
    public GameObject gameOverUI; // Reference to the Game Over UI
    private bool isGameOver = false;

    [Header("Sound Effects")]
    private AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip healSound;
    public AudioClip deathSound;

    void Start()
    {
        currentHealth = maxHealth; // Set player health to max
        UpdateHealthBar(); // Initialize the health bar UI

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Hide Game Over screen at start
        }

        // Setup Audio Source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Method for taking damage
    public void TakeDamage(int damage)
    {
        if (isGameOver) return; // Prevent further damage if game over
        
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        UpdateHealthBar(); // Update the UI when taking damage

        // Play damage sound
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0)
        {
            GameOver(); // Trigger Game Over if health is zero
        }
    }

    // Method for healing
    public void Heal(int healAmount)
    {
        if (isGameOver) return; // Don't heal if game is over
        
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        UpdateHealthBar();

        // Play heal sound
        if (audioSource != null && healSound != null)
        {
            audioSource.PlayOneShot(healSound);
        }

        Debug.Log($"Player healed! Current Health: {currentHealth}");
    }

    // Update the Health Bar UI
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth; // Set max value dynamically
            healthBar.value = currentHealth; // Update current health
        }
    }

    // Handle Game Over logic
    private void GameOver()
    {
        if (isGameOver) return; // Prevent multiple calls

        isGameOver = true;
        Debug.Log("Game Over Triggered! GameOver() called.");

        // Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Stop all enemy sounds when the player dies
        WanderingAI[] enemies = FindObjectsOfType<WanderingAI>();
        foreach (WanderingAI enemy in enemies)
        {
            enemy.StopSounds();
        }

        // Show Game Over UI
        if (gameOverUI != null)
        {
            Debug.Log("Game Over UI should now be visible!");
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogError("Game Over UI is NULL! Make sure it is assigned in the Inspector.");
        }

        // Unlock the mouse for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable player movement
        DisablePlayer();
    }



    public bool IsGameOver()
    {
        return isGameOver;
    }


    // Disable Player Movement and Mouse Look
    private void DisablePlayer()
    {
        // Disable player movement script
        FPSInput playerMovement = GetComponent<FPSInput>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Disable Mouse Look on the Player (horizontal movement)
        MouseLook playerMouseLook = GetComponent<MouseLook>();
        if (playerMouseLook != null)
        {
            playerMouseLook.enabled = false;
        }

        // Disable Mouse Look on the Camera (vertical movement)
        MouseLook cameraMouseLook = Camera.main.GetComponent<MouseLook>();
        if (cameraMouseLook != null)
        {
            cameraMouseLook.enabled = false;
        }
    }

    // Restart the Game (Can be called from UI Button)
    public void RestartGame()
    {
        // Lock cursor again when restarting
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable Mouse Look on the Player
        MouseLook playerMouseLook = GetComponent<MouseLook>();
        if (playerMouseLook != null)
        {
            playerMouseLook.enabled = true;
        }

        // Re-enable Mouse Look on the Camera
        MouseLook cameraMouseLook = Camera.main.GetComponent<MouseLook>();
        if (cameraMouseLook != null)
        {
            cameraMouseLook.enabled = true;
        }

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
