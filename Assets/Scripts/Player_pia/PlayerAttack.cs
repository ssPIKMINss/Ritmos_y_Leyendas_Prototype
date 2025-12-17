using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("⚔️ Ataque Melee")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.6f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 0.35f;

    [Header("🎞 Animación")]
    [SerializeField] private Animator animator;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("AttackTrigger");

        if (attackPoint == null)
        {
            Debug.LogWarning("AttackPoint no asignado");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            // Enemy normal
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                continue;
            }

            // Enemy Minion
            EnemyHealth_Minion minion = hit.GetComponentInParent<EnemyHealth_Minion>();
            if (minion != null)
            {
                minion.TakeDamage(damage);
                continue;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
