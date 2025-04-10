using System.Security.Cryptography;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackPrefab; // Prefab to instantiate for attack
    public float attackSpeed = 10f; // Speed of attack movement
    public float attackLifetime = 1f; // How long the attack stays before disappearing

    private int attackIndex = 0; // Keep track of how many attacks we've shot

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Long Range Attack");

            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.PlayDuckQuackAnimation();
                Debug.Log("AnimationQuack");
            }
            PerformLongRangeAttack();
        }
        
    }

    void PerformLongRangeAttack()
    {
        // Reset the attack counter before starting the sequence
        attackIndex = 0;

        // Loop to shoot out 3 projectiles (or attacks) one after the other
        ShootAttackWithDelay();
    }

    void ShootAttackWithDelay()
    {
        // Ensure we only shoot 3 attacks
        if (attackIndex < 3)
        {
            // Instantiate the attack prefab at the player's position
            GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);

            // Get the player's facing direction
            Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

            // Apply an offset for each shot to slightly space out the projectiles
            direction += new Vector3(attackIndex * 0.2f, 0, 0); // Slight offset for each attack

            // Set the attack's velocity or direction
            Rigidbody2D rb = attack.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * attackSpeed;
            }

            // Destroy the attack after its lifetime
            Destroy(attack, attackLifetime);

            // Increment the attackIndex to prepare for the next attack
            attackIndex++;

            // Call the method again after a delay for the next attack
            Invoke(nameof(ShootAttackWithDelay), 0.2f); // Delay between each attack
        }
    }
}