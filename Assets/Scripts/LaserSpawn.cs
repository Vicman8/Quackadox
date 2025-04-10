using UnityEngine;

public class LaserSpawn : MonoBehaviour
{
    private bool isActive;
    private float timer;
    private float laserActivationTime;
    [SerializeField] private bool isMovingLaser;
    [SerializeField] private int movementRange;
    [SerializeField] private float speed;
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private bool moveInY;
    [SerializeField] private bool laserStaysOn;
    private Vector3 startingPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = false;
        laserActivationTime = 3;
        timer = laserActivationTime;
        startingPosition = this.transform.position;
        if (movementRange < 0)
        {
            movementRange = 0 - movementRange;
        }

        if(laserStaysOn)
        {
            laserBeam.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!laserStaysOn && timer <= 0)
        {
            laserBeam.SetActive(!isActive);
            isActive = !isActive;
            timer = laserActivationTime;
        }

        if(!laserStaysOn)
        { 
            timer -= Time.deltaTime;
        }

        if (isMovingLaser && !moveInY)
        {
            if (this.transform.position.x >= startingPosition.x + movementRange || this.transform.position.x <= startingPosition.x - movementRange)
            {
                speed = -speed;
            }

            this.transform.position = new Vector3(this.transform.position.x + speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
        }
        else if(isMovingLaser)
        {
            if (this.transform.position.y >= startingPosition.y + movementRange || this.transform.position.y <= startingPosition.y - movementRange)
            {
                speed = -speed;
            }

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + speed * Time.deltaTime, this.transform.position.z);
        }
    }
}
