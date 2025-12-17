using UnityEngine;
using UnityEngine.SceneManagement;

public class TraukoBossHealth : MonoBehaviour
{
    [Header("Vida del Boss")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    private bool isDead = false;

    [Header("Animación")]
    [SerializeField] private Animator animator;
    [SerializeField] private string hitTrigger = "Hit";
    [SerializeField] private string deathTrigger = "Death";

    [Header("Cambio de escena")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private float delayBeforeSceneChange = 2f;

    private void Awake()
    {
        currentHealth = maxHealth;
        if (!animator) animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (animator && !string.IsNullOrEmpty(hitTrigger))
            animator.SetTrigger(hitTrigger);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Animación de muerte
        if (animator && !string.IsNullOrEmpty(deathTrigger))
            animator.SetTrigger(deathTrigger);

        // Desactiva colisiones
        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        // Desactiva IA / ritmo / ataques
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
        {
            if (s != this)
                s.enabled = false;
        }

        // Cambio de escena tras delay
        Invoke(nameof(LoadNextScene), delayBeforeSceneChange);
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogError("TraukoBossHealth → No se asignó la escena destino");
    }
}
