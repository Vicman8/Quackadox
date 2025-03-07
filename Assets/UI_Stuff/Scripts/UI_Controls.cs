using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Controls : MonoBehaviour
{
    public GameObject PauseMenu;
    private Boolean IsPaused;
    public float pauseState = 0;
    public void Start()
    {
        Continue();
    }

    public void Update()
    {
        //Will check is the game is paused or not. if its not, it will pause. 
        if (Input.GetKeyUp(KeyCode.Escape) && !IsPaused)
        {
            Paused();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && IsPaused)
        {
            Continue();
        }
    }
    public void Paused()
    {
        pauseState = 1;
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        IsPaused = true;
    }

    public void Continue()
    {
        pauseState = 0;
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        IsPaused = false;
    }

    public void Menu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
