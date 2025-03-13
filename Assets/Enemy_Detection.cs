using UnityEngine;

public class Enemy_Detection : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("THE DUCK");
        }
    }
}
