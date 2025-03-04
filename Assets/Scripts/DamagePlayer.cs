using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    //brian's addition for UI testing
    public TEMP_Player_Health Phealth;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject == player)
        {
            Debug.Log("OW!");
            Rigidbody2D playerRB = player.GetComponent<PlayerMovement>().getRB();
            playerRB.AddForce(player.transform.right + new Vector3(1000f,0f,0f), ForceMode2D.Impulse);

            //Brian's addition for UI testing
            //Phealth.Damaged();
        }
    }
}
