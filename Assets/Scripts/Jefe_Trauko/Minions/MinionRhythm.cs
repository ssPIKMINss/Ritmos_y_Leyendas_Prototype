using UnityEngine;
using System.Collections;

public class MinionRhythm : MonoBehaviour
{
    [Header("Stats")]
    public int health = 1;
    public int damageToPlayer = 1;

    [Header("Movement")]
    public float moveSpeed = 1.5f;

    [Header("Attack Dash")]
    public float dashDistance = 1f;
    public float dashSpeed = 8f;
    public float returnSpeed = 5f;

    [Header("Detection")]
    public float attackRange = 0.5f;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public Transform player;

    [Header("Animations")]
    public Animator animator;
    public string deathTrigger = "Death";      // Nombre del trigger de muerte en tu Animator

    private Rigidbody2D rb;
    private bool isDead = false;
    private bool isAttacking = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void OnEnable()  => BeatManager.OnBeat += ActOnBeat;
    void OnDisable() => BeatManager.OnBeat -= ActOnBeat;


    void ActOnBeat()
    {
        if (isDead || isAttacking || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange)
            Attack();
        else
            MoveTowardPlayer();
    }


    void MoveTowardPlayer()
    {
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        Vector2 targetPos = rb.position + dir * moveSpeed;
        rb.MovePosition(targetPos);
    }


    void Attack()
    {
        if (!isAttacking)
            StartCoroutine(AttackDash());
    }


    IEnumerator AttackDash()
    {
        isAttacking = true;

        Vector2 originalPos = rb.position;
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        if (dir == 0) dir = 1f;

        Vector2 targetPos = originalPos + Vector2.right * dashDistance * dir;

        while (Vector2.SqrMagnitude(rb.position - targetPos) > 0.001f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPos, dashSpeed * Time.fixedDeltaTime));
            CheckHit();
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.05f);

        while (Vector2.SqrMagnitude(rb.position - originalPos) > 0.001f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, originalPos, returnSpeed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }

        isAttacking = false;
    }


    void CheckHit()
    {
        if (attackPoint == null) return;

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (hit != null)
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damageToPlayer);
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

        if (animator != null)
            animator.SetTrigger(deathTrigger);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false; // mantener posición para que se vea la muerte
        }

        Destroy(gameObject, 0.8f); // ajuste según duración de animación
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
