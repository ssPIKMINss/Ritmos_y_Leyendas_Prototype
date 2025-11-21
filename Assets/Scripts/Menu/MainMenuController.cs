using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Se llama desde el botón "Jugar"
    public void PlayGame()
    {
        Debug.Log("Cargando juego...");
        StartCoroutine(LoadGameWithDelay());
    }

    private System.Collections.IEnumerator LoadGameWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta el delay si quieres
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
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}

