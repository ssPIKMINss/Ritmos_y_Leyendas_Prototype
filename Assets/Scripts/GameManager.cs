using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Vidas")]
    [SerializeField] private int startingLives = 3;
    public int CurrentLives { get; private set; }

    // Eventos para que la UI (o audio) se suscriba sin acoplarse
    [System.Serializable] public class IntEvent : UnityEvent<int> { }
    public IntEvent OnLivesChanged;      // se dispara cada vez que cambia CurrentLives
    public UnityEvent OnOutOfLives;      // se dispara cuando las vidas llegan a 0

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentLives = Mathf.Max(0, startingLives);
        OnLivesChanged?.Invoke(CurrentLives);
    }

    // ↓ Llama esto para restar vidas (desde PlayerHealth u otros)
    public void LoseLife(int amount = 1)
    {
        int prev = CurrentLives;
        CurrentLives = Mathf.Max(0, CurrentLives - Mathf.Abs(amount));
        if (CurrentLives != prev) OnLivesChanged?.Invoke(CurrentLives);
        if (CurrentLives <= 0) OnOutOfLives?.Invoke();
    }

    // ↓ Llama esto para sumar vidas (powerups, etc.)
    public void AddLife(int amount = 1)
    {
        if (amount <= 0) return;
        CurrentLives += amount;
        OnLivesChanged?.Invoke(CurrentLives);
    }

    // ↓ Útil para reiniciar partida
    public void ResetLives()
    {
        CurrentLives = Mathf.Max(0, startingLives);
        OnLivesChanged?.Invoke(CurrentLives);
    }

    // ↓ Si quieres cambiar el valor inicial en runtime (menús, dificultad…)
    public void SetStartingLives(int newStarting)
    {
        startingLives = Mathf.Max(0, newStarting);
        ResetLives();
    }
}
