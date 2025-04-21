using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackPrefab; // Prefab to instantiate for attack
    public float attackSpeed = 10f; // Speed of attack movement
    public float attackLifetime = 1f; // How long the attack stays before disappearing

    private int attackIndex = 0; // Keep track of how many attacks we've shot

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Long Range Attack");

            if (playerMovement != null)
            {
                playerMovement.PlayDuckQuackAnimation(); // Set bool true
                Debug.Log("AnimationQuack");

                // Stop animation after a delay (match the animation duration)
                StartCoroutine(StopQuackAfterDelay(0.5f)); // Adjust 0.5f to match your animation length
            }

            PerformLongRangeAttack();
        }
    }

    IEnumerator StopQuackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerMovement != null)
        {
            playerMovement.StopDuckQuackAnimation(); // Set bool false
        }
    }

    void PerformLongRangeAttack()
    {
        attackIndex = 0;
        ShootAttackWithDelay();
    }

    void ShootAttackWithDelay()
    {
        if (attackIndex < 3)
        {
            GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);

            Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
            direction += new Vector3(attackIndex * 0.2f, 0, 0);

            Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * attackSpeed;
            }

            Destroy(attack, attackLifetime);
            attackIndex++;

            Invoke(nameof(ShootAttackWithDelay), 0.2f);
        }
    }
}