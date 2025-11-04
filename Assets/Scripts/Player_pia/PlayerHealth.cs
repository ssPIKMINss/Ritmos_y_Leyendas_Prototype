using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount = 1)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            // cuando la salud llegue a 0:
GameManager.Instance?.LoseLife(1);
// …luego reseteas la salud interna del player si quieres:
            currentHealth = maxHealth;
        }
    }
}
