using UnityEngine;
using UnityEngine.SceneManagement;
public class ResetLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.name); // Debug line
        if (other.CompareTag("Player"))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
