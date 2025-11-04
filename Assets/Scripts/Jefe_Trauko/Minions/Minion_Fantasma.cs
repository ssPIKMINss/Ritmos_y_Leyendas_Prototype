using UnityEngine;

public class Minion : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f; // velocidad de movimiento
    public bool destroyOnWall = false; // si quieres que desaparezca al chocar con pared

    [Header("Combate")]
    public int damage = 1;
    public int maxHealth = 3;

    private int currentHealth;
    private bool isDead = false;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Aseguramos que comience corriendo a la izquierda
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
    }

    void Update()
    {
        if (isDead) return;

        // Mantener velocidad constante hacia la izquierda
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // Dañar al jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
                player.TakeDamage(damage);
        }

        // Si choca con una pared o límite, desaparecer o girar
        if (collision.contacts[0].normal.x > 0.5f) // pared en frente
        {
            if (destroyOnWall)
            {
                Destroy(gameObject);
            }
            else
            {
                Flip();
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y); // girar dirección
                speed = -speed; // invertir sentido
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Die");

        Destroy(gameObject, 1f);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
