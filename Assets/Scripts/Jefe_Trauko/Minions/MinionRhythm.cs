using UnityEngine;

public class MinionRhythm : MonoBehaviour
{
    [Header("Stats")]
    public int health = 1;
    public int damageToPlayer = 1;

    [Header("Movement")]
    public float moveSpeed = 1.5f; // mov por beat

    [Header("Detection")]
    public float attackRange = 0.5f;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public Transform player;

    private Rigidbody2D rb;
    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        BeatManager.OnBeat += ActOnBeat;
    }

    void OnDisable()
    {
        BeatManager.OnBeat -= ActOnBeat;
    }

    void ActOnBeat()
    {
        if (isDead) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange)
            Attack();
        else
            MoveTowardPlayer();
    }

    void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 targetPos = (Vector2)transform.position + direction * moveSpeed;

        rb.MovePosition(targetPos);  // 🟢 FÍSICAS CORRECTAS
    }

    void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRange,
            playerLayer
        );

        if (hit != null)
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(damageToPlayer);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        health -= dmg;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        Destroy(gameObject, 0.1f);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
