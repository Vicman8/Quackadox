using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TEMP_Player_Health : MonoBehaviour
{
    //Variables
    public int health = 5; //the amount of health the player currently has
    public int maxHealth = 5; //the max amount of health the player can have
    public Image[] T_Health; //an array that contains the health bars
    //UI health bar placement
    public Image Heart1;
    public Image Heart2;
    public Image Heart3;
    public Image Heart4;
    public Image Heart5;
    //The two sprites of the UI health bar
    public Sprite dHealth; //Damaged health/eaten bread
    public Sprite hHealth; //Full health/Full bread


    //private CheckpointManager checkpointManager;
    private bool isRespawning = false;

    //public GameObject Death_Screen;


    private CheckpointManager checkpointManager;
    //private bool isRespawning = false;

    public void Start()
    {
        //Death_Screen.SetActive(false);
        //Creating the health bar
        T_Health = new Image[maxHealth];
        T_Health[0] = Heart1;
        T_Health[1] = Heart2;
        T_Health[2] = Heart3;
        T_Health[3] = Heart4;
        T_Health[4] = Heart5;


        //checkpointManager = CheckpointManager.Instance;

        // Find the CheckpointManager
        checkpointManager = CheckpointManager.Instance;

    }

    public void Update()
    {
        if (health <= 0 && !isRespawning)
        {
            isRespawning = true;
            //Death_Screen.SetActive(true);
            Debug.Log("You dead");


            // Start death sequence
            //StartCoroutine(HandleDeath());

            //SceneManager.LoadScene("Level1", LoadSceneMode.Single);

            // Start death sequence
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        // Short delay before respawning
        yield return new WaitForSeconds(1.0f);

        // Notify checkpoint manager
        if (checkpointManager != null)
        {
            checkpointManager.PlayerDied();
        }
        else
        {
            Debug.LogError("CheckpointManager not found!");
            // Fallback reset if no checkpoint manager
            ResetHealth();
            isRespawning = false;

        }
    }
    /*
    private IEnumerator HandleDeath()
    {
        // Short delay before respawning
        yield return new WaitForSeconds(1.0f);

        // Notify checkpoint manager
        if (checkpointManager != null)
        {
            checkpointManager.PlayerDied();
        }
        else
        {
            Debug.LogError("CheckpointManager not found!");
            // Fallback reset if no checkpoint manager
            ResetHealth();
            isRespawning = false;
        }
    }
    */

    public void Damaged() //This function controls the player losing a health
    {
        if (health > 0)
        {
            T_Health[health - 1].sprite = dHealth;
            health -= 1;
            Debug.Log("Health: " + health);
        }
    }

    public void Healed() //This function allows the player to gain a health
    {
        if (health < maxHealth)
        {
            T_Health[health].sprite = hHealth;
            health += 1;
            Debug.Log("Health: " + health);
        }
    }


    // Called by CheckpointManager when respawning player

    public void ResetHealth()
    {
        health = maxHealth;

        // Reset all heart sprites
        for (int i = 0; i < maxHealth; i++)
        {
            T_Health[i].sprite = hHealth;
        }

        isRespawning = false;
        Debug.Log("Health reset to " + health);
    }
}
