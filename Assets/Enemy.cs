using UnityEngine;
public class Enemy : MonoBehaviour
{
    //The two point in which the enemy patrol in between
    public GameObject PointA;
    public GameObject PointB;
    //the point that the enemy returns to when left it's main post
    public GameObject ReturnPoint;
    private Rigidbody2D rb;
    //see what point the enemy is going to. for the guard function
    private Transform currentPoint;
    //speed for the enemy 
    public float speed;
    //the position of the player
    public Transform PlayerPoint;
    //indicates what the enemy will be doing. 0 = guarding, 1 = follow the player, and 2 is returning
    public float GuardState; 
    //for the return function
    public float stopDistance = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = PointB.transform;
        GuardState = 0;

    }

    // Update is called once per frame
    public void Update()
    {
        //this will indicate what state the enemy will be in. it will call the function depending on the state
        if (GuardState == 0)
        {
            Guard();
        } 
        else if (Mathf.Approximately(GuardState, 1))
        {
            FollowPlayer();
        }
        else if (Mathf.Approximately(GuardState, 2))
        {
            Return();
        }

    }

    //This function will use the position of the player and goes towards it
    private void FollowPlayer()
    {
        
        Vector3 direction = (PlayerPoint.position - transform.position).normalized;
        direction.y = 0f; 

      
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

    }

    //the enemy will go in between the two points (a and b)
    public void Guard()
    {

        if (currentPoint == PointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
            Vector3 localScale = transform.localScale;
            localScale.x = 1.25f;
            transform.localScale = localScale;
            //Debug.Log("right");

        }
        else if (currentPoint == PointA.transform)
        {
            rb.linearVelocity = new Vector2(-speed, 0);
            Vector3 localScale = transform.localScale;
            localScale.x = -1.25f;
            transform.localScale = localScale;
            //Debug.Log("left");
        }


        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointB.transform)
        {
            Debug.Log("going left");
            currentPoint = PointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointA.transform)
        {
            Debug.Log("going right");
            currentPoint = PointB.transform;
        }
    }

    //the enemy if gone to far from its original place will go back to the point in that area.
    private void Return()
    {
        Debug.Log("Return");
        Vector2 targetPosition = ReturnPoint.transform.position;
        
        if (Vector2.Distance(transform.position, targetPosition) > stopDistance && transform.position.x > 0)
        {
       
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            Vector3 localScale = transform.localScale;
            localScale.x = -1.25f;
            transform.localScale = localScale;
        }
        else if (Vector2.Distance(transform.position, targetPosition) > stopDistance && transform.position.x < 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            Vector3 localScale = transform.localScale;
            localScale.x = 1.25f;
            transform.localScale = localScale;
        }
        else
        {
          
            GuardState = 0;
            Debug.Log("im back");
            
        }
    }

}
