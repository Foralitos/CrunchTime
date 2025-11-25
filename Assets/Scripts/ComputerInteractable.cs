using UnityEngine;
using UnityEngine.SceneManagement;
using GameUtch.SearchSystem;

/// <summary>
/// Component que marca una computadora como interactiva
/// Permite al jugador iniciar un minijuego de trabajo
/// Patrón similar a SearchableArea pero para transiciones de escena
/// </summary>
[RequireComponent(typeof(Collider))]
public class ComputerInteractable : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    [Tooltip("Texto que se muestra al acercarse")]
    public string interactionPrompt = "Presiona E para trabajar";

    [Tooltip("Distancia máxima para poder interactuar")]
    public float interactionRange = 2f;

    [Header("Mini-Game")]
    [Tooltip("Nombre de la escena del minijuego")]
    public string miniGameSceneName = "WorkMiniGame";

    [Header("Visual")]
    [Tooltip("Material para highlight cuando el jugador está cerca")]
    public Material highlightMaterial;

    [Tooltip("Activar highlight automático al acercarse")]
    public bool useHighlight = true;

    [Header("Audio")]
    public AudioClip interactSound;

    [Header("Debug")]
    public bool showDebugInfo = false;

    // Estado interno
    private bool playerInRange = false;
    private GameObject playerObject;

    // Referencias
    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private AudioSource audioSource;

    void Start()
    {
        // Verificar que el collider sea trigger
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"[ComputerInteractable] El collider en {gameObject.name} debería ser trigger. Configurando automáticamente.");
            col.isTrigger = true;
        }

        // Obtener renderers para highlight
        renderers = GetComponentsInChildren<Renderer>();
        if (useHighlight && renderers.Length > 0)
        {
            SaveOriginalMaterials();
        }

        // Configurar AudioSource si es necesario
        if (interactSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // 3D sound
        }

        DebugLog($"Computadora interactiva inicializada");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerObject = other.gameObject;

            if (useHighlight)
            {
                EnableHighlight(true);
            }

            DebugLog("Jugador entró en rango");

            // Notificar al PlayerSearchController
            PlayerSearchController controller = other.GetComponent<PlayerSearchController>();
            if (controller != null)
            {
                controller.SetNearbyComputer(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerObject = null;

            if (useHighlight)
            {
                EnableHighlight(false);
            }

            DebugLog("Jugador salió de rango");

            // Notificar al PlayerSearchController
            PlayerSearchController controller = other.GetComponent<PlayerSearchController>();
            if (controller != null)
            {
                controller.ClearNearbyComputer(this);
            }
        }
    }

    /// <summary>
    /// Inicia el minijuego de trabajo
    /// Guarda el estado del jugador y carga la escena del minijuego
    /// </summary>
    public void StartWork()
    {
        if (!CanStartWork(out string reason))
        {
            DebugLog($"No se puede iniciar trabajo: {reason}");
            return;
        }

        DebugLog($"Iniciando minijuego: {miniGameSceneName}");

        // Reproducir sonido de interacción
        PlaySound(interactSound);

        // Guardar estado del jugador antes de cambiar de escena
        if (playerObject != null)
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.SavePlayerState(playerObject.transform);
            }
            else
            {
                Debug.LogError("[ComputerInteractable] GameStateManager.Instance es null! Asegúrate de tener un GameStateManager en la escena.");
            }
        }

        // Cargar escena del minijuego
        SceneManager.LoadScene(miniGameSceneName);
    }

    /// <summary>
    /// Verifica si se puede iniciar el trabajo
    /// </summary>
    public bool CanStartWork(out string reason)
    {
        // Verificar que el jugador esté en rango
        if (!playerInRange)
        {
            reason = "Debes estar más cerca";
            return false;
        }

        // Verificar que exista GameStateManager
        if (GameStateManager.Instance == null)
        {
            reason = "Error: GameStateManager no encontrado";
            Debug.LogError("[ComputerInteractable] GameStateManager.Instance es null!");
            return false;
        }

        // Verificar que la escena existe
        if (string.IsNullOrEmpty(miniGameSceneName))
        {
            reason = "Error: Nombre de escena no configurado";
            Debug.LogError("[ComputerInteractable] miniGameSceneName está vacío!");
            return false;
        }

        reason = "";
        return true;
    }

    /// <summary>
    /// Activa o desactiva el highlight visual
    /// </summary>
    private void EnableHighlight(bool enable)
    {
        if (!useHighlight || renderers == null || renderers.Length == 0 || highlightMaterial == null)
            return;

        foreach (Renderer rend in renderers)
        {
            if (rend == null) continue;

            if (enable)
            {
                // Aplicar highlight material
                Material[] mats = new Material[rend.materials.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = highlightMaterial;
                }
                rend.materials = mats;
            }
            else
            {
                // Restaurar materiales originales
                int rendererIndex = System.Array.IndexOf(renderers, rend);
                if (rendererIndex >= 0 && rendererIndex < originalMaterials.Length)
                {
                    rend.materials = originalMaterials[rendererIndex];
                }
            }
        }
    }

    /// <summary>
    /// Guarda los materiales originales para restaurarlos después
    /// </summary>
    private void SaveOriginalMaterials()
    {
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                originalMaterials[i] = renderers[i].materials;
            }
        }
    }

    /// <summary>
    /// Reproduce un sonido si está configurado
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Log de debug condicional
    /// </summary>
    private void DebugLog(string message)
    {
        if (showDebugInfo)
        {
            Debug.Log($"[ComputerInteractable:{gameObject.name}] {message}");
        }
    }

    // Gizmos para visualizar el área en el editor
    void OnDrawGizmosSelected()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = playerInRange ? Color.green : Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }

    // Getters públicos
    public bool IsPlayerInRange() => playerInRange;
    public string GetInteractionPrompt() => interactionPrompt;
}
