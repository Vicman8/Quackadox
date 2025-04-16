using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Color originalBackgroundColor;

    private bool hasTeleported = false;

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

    private bool isJumping = false;
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
            isJumping = true;
            FindObjectOfType<AudioManager>().Play("Jump");

        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // Reset jumping state once grounded
        if (IsGrounded() && isJumping)
        {
            isJumping = false;
            animator.Play("DuckIdle"); // Return to idle once grounded
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
        // Reset teleport flag
        hasTeleported = false;

        canDash = false;
        isDashing = true;
        currentDashCharge -= 1f;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        tr.emitting = true;

        Vector2 dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        float dashDistance = 5f;
        Vector2 entryPosition = (Vector2)transform.position + dashDirection * dashDistance;

        // Instantiate entry portal
        GameObject entryPortal = Instantiate(portalPrefab, entryPosition, Quaternion.identity);
        Animator portalAnimator = entryPortal.GetComponent<Animator>();
        portalAnimator.Play("Portal");

        // Set the player reference on the portal
        PortalTrigger portalTrigger = entryPortal.GetComponent<PortalTrigger>();
        if (portalTrigger != null)
        {
            portalTrigger.SetPlayerReference(this);
        }

        // Wait for portal animation to be ready
        float portalAnimationTime = 1f;
        yield return new WaitForSeconds(portalAnimationTime);

        // Dash forward
        float dashDuration = 1f;
        Vector2 targetPosition = entryPosition;
        float timeElapsed = 0f;

        while (timeElapsed < dashDuration && !hasTeleported)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, timeElapsed / dashDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // If we never hit the portal (rare case), teleport anyway
        if (!hasTeleported)
        {
            transform.position = targetPosition;
            TeleportToAlternateWorld();
        }

        // Wait a bit before destroying the entry portal
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        // Destroy the entry portal
        Destroy(entryPortal);

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public void TeleportToAlternateWorld()
    {
        if (!hasTeleported)
        {
            hasTeleported = true;

            // Calculate the position where the exit portal should appear in the new world
            Vector2 exitPortalPosition;
            Vector2 playerExitPosition;

            if (!isInAlternateLevel)
            {
                // Going to alternate world
                exitPortalPosition = (Vector2)transform.position + levelOffset;
                // Store original position before teleporting
                originalLevelPosition = transform.position;

                // Player should appear slightly to the right/left of the exit portal
                Vector2 exitDirection = isFacingRight ? Vector2.right : Vector2.left;
                playerExitPosition = exitPortalPosition + exitDirection * 1.5f;
            }
            else
            {
                // Returning to original world
                exitPortalPosition = originalLevelPosition;

                // Player should appear slightly to the right/left of the exit portal
                Vector2 exitDirection = isFacingRight ? Vector2.right : Vector2.left;
                playerExitPosition = exitPortalPosition + exitDirection * 1.5f;
            }

            // Create the exit portal at the destination position
            GameObject exitPortal = Instantiate(portalPrefab, exitPortalPosition, Quaternion.identity);
            Animator exitPortalAnimator = exitPortal.GetComponent<Animator>();
            exitPortalAnimator.Play("Portal");

            // Switch world state
            isInAlternateLevel = !isInAlternateLevel;

            // Start a coroutine to handle the player's emergence from the portal
            StartCoroutine(EmergeThroughPortal(playerExitPosition, exitPortal));

            // Switch world colors or background
            SwitchWorld();
        }
    }

    private IEnumerator EmergeThroughPortal(Vector2 playerExitPosition, GameObject exitPortal)
    {
        // First teleport player to exit portal position
        transform.position = exitPortal.transform.position;

        // Give the portal a moment to be visible
        yield return new WaitForSeconds(0.2f);

        // Move player out from the portal
        float emergeTime = 0.3f;
        float emergeTimer = 0;
        Vector2 startPos = transform.position;

        // Play player emerge animation if you have one
        // animator.Play("EmergingAnimation");

        while (emergeTimer < emergeTime)
        {
            transform.position = Vector2.Lerp(startPos, playerExitPosition, emergeTimer / emergeTime);
            emergeTimer += Time.deltaTime;
            yield return null;
        }

        // Ensure player ends at the exact destination
        transform.position = playerExitPosition;

        // Destroy the exit portal after a short delay
        yield return new WaitForSeconds(0.5f);
        Destroy(exitPortal);
    }

    private void UpdateAnimationState()
    {
        // Check if the player is in the air (jumping or falling)
        if (isJumping || rb.linearVelocity.y > 0.1f) // Player is in the air
        {
            animator.Play("DuckJump");
        }
        else if (Mathf.Abs(horizontal) > 0.1f) // Player is moving horizontally
        {
            animator.Play("DuckWalk");
        }
        else // Player is not moving horizontally
        {
            animator.Play("DuckIdle");
        }

        // Adjust the "Speed" float parameter in Animator for walking
        animator.SetFloat("Speed", Mathf.Abs(horizontal) * speed);
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