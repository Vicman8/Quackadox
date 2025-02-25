using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    [SerializeField] private GameObject player;

    //brian's addition for UI testing
    public TEMP_Player_Health Phealth;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject == player)
        {
            Debug.Log("OW!");

            //Brian's addition for UI testing
            Phealth.Damaged();
        }
    }
}
