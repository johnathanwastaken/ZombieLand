using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("ZombieLand"); // Replace with your actual game scene name
    }

    public void QuitGame()
    {
        Application.Quit(); // Quits the game
        Debug.Log("Game Quit!"); // Only works in a built game
    }
}
