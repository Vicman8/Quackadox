using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject nextPortal;
    [SerializeField] private string nextScene;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player.gameObject)
        {
            if(player.GetHorizontal() >= 0 && nextPortal != null)
            {
                player.transform.position = new Vector3(nextPortal.transform.position.x + 5f, nextPortal.transform.position.y, nextPortal.transform.position.z);
            }
            else if(nextPortal != null)
            {
                player.transform.position = new Vector3(nextPortal.transform.position.x - 5f, nextPortal.transform.position.y, nextPortal.transform.position.z);
            }
            else if(nextScene != null)
            {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
        }
    }
}
