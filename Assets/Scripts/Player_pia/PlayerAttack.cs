using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("⚔️ Configuración del Ataque Melee")]
    [SerializeField] private Transform attackPoint;    
    [SerializeField] private float attackRange = 0.6f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 0.35f;

    [Header("🎞 Animación (opcional)")]
    [SerializeField] private Animator animator;

    private float nextAttackTime = 0f;


    private void Update()
    {
        // Click izquierdo para atacar
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }


    private void Attack()
    {
        if (animator != null) animator.SetTrigger("AttackTrigger");

        if (attackPoint == null)
        {
            Debug.LogWarning("⚠ No has asignado un AttackPoint en el Player.");
            return;
        }

        // Detectar enemigos en un círculo
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (var h in hits)
        {
            // 1) Enemigos normales con EnemyHealth_Minion
            EnemyHealth_Minion normalEnemy = h.GetComponentInParent<EnemyHealth_Minion>();
            if (normalEnemy != null)
            {
                normalEnemy.TakeDamage(damage);
                continue;
            }

            // 2) Minions rítmicos con MinionRhythm
            MinionRhythm rhythmMinion = h.GetComponentInParent<MinionRhythm>();
            if (rhythmMinion != null)
            {
                rhythmMinion.TakeDamage(damage);
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
