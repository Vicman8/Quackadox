using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed;

    public void Start()
    {
        {
            rb = GetComponent<Rigidbody2D>();
            currentPoint = PointB.transform;
            //Guarding = true;

        }
    }
    public void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == PointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
    
            //Flip();
            //EnemyA.Flip();

        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
   
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
}
