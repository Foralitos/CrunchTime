using UnityEngine;
using UnityEngine.SceneManagement; // ¡Esta línea es importante para cambiar de escena!

public class MainMenuManager : MonoBehaviour
{
    // Esta función se llamará cuando el jugador presione el botón de "Start"
    public void StartGame()
    {
        // Carga la escena que se llama "Tutorial Level"
        // ¡Asegúrate de que el nombre coincida exactamente!
        SceneManager.LoadScene("Tutorial Level");
    }

    // Esta función se llamará cuando el jugador presione el botón de "Quit"
    public void QuitGame()
    {
        // Esto solo cierra el juego cuando está compilado, no en el editor.
        // En el editor, solo verás este mensaje en la consola.
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}