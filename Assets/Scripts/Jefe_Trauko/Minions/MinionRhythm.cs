using UnityEngine;

public class MinionRhythm : MonoBehaviour
{
    [Header("Stats")]
    public int health = 1;
    public int damageToPlayer = 1;

    [Header("Detection")]
    public float attackRange = 1f;
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 3f;

    private bool isDead = false;

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

        // Attack if close enough
        if (dist <= attackRange)
        {
            Attack();
        }
        else
        {
            MoveTowardPlayer();
        }
    }

    void MoveTowardPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * moveSpeed);
    }

    void Attack()
    {
        // Aquí llamas al script del jugador
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(damageToPlayer);
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
        // animación opcional
        Destroy(gameObject, 0.1f);
    }
}
