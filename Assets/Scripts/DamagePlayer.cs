using UnityEngine;
using System.Collections;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public Rigidbody2D playerRB;
    [SerializeField] private bool shouldPushPlayer;

    //brian's addition for UI testing
    public TEMP_Player_Health Phealth;

    private bool playerDamaged;
    [SerializeField] private float damageCooldown;
    private float damageCountdown;

    void Start()
    {
        playerRB = player.GetComponent<PlayerMovement>().GetRB();
        playerDamaged = false;
        damageCountdown = 0;
    }

    void Update()
    {
        if(damageCountdown > 0)
        {
            damageCountdown -= Time.deltaTime;
        }
        
        if(damageCountdown <= 0)
        {
            playerDamaged = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject);
        if(collider.gameObject == player)
        {
            Debug.Log("OW!");
            //Rigidbody2D playerRB = player.GetComponent<PlayerMovement>().getRB();
            //playerRB.AddForce(player.transform.right + new Vector3(1000f,0f,0f), ForceMode2D.Impulse);

            if(shouldPushPlayer)
            {
                player.GetComponent<PlayerMovement>().SetPushed(true);
                float movementDirection = player.GetComponent<PlayerMovement>().GetHorizontal();
                if (movementDirection < 0)
                {
                    playerRB.linearVelocity = new Vector3(20f, 0f, 0f);
                    Debug.Log("Movement is negative");
                    Debug.Log(playerRB.linearVelocity);
                    StartCoroutine(ChangeExternalVelocityRight());
                }
                else
                {
                    playerRB.linearVelocity = new Vector3(-20f, 0f, 0f);
                    Debug.Log(playerRB.linearVelocity);
                    Debug.Log("Movement is positive or zero");
                    StartCoroutine(ChangeExternalVelocityLeft());
                }
            }

            //Brian's addition for UI testing
            if(!playerDamaged)
            {
                Phealth.Damaged();
                Debug.Log("Take damage");
                playerDamaged = true;
                damageCountdown = damageCooldown;
            }
        }
    }

    public IEnumerator ChangeExternalVelocityRight() 
    {
        while(playerRB.linearVelocity.x > 0)
        {
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x - 2f, 0f, 0f);
            Debug.Log("Zeroing velocity right");
            Debug.Log(playerRB.linearVelocity);
            yield return null;
        }

        player.GetComponent<PlayerMovement>().SetPushed(false);

        yield return new WaitForSeconds(1.0f);
    }

    public IEnumerator ChangeExternalVelocityLeft() 
    {
        while (playerRB.linearVelocity.x < 0)
        {
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x + 2f, 0f, 0f);
            Debug.Log("Zeroing velocity left");
            Debug.Log(playerRB.linearVelocity);
            yield return null;
        }

        player.GetComponent<PlayerMovement>().SetPushed(false);

        yield return new WaitForSeconds(1.0f);
    }
}
