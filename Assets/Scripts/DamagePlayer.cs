using UnityEngine;
using System.Collections;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody2D playerRB;

    //brian's addition for UI testing
    public TEMP_Player_Health Phealth;

    void Start()
    {
        playerRB = player.GetComponent<PlayerMovement>().GetRB();
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject == player)
        {
            Debug.Log("OW!");
            //Rigidbody2D playerRB = player.GetComponent<PlayerMovement>().getRB();
            //playerRB.AddForce(player.transform.right + new Vector3(1000f,0f,0f), ForceMode2D.Impulse);
            player.GetComponent<PlayerMovement>().SetPushed(true);
            float movementDirection = player.GetComponent<PlayerMovement>().GetHorizontal();
            if(movementDirection < 0)
            {
                playerRB.linearVelocity = new Vector3(45f, 0f, 0f);
                Debug.Log("Movement is negative");
                StartCoroutine(ChangeExternalVelocityRight());
            }
            else
            {
                playerRB.linearVelocity = new Vector3(-45f, 0f, 0f);
                Debug.Log("Movement is positive or zero");
                StartCoroutine(ChangeExternalVelocityLeft());
            }

            //Brian's addition for UI testing
            //Phealth.Damaged();
        }
    }

    public IEnumerator ChangeExternalVelocityRight() 
    {
        while(playerRB.linearVelocity.x > 0)
        {
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x - 5f, 0f, 0f);
            Debug.Log("Zeroing velocity right");
            yield return null;
        }

        player.GetComponent<PlayerMovement>().SetPushed(false);

        yield return new WaitForSeconds(1.0f);
    }

    public IEnumerator ChangeExternalVelocityLeft() 
    {
        while (playerRB.linearVelocity.x < 0)
        {
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x + 5f, 0f, 0f);
            Debug.Log("Zeroing velocity left");
            yield return null;
        }

        player.GetComponent<PlayerMovement>().SetPushed(false);

        yield return new WaitForSeconds(1.0f);
    }
}
