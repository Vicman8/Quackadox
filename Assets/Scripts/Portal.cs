using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject nextPortal;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player.gameObject)
        {
            if(player.GetHorizontal() >= 0)
            {
                player.transform.position = new Vector3(nextPortal.transform.position.x + 1.5f, nextPortal.transform.position.y, nextPortal.transform.position.z);
            }
            else
            {
                player.transform.position = new Vector3(nextPortal.transform.position.x - 1.5f, nextPortal.transform.position.y, nextPortal.transform.position.z);
            }
        }
    }
}
