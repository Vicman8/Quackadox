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

    // Dash Charge System
    private float maxDashCharge = 5f;  // Total dash charge
    private float currentDashCharge = 5f;  // Current available charge
    private float dashRechargeRate = 1f;  // Charge per second
    private float dashChargeTime = 0f;

    // Dash Charge Durations
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
    private float dashRechargeCooldownTimer = 0f;

    void Update()
    {
        // Dash Charge Recharge
        if (currentDashCharge < maxDashCharge)
        {
            currentDashCharge += dashRechargeRate * Time.deltaTime;
            currentDashCharge = Mathf.Min(currentDashCharge, maxDashCharge);
        }

        // Dash Recharge Cooldown Timer
        if (dashRechargeCooldownTimer > 0)
        {
            dashRechargeCooldownTimer -= Time.deltaTime;
            Debug.Log($"Dash Recharge Cooldown: {dashRechargeCooldownTimer:F2} seconds remaining");
        }

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
            if (dashChargeTime > 0 && currentDashCharge > 0)
            {
                float dashCost = CalculateDashCost(dashChargeTime);
                if (currentDashCharge >= dashCost)
                {
                    StartCoroutine(PortalDash(dashCost));
                    animator.Play("DuckDash");
                }
                else
                {
                    Debug.Log("Not enough dash charge!");
                }
            }
            dashChargeTime = 0f;
        }

        UpdateAnimationState();
        Flip();
    }

    private float CalculateDashCost(float chargeTime)
    {
        if (chargeTime < minChargeDuration)
        {
            return 1f;  // Minimal dash
        }
        else if (chargeTime < shortChargeDuration)
        {
            return 2f;  // Short dash
        }
        else if (chargeTime < longChargeDuration)
        {
            return 3f;  // Medium dash
        }
        else
        {
            return 5f;  // Full charge dash
        }
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

    private IEnumerator PortalDash(float dashCost)
    {
        canDash = false;
        isDashing = true;
        currentDashCharge -= dashCost;

        // Set cooldown based on dash cost
        dashRechargeCooldownTimer = dashCost;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;

        Vector2 dashDirection = isFacingRight ? Vector2.right : Vector2.left;

        // Determine dash distance based on charge time
        float dashDistance;
        if (dashCost <= 1f)
        {
            dashDistance = shortDashDistance;
        }
        else if (dashCost <= 2f)
        {
            dashDistance = mediumDashDistance;
        }
        else if (dashCost <= 3f)
        {
            dashDistance = longDashDistance;
        }
        else
        {
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

        Debug.Log($"Dash Charge Remaining: {currentDashCharge:F2}");
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
}