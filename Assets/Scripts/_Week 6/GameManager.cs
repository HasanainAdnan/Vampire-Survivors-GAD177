using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Complete each section marked TODO.

    [Header("Screens")]
    public GameObject inGameScreen;
    public GameObject gameOverScreen;

    [Header("Game Over Text")]
    public TextMeshProUGUI killText;
    public TextMeshProUGUI coinsText;

    [Header("Settings")]
    public bool pauseOnDeath = true;

    [Header("Spawner Tracking")]
    public int spawnerCount = 0;

    [Header("Debug")]
    public bool enableDebugKeys = true;
    public bool showDebugLogs = true;

    public Key addSpawnerKey = Key.Equals;
    public Key removeSpawnerKey = Key.Minus;
    public Key gameOverKey = Key.G;

    private bool gameIsOver = false;

    private void Start()
    {
        // Make sure the game is running at normal speed.
        Time.timeScale = 1f;

        // TODO: Call the function that hides the Game Over screen.
        ShowGameOverScreen(false);

        // TODO: Call the function that shows the In Game screen.
        ShowInGameScreen(true);

        // TODO: If debug logs are enabled, print the starting spawner count.
        if (showDebugLogs == true)
        {
            Debug.Log("GameManager: Starting spawner count is " + spawnerCount);
        }
    }

    private void Update()
    {
        HandleDebugInput();
    }

    private void HandleDebugInput()
    {
        if (enableDebugKeys == false)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current[addSpawnerKey].wasPressedThisFrame)
        {
            // TODO: Call the function that adds a spawner.
            AddSpawner();
        }

        if (Keyboard.current[removeSpawnerKey].wasPressedThisFrame)
        {
            // TODO: Call the function that removes a spawner.
            RemoveSpawner();
        }

        if (Keyboard.current[gameOverKey].wasPressedThisFrame)
        {
            // TODO: Call the GameOver function.
            GameOver();
        }
    }

    public void GameOver()
    {
        // TODO: Check whether the game is already over.
        // If it is, stop this function.
        if (gameIsOver == true)
        {
            return;
        }

        // TODO: Set gameIsOver to true.
        gameIsOver = true;

        // TODO: Hide the In Game screen.
        ShowInGameScreen(false);

        // TODO: Show the Game Over screen.
        ShowGameOverScreen(true);

        // TODO: Check whether pauseOnDeath is true.
        // If it is, pause the game by setting Time.timeScale to 0.
        if (pauseOnDeath == true)
        {
            Time.timeScale = 0f;
        }

        // TODO: If debug logs are enabled, print "GameManager: Game Over.".
        if (showDebugLogs == true)
        {
            Debug.Log("GameManager: Game Over.");
        }
    }

    public void ShowGameOverScreen(bool enabled)
    {
        // TODO: Check that gameOverScreen is not null.
        // If it exists, use SetActive and pass in enabled.
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(enabled);
        }
    }

    public void ShowInGameScreen(bool enabled)
    {
        // TODO: Check that inGameScreen is not null.
        // If it exists, use SetActive and pass in enabled.
        if (inGameScreen != null)
        {
            inGameScreen.SetActive(enabled);
        }
    }

    public void AddSpawner()
    {
        if (gameIsOver == true)
        {
            return;
        }

        // TODO: Increase spawnerCount by 1.
        spawnerCount++;

        // TODO: If debug logs are enabled, print the current spawner count.
        if (showDebugLogs == true)
        {
            Debug.Log("GameManager: Spawner count is " + spawnerCount);
        }
    }

    public void RemoveSpawner()
    {
        if (gameIsOver == true)
        {
            return;
        }

        // TODO: Decrease spawnerCount by 1.
        spawnerCount--;

        // TODO: Check whether spawnerCount is below 0.
        // If it is, set spawnerCount back to 0.
        if (spawnerCount < 0)
        {
            spawnerCount = 0;
        }

        // TODO: If debug logs are enabled, print the current spawner count.
        if (showDebugLogs == true)
        {
            Debug.Log("GameManager: Spawner count is " + spawnerCount);
        }

        // TODO: Check whether spawnerCount is 0 or less.
        // If it is, call GameOver.
        if (spawnerCount <= 0)
        {
            GameOver();
        }
    }

    // Hook this function up to the Retry UI Button.
    public void Retry()
    {
        // Make sure the game is running again.
        Time.timeScale = 1f;

        // Reload the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Hook this function up to the Main Menu UI Button.
    public void ReturnToStartMenu()
    {
        // TODO: Make sure the game is running again.
        Time.timeScale = 1f;

        // TODO: Load Scene 0.
        // Use the Retry function above as an example.
        SceneManager.LoadScene(0);
    }
}
