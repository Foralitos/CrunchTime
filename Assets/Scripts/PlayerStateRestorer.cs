using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script que se agrega al jugador para restaurar su posición
/// cuando regresa de un minijuego a la escena principal
/// </summary>
public class PlayerStateRestorer : MonoBehaviour
{
    [Header("Debug")]
    public bool showDebugInfo = false;

    void Start()
    {
        // Verificar si acabamos de regresar de un minijuego
        if (GameStateManager.Instance != null)
        {
            if (GameStateManager.Instance.ShouldRestorePlayer())
            {
                // Restaurar posición del jugador
                GameStateManager.Instance.RestorePlayerState(gameObject);
                DebugLog("Posición del jugador restaurada después del minijuego");
            }
            else
            {
                DebugLog("No hay estado guardado para restaurar");
            }
        }
        else
        {
            DebugLog("GameStateManager no encontrado - no se restaurará posición");
        }
    }

    /// <summary>
    /// Log de debug condicional
    /// </summary>
    private void DebugLog(string message)
    {
        if (showDebugInfo)
        {
            Debug.Log($"[PlayerStateRestorer] {message}");
        }
    }
}
