using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Melee Attack");
            PerformMeleeAttack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Long Range Attack");
            PerformLongRangeAttack();
        }
    }

    void PerformMeleeAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 1f);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }

    void PerformLongRangeAttack()
    {
        Debug.Log("Long Range Attack Performed");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
