using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;

    public Transform PlayerPoint;

    public bool Guarding;

    //public EnemyAttack EnemyA;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = PointB.transform;
        Guarding = true;

    }

    // Update is called once per frame
    public void Update()
    {
        if (Guarding)
        {
            Guard();
        }
        else if (!Guarding)
        {
            FollowPlayer();
        }
    }

    public void FollowPlayer()
    {
        //Guarding = false;
        Vector3 direction = (PlayerPoint.position - transform.position).normalized;
        direction.y = 0f; // Lock movement on Y-axis

        // Apply velocity with X-axis movement only
        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, 0f);
    }

    public void Guard()
    {
        //Guarding = true;
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
