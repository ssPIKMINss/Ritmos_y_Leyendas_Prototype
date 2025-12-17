using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida del jugador")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float restartDelay = 0.9f; // tiempo para ver la anim de muerte
    private int currentHealth;
    private bool isDead;

    [Header("Animación")]
    [SerializeField] private Animator animator;
    [SerializeField] private string deathTriggerName = "Death";
    [SerializeField] private string hitTriggerName = "Hit";

    [Header("Opcional: scripts a desactivar al morir")]
    [SerializeField] private MonoBehaviour[] disableOnDeath; // arrastra PlayerMovement, PlayerAttack, etc.

    private void Start()
    {
        currentHealth = maxHealth;
        if (!animator) animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // 🔥 Hit anim
        TrySetTrigger(hitTriggerName);

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // ☠️ Death anim
        TrySetTrigger(deathTriggerName);

        // Desactivar scripts (movimiento, ataque, etc.)
        if (disableOnDeath != null && disableOnDeath.Length > 0)
        {
            foreach (var s in disableOnDeath)
                if (s != null) s.enabled = false;
        }

        // Congelar físicas para que no se caiga
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // Reinicio con fade si existe, si no, reinicia con delay
        if (FadeController.Instance != null)
        {
            // Si tu FadeController reinicia al terminar el fade, perfecto.
            FadeController.Instance.FadeOutAndRestart();
        }
        else
        {
            Invoke(nameof(RestartLevel), restartDelay);
        }
    }

    private void RestartLevel()
    {
        var scn = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scn.name);
    }

    // Solo dispara si el parámetro existe en el Animator
    private void TrySetTrigger(string trigger)
    {
        if (!animator || string.IsNullOrEmpty(trigger)) return;
        if (HasParameter(animator, trigger, AnimatorControllerParameterType.Trigger))
            animator.SetTrigger(trigger);
    }

    private bool HasParameter(Animator anim, string name, AnimatorControllerParameterType type)
    {
        foreach (var p in anim.parameters)
            if (p.type == type && p.name == name) return true;
        return false;
    }
}
