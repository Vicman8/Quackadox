using System;
using UnityEngine;

public class Enemy_Detection : MonoBehaviour
{
    //grabing the enemy script
    public Enemy enemy;
    //count down for detection
    private float countDown;
    public bool Detect;


    public void Start()
    {
        countDown = 0f;
        Detect = false;
    }

    public void Update()
    {
        //this will change the status of the enemy when the player has been seen
        //Debug.Log("countDown: " + countDown);
        
        //this is when the player lost the player. it will start to count down until the enemy has to go back to it position
        if (!Detect && countDown > 0)
        {
            countDown = countDown - (1 * Time.deltaTime);
            if (countDown <= 0)
            {
                Debug.Log("Where?");
                enemy.GuardState = 2;
            }
        }
        else if (Detect)
        {
            countDown = 6f;
        }
     
        
    }

    //if the player is in the circle trigger, the enemy will start to follow the player
    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy found the duck");
            //enemy.Guarding = false;
            enemy.GuardState = 1;
            Detect = true;
 
            //Debug.Log("THE DUCK");
        }
    }

    //if the player leaves that circle, there will be a countdown. when it reaches 0, the enemy will go back to its original place
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Detect = false;
            Debug.Log("Enemy lost the duck");
        }
    }
}
