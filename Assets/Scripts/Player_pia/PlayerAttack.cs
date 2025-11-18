using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("⚔️ Configuración del Ataque Melee")]
    [SerializeField] private Transform attackPoint;    // Arrastra aquí el Empty 'attackPoint'
    [SerializeField] private float attackRange = 0.6f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 0.35f;

    [Header("🎞️ Animación (opcional)")]
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

        if (attackPoint == null) { Debug.LogWarning("AttackPoint no asignado."); return; }

        var hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (var h in hits)
        {
            h.GetComponent<EnemyHealth_Minion>()?.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
