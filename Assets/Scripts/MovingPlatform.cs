using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int movementRange;
    [SerializeField] bool moveInY;
    [SerializeField] PlayerMovement player;

    private float actualSpeed;
    private Vector3 middlePosition;
    private Vector3 endPosition;
    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = this.transform.position;

        if (!moveInY)
        {
            middlePosition = new Vector3(this.transform.position.x + movementRange, this.transform.position.y, this.transform.position.z);
            endPosition = new Vector3(middlePosition.x + movementRange, middlePosition.y, middlePosition.z);
        }
        else
        {
            middlePosition = new Vector3(this.transform.position.x, this.transform.position.y + movementRange, this.transform.position.z);
            endPosition = new Vector3(middlePosition.x, middlePosition.y + movementRange, middlePosition.z);
        }
        if (movementRange < 0)
        {
            movementRange = 0 - movementRange;
        }
        
        actualSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveInY)
        {
            if (actualSpeed != 0 && (this.transform.position.x >= middlePosition.x + movementRange || this.transform.position.x <= middlePosition.x - movementRange))
            {
                actualSpeed = -actualSpeed;
                Debug.Log(actualSpeed);
            }

            this.transform.position = new Vector3(this.transform.position.x + actualSpeed * Time.deltaTime, this.transform.position.y, this.transform.position.z);

/*            Vector3 currPosition = this.transform.position;
            if (actualSpeed == 0 && !(currPosition.x <= startPosition.x || currPosition.x >= endPosition.x))
            {
                //Debug.Log("Stopped at non endpoint");
                float distToEnd = endPosition.x - currPosition.x;
                float distToStart = currPosition.x - startPosition.x;

                if(distToEnd < distToStart)
                {
                    //Debug.Log("Adjusting to end position");
*//*                    if(speed < 0)
                    {
                        speed = -speed;
                    }*//*

                    this.transform.position = new Vector3(this.transform.position.x + speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
                }
                else
                {
                    *//*if(speed > 0)
                    {
                        speed = -speed;
                    }*//*

                    //Debug.Log("Adjusting to start position");
                    this.transform.position = new Vector3(this.transform.position.x + speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
                }
            }*/
        }
        else
        {
            if (this.transform.position.y >= middlePosition.y + movementRange || this.transform.position.y <= middlePosition.y - movementRange)
            {
                actualSpeed = -actualSpeed;
            }

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + actualSpeed * Time.deltaTime, this.transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == player.gameObject)
        {
            Debug.Log("Actual speed set to speed");
            actualSpeed = speed;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            player.PlayerFollowPlatform(true, actualSpeed);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject == player.gameObject)
        {
            Debug.Log("Deactivation");
            player.PlayerFollowPlatform(false, 0);
        }
    }
}
