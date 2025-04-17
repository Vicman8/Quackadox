using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour

{
    public static CheckpointManager Instance { get; private set; }

    [SerializeField] private GameObject player; // Direct reference to player in scene
    [SerializeField] private Transform startPosition; // Default starting position
    [SerializeField] private int maxLives = 3; // Maximum number of lives

    private int currentLives;
    private Vector2 currentCheckpointPosition;
    private bool hasCheckpoint = false;
    private TEMP_Player_Health playerHealth;

    private void Awake()
    {
        // Initialize singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize lives
        currentLives = maxLives;
    }

    private void Start()
    {
        // If player reference is null, try to find it in the scene
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        // Get reference to player health
        if (player != null)
        {
            playerHealth = player.GetComponent<TEMP_Player_Health>();
            player.transform.position = startPosition.position;
        }
        else
        {
            Debug.LogError("No player found! Please assign a player in the inspector or add a Player tag to your player object.");
        }
    }

    // Call this method when a new checkpoint is activated
    public void SetCheckpoint(Vector2 position)
    {
        currentCheckpointPosition = position;
        hasCheckpoint = true;
        Debug.Log("Checkpoint set at: " + position);
    }

    // Call this method when the player dies - called from Player Health script
    public void PlayerDied()
    {
        currentLives--;
        Debug.Log("Player died. Lives remaining: " + currentLives);

        if (currentLives <= 0)
        {
            // No more lives, reset the level
            ResetLevel();
        }
        else
        {
            // Respawn at checkpoint if available, otherwise at start position
            if (hasCheckpoint)
            {
                RespawnPlayerAtCheckpoint();
            }
            else
            {
                player.transform.position = startPosition.position;
                ResetPlayerState();
            }
        }
    }

    // Respawn player at last checkpoint
    public void RespawnPlayerAtCheckpoint()
    {
        if (player != null)
        {
            player.transform.position = currentCheckpointPosition;
            // Reset player state (health, etc.)
            ResetPlayerState();
            Debug.Log("Player respawned at checkpoint: " + currentCheckpointPosition);
        }
        else
        {
            Debug.LogError("Player is null in RespawnPlayerAtCheckpoint!");
        }
    }

    // Reset player's state after respawning
    private void ResetPlayerState()
    {
        // Reset health
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }

        // Reset movement if needed
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetPushed(false);
            // Reset any other player-specific state here
        }
    }

    // Reset the entire level
    private void ResetLevel()
    {
        Debug.Log("Resetting level. Lives reset to: " + maxLives);

        // Reset lives
        currentLives = maxLives;

        // Reset checkpoint
        hasCheckpoint = false;

        // Reset all checkpoints in the scene
        Checkpoint[] checkpoints = Object.FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.Reset();
        }

        // Move player to start
        if (player != null)
        {
            player.transform.position = startPosition.position;
            ResetPlayerState();
        }
    }

    // For updating UI elements (hearts display)
    public int GetCurrentLives()
    {
        return currentLives;
    }

    // For getting the maximum lives (useful for UI)
    public int GetMaxLives()
    {
        return maxLives;
    }
}