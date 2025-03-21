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
            Debug.Log("The duckkkk");
        }
        else if (!Detect)
        {
            countDown = 4;
            //enemy.Guard();
            enemy.Guarding = true;
            Debug.Log("huh, where is duck");
        }

        if (countDown <= 0)
        {
            Detect = false;
        }


        //countDown = countDown - 1 * Time.deltaTime;
        Debug.Log($"{countDown}");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.Guarding = false;
            Detect = true;
            //Debug.Log("THE DUCK");
        }
    }
}
