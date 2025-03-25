using UnityEngine;

public class New_Camera_Control : MonoBehaviour
{
    private Vector3 targetPoint = Vector3.zero;

    public PlayerMovement player;

    public float Speed;

    public float LookAheadDistance = 5f, LookAheadSpeed = 3f;

    private float LookOffset;

    private bool isFalling;

    public float maxvertOffset = 5f;

    public void Start()
    {
        targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, -10);

    }

    public void LateUpdate()
    {
        if (player.IsGrounded())
        {
            targetPoint.y = player.transform.position.y;
        }

        if (transform.position.y - player.transform.position.y > maxvertOffset)
        {
            isFalling = true;
        }

        if (isFalling)
        {
            targetPoint.y = player.transform.position.y;

            if (player.IsGrounded())
            {
                isFalling = false; 
            }
        }

        //targetPoint.x = player.transform.position.x;
        //targetPoint.y = player.transform.position.y;

        if(player.rb.linearVelocity.x > 0f)
        {
            LookOffset = Mathf.Lerp(LookOffset, LookAheadDistance, LookAheadSpeed * Time.deltaTime);
        }
        else if (player.rb.linearVelocity.x < 0f)
        {
            LookOffset = Mathf.Lerp(LookOffset, -LookAheadDistance, LookAheadSpeed * Time.deltaTime);
        }

        targetPoint.x = player.transform.position.x + LookOffset;

        transform.position = Vector3.Lerp(transform.position, targetPoint, Speed * Time.deltaTime);
    }
}
