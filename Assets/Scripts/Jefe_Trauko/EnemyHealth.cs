using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Salud")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private bool destroyOnDeath = true;

    [Header("Animaciones")]
    [SerializeField] private Animator animator;
    [SerializeField] private string hitTrigger = "Hit";
    [SerializeField] private string deathTrigger = "Die";
    [SerializeField] private float deathDelay = 0.8f; // duración animación muerte

    private int currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        if (!animator) animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount = 1)
    {
        if (isDead) return;

        currentHealth -= amount;

        // 🔥 Animación de daño
        if (animator && !string.IsNullOrEmpty(hitTrigger))
            animator.SetTrigger(hitTrigger);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // ☠️ Animación de muerte
        if (animator && !string.IsNullOrEmpty(deathTrigger))
            animator.SetTrigger(deathTrigger);

        // Desactivar colisiones
        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        // Desactivar otros scripts (IA, movimiento, ataque)
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
        {
            if (s != this)
                s.enabled = false;
        }

        if (destroyOnDeath)
        {
            Destroy(gameObject, deathDelay);
        }
        else
        {
            Invoke(nameof(DisableObject), deathDelay);
        }
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
