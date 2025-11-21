using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    public VideoPlayer introVideo;     // Asigna tu intro.mp4 aquí (opcional)
    public GameObject menuUI;          // Canvas del menú (botones, logo, etc.)
    private bool introTerminada = false;

    void Start()
    {
        if (introVideo != null)
        {
            menuUI.SetActive(false); // Oculta el menú mientras corre la intro
            introVideo.loopPointReached += OnIntroFinish;
        }
        else
        {
            menuUI.SetActive(true);
        }
    }

    void OnIntroFinish(VideoPlayer vp)
    {
        introTerminada = true;
        menuUI.SetActive(true);
    }

    // ---- Botones ----
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1"); // Cambia por el nombre de tu escena del juego
    }

    public void Opciones()
    {
        // Aquí puedes abrir un panel con configuraciones de sonido o controles
        Debug.Log("Abriendo opciones...");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
