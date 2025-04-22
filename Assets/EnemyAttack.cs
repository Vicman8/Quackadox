using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject player;  // Reference to the player
    public TEMP_Player_Health Phealth;  // Health system for player
    private Animator enemyAnimator;  // Reference to the enemy's Animator

    void Start()
    {
        // Get the Animator component from the enemy's parent (or the enemy object itself)
        enemyAnimator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // If the enemy collides with the player, trigger the attack animation
        if (collider.gameObject.CompareTag("Player"))
        {
            // Trigger the IsAttacking animation
            if (enemyAnimator != null)
            {
                Debug.Log("Setting IsAttacking to true");
                enemyAnimator.SetBool("IsAttacking", true);
            }

            // Damage the player
            Phealth.Damaged();

            // Optionally, you can set IsAttacking back to false after some time (if the attack has an animation length)
            StartCoroutine(ResetAttackAnimation());
        }
    }

    // Coroutine to reset IsAttacking after the attack animation plays (adjust time based on your animation duration)
    private IEnumerator ResetAttackAnimation()
    {
        // Wait for the attack animation duration (adjust the time accordingly)
        yield return new WaitForSeconds(1f);  // Adjust the duration to match your attack animation time

        // Reset the IsAttacking parameter to false to return to the idle or other state
        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("IsAttacking", false);
            Debug.Log("Resetting IsAttacking to false");
        }
    }
}
