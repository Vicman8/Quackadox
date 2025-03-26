using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

    // New dash charging variables
    private float dashChargeTime = 0f;
    private float minChargeDuration = 0.5f;  // Minimum time to start charging
    private float shortChargeDuration = 3f;  // Short charge duration
    private float longChargeDuration = 5f;   // Long charge duration
    private float shortDashDistance = 3f;    // Distance for short dash
    private float mediumDashDistance = 5f;   // Distance for medium dash
    private float longDashDistance = 8f;     // Distance for long dash

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject portalPrefab;

    private bool isBeingPushed = false;

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            animator.Play("DuckJump");
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // Dash charging mechanism
        if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            dashChargeTime += Time.deltaTime;
        }

        // Trigger dash when Shift key is released
        if (Input.GetKeyUp(KeyCode.LeftShift) && canDash)
        {
            if (dashChargeTime > 0)
            {
                StartCoroutine(PortalDash());
                animator.Play("DuckDash");
            }
            dashChargeTime = 0f;
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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Added back the GetHorizontal method
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
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;

        Vector2 dashDirection = isFacingRight ? Vector2.right : Vector2.left;

        // Determine dash distance based on charge time
        float dashDistance;
        if (dashChargeTime < minChargeDuration)
        {
            // Very short tap - minimal dash
            dashDistance = shortDashDistance;
        }
        else if (dashChargeTime < shortChargeDuration)
        {
            // Short charge
            dashDistance = mediumDashDistance;
        }
        else if (dashChargeTime < longChargeDuration)
        {
            // Longer charge
            dashDistance = longDashDistance;
        }
        else
        {
            // Maximum charge
            dashDistance = longDashDistance * 1.5f;
        }

        // Create entry portal farther from the player
        Vector2 entryPosition = (Vector2)transform.position + dashDirection * dashDistance;
        GameObject entryPortal = Instantiate(portalPrefab, entryPosition, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);

        // Create exit portal
        Vector2 exitPosition = entryPosition + dashDirection * dashDistance;
        GameObject exitPortal = Instantiate(portalPrefab, exitPosition, Quaternion.identity);

        // Teleport player to exit portal
        transform.position = exitPosition;

        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        // Remove portals after dash
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
}