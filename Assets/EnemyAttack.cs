using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public TEMP_Player_Health Phealth;
    public GameObject HitBox;


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            Flip();
        }
    }

    public void Flip()
    {
        Vector3 hi = HitBox.transform.localPosition;
        hi.x += 1f;
        Debug.Log(hi.x);
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("OW!");
            Rigidbody2D playerRB = player.GetComponent<PlayerMovement>().getRB();
            playerRB.AddForce(player.transform.right + new Vector3(1000f, 0f, 0f), ForceMode2D.Impulse);

            //Brian's addition for UI testing
            Phealth.Damaged();
        }
    }
}
