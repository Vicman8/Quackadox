using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject activatedVisual;
    [SerializeField] private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
        {
            // Activate this checkpoint
            isActivated = true;

            // Show activated visual if available
            if (activatedVisual != null)
            {
                activatedVisual.SetActive(true);
            }

            // Notify the checkpoint manager
            CheckpointManager.Instance.SetCheckpoint(transform.position);

            Debug.Log("Checkpoint activated at: " + transform.position);
        }
    }

    // Call this method to reset the checkpoint when player loses all lives
    public void Reset()
    {
        isActivated = false;

        // Hide activated visual if available
        if (activatedVisual != null)
        {
            activatedVisual.SetActive(false);
        }
    }
}
