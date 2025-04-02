using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    public GameObject ReturnPoint;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;

    public Transform PlayerPoint;

    public bool Guarding;
    public bool AtGuardPoint;

    public float GuardState = 0; 
    //0 = guarding, 1 = following player, 2 = returning to post
    

    //public EnemyAttack EnemyA;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = PointB.transform;
        Guarding = true;
        AtGuardPoint = true;

    }

    // Update is called once per frame
    public void Update()
    {
        Debug.Log(GuardState);
        
        if (GuardState == 0)
        {
            Guard();
        } 
        else if (GuardState == 1)
        {
            FollowPlayer();
        }
        else if (GuardState == 2)
        {
            Return();
        }
        /*
        if (Guarding && AtGuardPoint)
        {
            Guard();
        }
        else if (!Guarding && !AtGuardPoint)
        {
            Return();
        } 
        else if (!Guarding && AtGuardPoint)
        {
            FollowPlayer();
        }
        */
    }

    private void FollowPlayer()
    {
        
        Vector3 direction = (PlayerPoint.position - transform.position).normalized;
        direction.y = 0f; // Lock movement on Y-axis

        // Apply velocity with X-axis movement only
        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, 0f);
        if(direction.x > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = 1.25f;
            transform.localScale = localScale;
        } 
        else if(direction.x < 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -1.25f;
            transform.localScale = localScale;
        }
        Debug.Log(direction.x);
    }

    public void Guard()
    {
        //Debug.Log("guarding");
        //Guarding = true;
        //Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == PointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
            Vector3 localScale = transform.localScale;
            localScale.x = 1.25f;
            transform.localScale = localScale;
            //Flip();
            //EnemyA.Flip();

        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
            Vector3 localScale = transform.localScale;
            localScale.x = -1.25f;
            transform.localScale = localScale;
            //EnemyA.Flip();
            //Flip();
        }


        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointB.transform)
        {
            currentPoint = PointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointA.transform)
        {
            currentPoint = PointB.transform;
        }
    }

    private void Return()
    {
        Debug.Log("Return");
        Vector3 direction = (ReturnPoint.transform.position - currentPoint.position) .normalized;
        
        // Set the velocity in the direction of the target
        rb.linearVelocity = direction * speed;
        //GuardState = 0;
    }


    /*

    private void Flip()
    {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        
    }

    */
}
