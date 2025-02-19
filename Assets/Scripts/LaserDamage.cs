using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject == player)
        {
            Debug.Log("Hit player");
        }
    }
}
