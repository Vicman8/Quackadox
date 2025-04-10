using UnityEngine;

public class UnlockBehavior : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collided");
        if(collider.gameObject == player.gameObject)
        {
            player.SetDashUnlocked(true);
            Debug.Log("unlocked dash!");
        }
    }
}
