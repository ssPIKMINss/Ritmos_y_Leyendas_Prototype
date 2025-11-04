using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Salud")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private bool destroyOnDeath = true;

    [Header("Feedback (opcional)")]
    [SerializeField] private Animator animator; // Trigger "Hit"/"Die" si lo usas

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        if (!animator) animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount = 1)
    {
        currentHealth -= amount;
        if (animator) animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (animator) animator.SetTrigger("Die");
        // Aquí puedes desactivar colisiones, IA, etc.
        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        if (destroyOnDeath)
        {
            // pequeño delay si quieres que se vea la animación
            Destroy(gameObject, 0.25f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
