using UnityEngine;

public class Enemy_Detection : MonoBehaviour
{
    public Enemy enemy;

    public float countDown = 6;
    public bool Detect;

    public void Update()
    {
        if (Detect)
        {
            countDown = countDown - 1 * Time.deltaTime;
            //Debug.Log("The duckkkk");
        }
        else if (!Detect)
        {
            countDown = 4;
            enemy.GuardState = 0;
            //Debug.Log("huh, where is duck");
        }

        if (countDown <= 0)
        {
            Detect = false;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //enemy.Guarding = false;
            enemy.GuardState = 1;
            Detect = true;
            //Debug.Log("THE DUCK");
        }
    }
}
