using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance { get; private set; }

    [Header("Fade")]
    [SerializeField] private Image fadeImage;     // Image negro a pantalla completa
    [SerializeField] private float fadeDuration = 4f; // segundos (fade out)
    [SerializeField] private float fadeInDuration = 0.75f; // segundos (al empezar escena)

    private void Awake()
    {
        // Garantiza singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Asegura que el objeto raíz sobreviva al cambio de escena
        DontDestroyOnLoad(transform.root.gameObject);

        // Fade-in al inicio de cualquier escena
        if (fadeImage != null)
        {
            var c = fadeImage.color;
            c.a = 1f;                // empieza negro
            fadeImage.color = c;
            StartCoroutine(FadeTo(0f, fadeInDuration)); // y aclara
        }
    }

    public void FadeOutAndRestart()
    {
        if (fadeImage == null) { RestartNow(); return; }
        StartCoroutine(FadeOutThenReload());
    }

    private System.Collections.IEnumerator FadeOutThenReload()
    {
        yield return FadeTo(1f, fadeDuration); // negro en 4s
        RestartNow();
    }

    private void RestartNow()
    {
        var scn = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scn.name);
    }

    // Interpola alfa del Image
    private System.Collections.IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // usa tiempo no escalado (por si hay pausa)
            float a = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            var c = fadeImage.color; c.a = a; fadeImage.color = c;
            yield return null;
        }

        var cfinal = fadeImage.color; cfinal.a = targetAlpha; fadeImage.color = cfinal;
    }
}
