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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            DoAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void DoAttack()
    {
        if (animator != null) animator.SetTrigger("AttackTrigger");

        if (attackPoint == null)
        {
            Debug.LogWarning("AttackPoint no asignado.");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D h in hits)
        {
            // Minion normal
            var minion = h.GetComponentInParent<EnemyHealth_Minion>();
            if (minion != null)
            {
                minion.TakeDamage(damage);
                continue;
            }

            // Minion rítmico
            var rhythm = h.GetComponentInParent<MinionRhythm>();
            if (rhythm != null)
            {
                rhythm.TakeDamage(damage);
                continue;
            }

            // Boss Trauko
            var boss = h.GetComponentInParent<TraukoBossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                continue;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
