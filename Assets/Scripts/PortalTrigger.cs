using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    // Reference to the player's script 
    private PlayerMovement playerMovement;

    // Called by PlayerMovement when instantiating portal
    public void SetPlayerReference(PlayerMovement player)
    {
        playerMovement = player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerMovement != null)
        {
            // Call teleport method directly on player script
            playerMovement.TeleportToAlternateWorld();
        }
    }
}