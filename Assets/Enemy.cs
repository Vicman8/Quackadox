using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;

    public Transform PlayerPoint;

    //public EnemyAttack EnemyA;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = PointB.transform;


    }

    // Update is called once per frame
    public void Update()
    {
        /*
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == PointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
            //EnemyA.Flip();

        }
        else {
            rb.linearVelocity = new Vector2(-speed, 0);
            //EnemyA.Flip();
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointB.transform)
        {
            currentPoint = PointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointA.transform)
        {
            currentPoint = PointB.transform;
        }
        */
        //Guard();
        FollowPlayer();


    }

    public void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, PlayerPoint.position, .02f);
    }

    public void Guard()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == PointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
            //EnemyA.Flip();

        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
            //EnemyA.Flip();
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


}
