using UnityEngine;

public class TEMP_Heals : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public TEMP_Player_Health Phealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player && Phealth.health < 5)
        {
            Debug.Log("healed :)");
            Phealth.Healed();
        }
    }
}
