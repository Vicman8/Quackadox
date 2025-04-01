using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Color originalBackgroundColor;

    private float horizontal;
    private float speed = 8f;
    private float jumpPower = 60f;
    private bool isFacingRight = true;

    //Dashing
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 5f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    [SerializeField] public Rigidbody2D rb;

    // Dash Charge System
    private float maxDashCharge = 5f;
    private float currentDashCharge = 5f;
    private float dashRechargeRate = 1f;

    [SerializeField] private Rigidbody2D rb;

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

    void Start()
    {
        // Save the original background color when the game starts
        originalBackgroundColor = Camera.main.backgroundColor;
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
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && UI.pauseState == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            animator.Play("DuckJump");
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // Trigger dash when Shift key is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && currentDashCharge > 0)
        {
            StartCoroutine(PortalDash());
            animator.Play("DuckDash");
        }

        UpdateAnimationState();
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing || isBeingPushed)
        {
            return;
        }
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
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
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;

        Vector2 dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        float dashDistance = 5f;

        // Create entry portal farther from the player
        Vector2 entryPosition = (Vector2)transform.position + dashDirection * dashDistance;
        GameObject entryPortal = Instantiate(portalPrefab, entryPosition, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);

        // Create exit portal
        Vector2 exitPosition = entryPosition + dashDirection * dashDistance;
        GameObject exitPortal = Instantiate(portalPrefab, exitPosition, Quaternion.identity);

        // Switch the world after the dash
        SwitchWorld(); // Call world switch method here

        // Teleport player to exit portal
        transform.position = exitPosition;

        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        Destroy(entryPortal);
        Destroy(exitPortal);

        yield return new WaitForSeconds(dashingCooldown);
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
            Camera.main.backgroundColor = Color.red;
            Debug.Log("World switched to red!");
        }
        else
        {
            // If it's already altered, switch back to the original color
            Camera.main.backgroundColor = originalBackgroundColor;
            Debug.Log("World switched back to the original state!");
        }
    }
}