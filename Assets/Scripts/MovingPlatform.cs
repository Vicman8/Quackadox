using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int movementRange;
    [SerializeField] bool moveInY;

    private Vector3 startingPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = this.transform.position;
        if (movementRange < 0)
        {
            movementRange = 0 - movementRange;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveInY)
        {
            if (this.transform.position.x >= startingPosition.x + movementRange || this.transform.position.x <= startingPosition.x - movementRange)
            {
                speed = -speed;
            }

            this.transform.position = new Vector3(this.transform.position.x + speed, this.transform.position.y, this.transform.position.z);
        }
        else
        {
            if (this.transform.position.y >= startingPosition.y + movementRange || this.transform.position.y <= startingPosition.y - movementRange)
            {
                speed = -speed;
            }

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + speed, this.transform.position.z);
        }
    }
}
