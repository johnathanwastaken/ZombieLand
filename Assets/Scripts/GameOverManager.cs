using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI; // Assign the Game Over UI Panel in the Inspector

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Hide UI at the start
        }
    }

    public void ShowGameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // Show Game Over screen
            Cursor.lockState = CursorLockMode.None; // Unlock the mouse
            Cursor.visible = true;
        }
        else
        {
            Debug.LogError("GameOverUI is not assigned in the inspector!");
        }
    }

    public void RestartGame()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor when restarting
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart current scene
    }

    public void ReturnToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu"); // Load main menu scene
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!");

        // Stop all sounds
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Stop();
        }

        // Disable player movement
        FPSInput playerMovement = FindObjectOfType<FPSInput>();
        if (playerMovement != null) playerMovement.enabled = false;

        // Disable shooting
        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
        if (weaponManager != null) weaponManager.enabled = false;

        // Freeze game time to prevent continued actions
        Time.timeScale = 0;

        // Quit the game (only works in a built executable)
        Application.Quit();

        // For testing inside Unity Play Mode
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}
