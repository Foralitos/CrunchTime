using UnityEngine;

/// <summary>
/// Singleton que persiste entre escenas para guardar y restaurar el estado del jugador
/// Usado para transiciones a minijuegos y regreso a la escena principal
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("Player State")]
    public Vector3 savedPlayerPosition;
    public Quaternion savedPlayerRotation;

    [Header("Scene Management")]
    public string returnSceneName = "Tutorial Level";
    public bool hasReturnedFromMiniGame = false;

    [Header("Debug")]
    public bool showDebugInfo = false;

    void Awake()
    {
        // Singleton pattern - solo una instancia persiste
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        DebugLog("GameStateManager inicializado y persistente entre escenas");
    }

    /// <summary>
    /// Guarda la posición y rotación actual del jugador antes de cambiar de escena
    /// </summary>
    public void SavePlayerState(Transform playerTransform)
    {
        if (playerTransform == null)
        {
            Debug.LogError("[GameStateManager] playerTransform es null!");
            return;
        }

        savedPlayerPosition = playerTransform.position;
        savedPlayerRotation = playerTransform.rotation;
        hasReturnedFromMiniGame = false;

        DebugLog($"Estado del jugador guardado - Pos: {savedPlayerPosition}, Rot: {savedPlayerRotation.eulerAngles}");
    }

    /// <summary>
    /// Restaura la posición y rotación guardada del jugador
    /// Debe ser llamado después de regresar de un minijuego
    /// </summary>
    public void RestorePlayerState(GameObject player)
    {
        if (player == null)
        {
            Debug.LogWarning("[GameStateManager] Player GameObject es null, no se puede restaurar estado");
            return;
        }

        // Desactivar CharacterController temporalmente para poder cambiar la posición
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }

        // Restaurar posición y rotación
        player.transform.position = savedPlayerPosition;
        player.transform.rotation = savedPlayerRotation;

        // Reactivar CharacterController
        if (cc != null)
        {
            cc.enabled = true;
        }

        hasReturnedFromMiniGame = true;
        DebugLog($"Estado del jugador restaurado - Pos: {savedPlayerPosition}, Rot: {savedPlayerRotation.eulerAngles}");
    }

    /// <summary>
    /// Resetea el estado guardado (útil para testing)
    /// </summary>
    public void ResetState()
    {
        savedPlayerPosition = Vector3.zero;
        savedPlayerRotation = Quaternion.identity;
        hasReturnedFromMiniGame = false;
        DebugLog("Estado reseteado");
    }

    /// <summary>
    /// Log condicional basado en showDebugInfo
    /// </summary>
    private void DebugLog(string message)
    {
        if (showDebugInfo)
        {
            Debug.Log($"[GameStateManager] {message}");
        }
    }

    // Getters públicos
    public bool HasSavedState() => savedPlayerPosition != Vector3.zero;
    public bool ShouldRestorePlayer() => hasReturnedFromMiniGame == false && HasSavedState();
}
