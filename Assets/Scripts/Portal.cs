using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject nextPortal;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            player.transform.position = new Vector3(nextPortal.transform.position.x + 1.5f, nextPortal.transform.position.y, nextPortal.transform.position.z);
        }
    }
}
