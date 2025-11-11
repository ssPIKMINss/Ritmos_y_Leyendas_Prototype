using UnityEngine;

public class EnemyHealth_Minion : MonoBehaviour
{
    [Header("Vida (minion muere de 1 golpe)")]
    [SerializeField] private int maxHealth = 1;   // déjalo en 1
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float destroyDelay = 0.25f; // tiempo para ver la anim de muerte (si hay)

    [Header("Opcional")]
    [Tooltip("Animator del minion (solo si tienes animación de muerte).")]
    [SerializeField] private Animator animator;
    [Tooltip("Nombre del Trigger de muerte en tu Animator (si existe).")]
    [SerializeField] private string dieTriggerName = "Die";
    [Tooltip("VFX o prefab que quieras instanciar al morir (opcional).")]
    [SerializeField] private GameObject deathVFX;

    private int currentHealth;
    private bool isDead;

    private void Awake()
    {
        currentHealth = Mathf.Max(1, maxHealth);
        if (!animator) animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Llama desde PlayerAttack: enemy.GetComponent&lt;EnemyHealth_Minion&gt;()?.TakeDamage(damage);
    /// </summary>
    public void TakeDamage(int amount = 1)
    {
        if (isDead) return;

        currentHealth -= Mathf.Max(1, amount);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Desactiva colisiones y movimiento básico
        var col = GetComponent<Collider2D>(); if (col) col.enabled = false;
        var rb  = GetComponent<Rigidbody2D>(); if (rb) { rb.linearVelocity = Vector2.zero; rb.isKinematic = true; }

        // Dispara anim de muerte SOLO si el trigger existe
        TrySetDieTrigger();

        // Instancia VFX (opcional)
        if (deathVFX) Instantiate(deathVFX, transform.position, Quaternion.identity);

        if (destroyOnDeath) Destroy(gameObject, destroyDelay);
        else gameObject.SetActive(false);
    }

    private void TrySetDieTrigger()
    {
        if (!animator || string.IsNullOrEmpty(dieTriggerName)) return;
        foreach (var p in animator.parameters)
        {
            if (p.type == AnimatorControllerParameterType.Trigger && p.name == dieTriggerName)
            {
                animator.SetTrigger(dieTriggerName);
                return;
            }
        }
        // Si no existe el trigger, no hacemos nada (evita warnings).
    }
}
