using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Color originalBackgroundColor;

    //Walk sound
    private float walkSoundToggleTimer = 0f;
    private bool playHighSound = true;

    private float horizontal;
    private float speed = 8f;
    private float jumpPower = 60f;
    private bool isFacingRight = true;

    //Dashing
    private bool canDash = false;
    private bool isDashing;
    private float dashingPower = 5f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    // Dash Charge System
    private float maxDashCharge = 5f;
    private float currentDashCharge = 5f;
    private float dashRechargeRate = 1f;

    // Level Switching
    private bool isInAlternateLevel = false;
    [SerializeField] private Vector2 levelOffset = new Vector2(0f, -100f); // adjust this offset to match distance between levels
    private Vector2 originalLevelPosition;


    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject portalPrefab;


    //UI stuff
    public UI_Controls UI;

    // Update is called once per frame

    private bool isBeingPushed = false;
    private float dashRechargeCooldownTimer = 0f;
    private float platformSpeed;
    private bool playerOnPlatform = false;

    [SerializeField] private AudioManager audioManager;
    void Start()
    {
        // Save the original background color when the game starts
        originalBackgroundColor = Camera.main.backgroundColor;
        originalLevelPosition = transform.position;
    }


    void Update()
    {
        // Dash Charge Recharge
        if (currentDashCharge < maxDashCharge)
        {
            currentDashCharge += dashRechargeRate * Time.deltaTime;
            currentDashCharge = Mathf.Min(currentDashCharge, maxDashCharge);
        }

        if (dashRechargeCooldownTimer > 0)
        {
            dashRechargeCooldownTimer -= Time.deltaTime;
        }

        if (isDashing)
        {
            return;
        }

        if (UI.pauseState == 0)
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            // Toggle walking sound
            if (horizontal != 0)
            {
                walkSoundToggleTimer -= Time.deltaTime;

                // Alternate the sound based on a timer or movement
                if (walkSoundToggleTimer <= 0f)
                {
                    if (playHighSound)
                    {
                        FindObjectOfType<AudioManager>().Play("WalkHigh");
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().Play("WalkLow");
                    }

                    // Toggle sound
                    playHighSound = !playHighSound;

                    // Reset the timer to control how fast it alternates (example: every 0.5 seconds)
                    walkSoundToggleTimer = 0.5f;
                }
            }
            else
            {
                // Stop both sounds when not moving
                walkSoundToggleTimer = 0f;  // Reset timer when player stops moving
            }
        }


        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && UI.pauseState == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            animator.Play("DuckJump");
            FindObjectOfType<AudioManager>().Play("Jump");

        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // Trigger dash when Shift key is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && currentDashCharge > 0)
        {
            if (canDash)
            {
                StartCoroutine(PortalDash());
                animator.Play("DuckDash");
            }
        }

        UpdateAnimationState();
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing || isBeingPushed)
        {
            Debug.Log("Return");
            return;
        }

        if (!playerOnPlatform)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
        else if (horizontal != 0)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(platformSpeed, rb.linearVelocity.y);
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public float GetHorizontal()
    {
        return horizontal;
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator PortalDash()
    {
        canDash = false;
        isDashing = true;
        currentDashCharge -= 1f; // Deduct charge for each dash

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero; // Stop the player before the dash
        tr.emitting = true; // Trail effect starts

        // Dash direction based on facing
        Vector2 dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        float dashDistance = 5f;

        // Instantiate the portal sprite and start animation
        Vector2 entryPosition = (Vector2)transform.position + dashDirection * dashDistance;
        Vector2 returnExitPosition = (Vector2)transform.position + dashDirection * 1.5f;  // Adjusting the offset if necessary

        // Debug log for positions
        Debug.Log("Entry Position: " + entryPosition);
        Debug.Log("Return Exit Position: " + returnExitPosition);

        // Instantiate entry portal at the entry position
        GameObject entryPortal = Instantiate(portalPrefab, entryPosition, Quaternion.identity);
        Animator portalAnimator = entryPortal.GetComponent<Animator>();
        portalAnimator.Play("Portal"); // Play the "portal" animation for entry

        // Wait before moving forward to give time for portal animation
        float portalAnimationTime = 1f;  // Time in seconds to let the portal animation play before moving forward
        yield return new WaitForSeconds(portalAnimationTime);

        // Dash forward (move the player to the portal)
        float dashSpeed = 5f;  // Dash speed (you can adjust this to make it slower or faster)
        float dashDuration = 1f;  // Time to move forward into the portal

        Vector2 targetPosition = (Vector2)transform.position + dashDirection * dashDistance;
        float timeElapsed = 0f;

        while (timeElapsed < dashDuration)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, timeElapsed / dashDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure we exactly reach the target position

        // After moving to the portal, create the exit portal
        GameObject exitPortal = Instantiate(portalPrefab, returnExitPosition, Quaternion.identity);
        portalAnimator = exitPortal.GetComponent<Animator>();
        portalAnimator.Play("Portal"); // Play the same "portal" animation for the exit portal

        // Teleport player to the "other world"
        if (!isInAlternateLevel)
        {
            originalLevelPosition = transform.position; // Save original position
            transform.position = (Vector2)transform.position + levelOffset;
        }
        else
        {
            transform.position = originalLevelPosition; // Go back to original level
        }

        isInAlternateLevel = !isInAlternateLevel;

        // Optional: Switch world colors or background
        SwitchWorld();

        // Wait for player to enter the portal before continuing (sync with animation)
        yield return new WaitForSeconds(dashingTime); // Wait for dash animation to finish
        tr.emitting = false; // Stop the trail effect
        rb.gravityScale = originalGravity;
        isDashing = false;

        // Destroy portals after the dash
        Destroy(entryPortal);
        Destroy(exitPortal);

        yield return new WaitForSeconds(dashingCooldown); // Dash cooldown
        canDash = true;
    }

    private void UpdateAnimationState()
    {
        if (!IsGrounded())
        {
            animator.Play("DuckJump");
        }
        else if (horizontal != 0)
        {
            animator.Play("DuckWalk");
        }
        else
        {
            animator.Play("DuckIdle");
        }
    }

    public Rigidbody2D GetRB()
    {
        return rb;
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public bool GetPushed()
    {
        return isBeingPushed;
    }

    public void SetPushed(bool pushed)
    {
        isBeingPushed = pushed;
    }

    public void PlayDuckQuackAnimation()
    {
        if (animator != null)
        {
            animator.Play("DuckQuack");
        }
    }


    private void SwitchWorld()
    {
        // Check if the world is currently in the altered state
        if (Camera.main.backgroundColor == originalBackgroundColor)
        {
            // If it's the original color, switch to the new color (e.g., red)
            Camera.main.backgroundColor = Color.magenta;
            Debug.Log("World switched to red!");
        }
        else
        {
            // If it's already altered, switch back to the original color
            Camera.main.backgroundColor = originalBackgroundColor;
            Debug.Log("World switched back to the original state!");
        }
    }

    public void PlayerFollowPlatform(bool isOnPlatform, float speed)
    {
        playerOnPlatform = isOnPlatform;
        platformSpeed = speed;
    }

    public void SetDashUnlocked(bool unlocked)
    {
        canDash = unlocked;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("COLLIDING: " + collider.gameObject);
    }
}