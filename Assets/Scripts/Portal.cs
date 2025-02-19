using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject player;
   /*private GameObject[] forwardPortals;
    private GameObject[] backwardPortals;*/
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*forwardPortals = GameObject.FindGameObjectsWithTag("Forward");
        backwardPortals = GameObject.FindGameObjectsWithTag("Backward");*/
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider == player)
        {
            //player.transform.position = new Vector3()
        }
    }
}
