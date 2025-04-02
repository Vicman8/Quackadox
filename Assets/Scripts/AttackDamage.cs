using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    public int damage = 1; // Amount of damage this attack does

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if we collided with an enemy
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();

        // If the object has an EnemyHealth component, damage it
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);

            // Optional: destroy the projectile after hitting an enemy
            Destroy(gameObject);
        }
    }
}

