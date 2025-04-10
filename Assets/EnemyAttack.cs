using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public TEMP_Player_Health Phealth;
    public GameObject HitBox;




    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("OW!");
            //Rigidbody2D playerrb = player.GetComponent<PlayerMovement>().getrb();
           // playerrb.AddForce(player.transform.right + new Vector3(1000f, 0f, 0f), ForceMode2D.Impulse);

            //Brian's addition for UI testing
            Phealth.Damaged();
        }
    }
}
