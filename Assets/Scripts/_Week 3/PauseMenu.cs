using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Complete each section marked TODO.

    [Header("Pause Menu")]
    public GameObject pauseMenu;

    public bool isPaused = false;

    private void Start()
    {
        // Hide the pause menu when the game starts.
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        // Check whether Escape was pressed.
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // TODO: Check if the game is paused.
            // If it is, call ResumeGame().
            if (isPaused == true)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // TODO: Set isPaused to true.
        isPaused = true;

        // TODO: Show the pause menu.
        // Hint: Use the .SetActive() function.
        pauseMenu.SetActive(true);


        // Pause the game.
        Time.timeScale = 0f;

        // TODO: Print "Game Paused" to the Console.
        Debug.Log("Game Paused");


    }

    // Hint: Don't forget to hook this function up to the UI Button.
    public void ResumeGame()
    {
        // TODO: Set isPaused to false.
        isPaused = false;

        // TODO: Hide the pause menu.
        // Hint: Use the .SetActive() function.
        pauseMenu.SetActive(false);


        // Resume the game.
        Time.timeScale = 1f;

        // TODO: Print "Game Resumed" to the Console.
        Debug.Log("Game Resumed");

    }

    public void RestartGame()
    {
        // Make sure time is running again.
        Time.timeScale = 1f;

        // Reload the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}