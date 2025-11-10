using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida del jugador")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    private bool isDead;

    [Header("Animación (opcional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string deathTriggerName = "Death";
    [SerializeField] private string hitTriggerName = "Hit";

    private void Start()
    {
        currentHealth = maxHealth;
        if (!animator) animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        TrySetTrigger(hitTriggerName);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        TrySetTrigger(deathTriggerName);

        if (FadeController.Instance != null)
            FadeController.Instance.FadeOutAndRestart();
        else
            Invoke(nameof(RestartLevel), 1f);
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
