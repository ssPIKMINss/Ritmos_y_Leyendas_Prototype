using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroSequenceController : MonoBehaviour
{
    [Header("Video Players")]
    public VideoPlayer videoIntro;     // Se reproduce una vez
    public VideoPlayer videoLoop;      // Se repite en bucle

    [Header("UI")]
    public GameObject menuUI;          // Menú interactivo (botones)
    public GameObject clickText;       // Texto tipo "Haz click para continuar"

    private bool puedeContinuar = false;
    private bool menuActivo = false;

    void Start()
    {
        if (menuUI != null)
            menuUI.SetActive(false);

        if (clickText != null)
            clickText.SetActive(false);

        // Cuando termina la intro, pasamos al loop
        videoIntro.loopPointReached += OnIntroFinish;

        // Inicia la intro
        videoIntro.Play();
    }

    void OnIntroFinish(VideoPlayer vp)
    {
        // Desactiva la intro y activa el loop
        videoIntro.gameObject.SetActive(false);

        videoLoop.gameObject.SetActive(true);
        videoLoop.Play();

        if (clickText != null)
            clickText.SetActive(true);

        puedeContinuar = true;
    }

    void Update()
    {
        // Esperar el click del jugador cuando ya puede continuar
        if (puedeContinuar && !menuActivo && Input.GetMouseButtonDown(0))
        {
            MostrarMenu();
        }
    }

    void MostrarMenu()
    {
        menuActivo = true;
        videoLoop.Pause();

        if (clickText != null)
            clickText.SetActive(false);

        if (menuUI != null)
            menuUI.SetActive(true);
    }

    // ---- Botones del menú ----
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
