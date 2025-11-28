using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Animator anim;
    public AudioSource fadeAudioSource; // <-- asigna aquí el Audio Source del FadePanel

    // Se llama desde el botón "Jugar"
    public void PlayGame()
    {
        Debug.Log("Cargando juego...");
        StartCoroutine(LoadGameWithDelay());
    }

    private System.Collections.IEnumerator LoadGameWithDelay()
    {
        anim.SetTrigger("FadeOut");

        // --- Reproduce el sonido del fade ---
        if (fadeAudioSource != null)
            fadeAudioSource.Play();

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Level_01");
    }

    // Se llama desde el botón "Salir"
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego");
        StartCoroutine(QuitWithDelay());
    }

    private System.Collections.IEnumerator QuitWithDelay()
    {
        // --- También puedes reproducir el mismo sonido al salir (opcional) ---
        if (fadeAudioSource != null)
            fadeAudioSource.Play();

        yield return new WaitForSeconds(0.5f);

        Application.Quit();
    }
}
