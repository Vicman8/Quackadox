using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Controls : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameUI");
    }

    public void QuitGame()
    {
        Debug.Log("You left the game :(");
    }
}
