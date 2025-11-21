using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Se llama desde el botón "Jugar"
    public void PlayGame()
    {
        Debug.Log("Cargando juego...");
        // Cambia "GameScene" por el nombre de tu escena de juego
        SceneManager.LoadScene("Level_01");
    }

    // Se llama desde el botón "Salir"
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego");
        // No cerrará en editor, pero sí en build final
        Application.Quit();
    }
}
