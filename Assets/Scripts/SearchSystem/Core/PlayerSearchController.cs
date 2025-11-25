using UnityEngine;
using UnityEngine.InputSystem;

namespace GameUtch.SearchSystem
{
    /// <summary>
    /// Controla la interacción del jugador con áreas investigables
    /// Se integra con el Input System y ThirdPersonController
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerSearchController : MonoBehaviour
    {
        [Header("Referencias")]
        [Tooltip("Referencia al SearchUI (se busca automáticamente si está vacío)")]
        public SearchUI searchUI;

        [Header("Configuración")]
        [Tooltip("Prevenir búsquedas mientras el jugador está en movimiento")]
        public bool preventSearchWhileMoving = false;

        [Tooltip("Velocidad mínima considerada como 'en movimiento'")]
        public float movementThreshold = 0.1f;

        [Header("Debug")]
        public bool showDebugInfo = false;

        // Estado
        private SearchableArea currentNearbyArea = null;
        private PlayerInput playerInput;
        private CharacterController characterController;
        private bool isSearching = false;

        void Start()
        {
            // Obtener referencias
            playerInput = GetComponent<PlayerInput>();
            characterController = GetComponent<CharacterController>();

            // Buscar SearchUI si no está asignado
            if (searchUI == null)
            {
                searchUI = FindFirstObjectByType<SearchUI>();
                if (searchUI == null)
                {
                    Debug.LogWarning("[PlayerSearchController] No se encontró SearchUI en la escena. Los mensajes no se mostrarán.");
                }
            }

            // Suscribirse al input action "Interact"
            SetupInputActions();

            DebugLog("PlayerSearchController inicializado");
        }

        void OnDestroy()
        {
            // Desuscribirse de eventos
            if (playerInput != null)
            {
                var interactAction = playerInput.actions.FindAction("Interact");
                if (interactAction != null)
                {
                    interactAction.performed -= OnInteractPerformed;
                }
            }
        }

        /// <summary>
        /// Configura las acciones de input
        /// </summary>
        private void SetupInputActions()
        {
            if (playerInput == null)
            {
                Debug.LogError("[PlayerSearchController] PlayerInput no encontrado!");
                return;
            }

            // Buscar la acción "Interact" (tecla E por defecto)
            var interactAction = playerInput.actions.FindAction("Interact");
            if (interactAction != null)
            {
                interactAction.performed += OnInteractPerformed;
                DebugLog("Input action 'Interact' configurado");
            }
            else
            {
                Debug.LogError("[PlayerSearchController] Input action 'Interact' no encontrado en el Input System!");
            }
        }

        /// <summary>
        /// Callback cuando se presiona el botón de interacción
        /// </summary>
        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            if (isSearching)
            {
                DebugLog("Ya hay una búsqueda en proceso");
                return;
            }

            TrySearch();
        }

        /// <summary>
        /// Intenta realizar una búsqueda en el área cercana
        /// </summary>
        public void TrySearch()
        {
            // Verificar que hay un área cercana y que no ha sido destruida
            if (currentNearbyArea == null || currentNearbyArea.gameObject == null)
            {
                DebugLog("No hay área investigable cercana");
                if (currentNearbyArea != null)
                {
                    // El área fue destruida, limpiar la referencia
                    currentNearbyArea = null;
                }
                return;
            }

            // Verificar movimiento si está configurado
            if (preventSearchWhileMoving && IsPlayerMoving())
            {
                ShowMessage("No puedes investigar mientras te mueves");
                return;
            }

            // Verificar si se puede buscar
            if (!currentNearbyArea.CanSearch(out string reason))
            {
                ShowMessage(reason);
                return;
            }

            // Realizar la búsqueda
            PerformSearch();
        }

        /// <summary>
        /// Ejecuta la búsqueda
        /// </summary>
        private void PerformSearch()
        {
            isSearching = true;
            DebugLog($"Buscando en: {currentNearbyArea.gameObject.name}");

            // Realizar búsqueda a través del área
            SearchResult result = currentNearbyArea.TrySearch();

            // Procesar resultado
            if (result.success && result.foundItem != null)
            {
                // Item encontrado - agregarlo al inventario
                bool added = InventorySystem.Instance?.AddItem(result.foundItem) ?? false;

                if (added)
                {
                    ShowMessage(result.message, result.foundItem.GetRarityColor());
                    DebugLog($"Item encontrado y agregado: {result.foundItem.itemName}");
                }
                else
                {
                    ShowMessage("Inventario lleno");
                    DebugLog("Inventario lleno - no se pudo agregar item");
                }
            }
            else
            {
                // No se encontró nada
                ShowMessage(result.message);
                DebugLog("No se encontró nada");
            }

            // Ocultar prompt inmediatamente (no podemos confiar en OnTriggerExit cuando el área se destruye)
            if (searchUI != null)
            {
                searchUI.HideInteractionPrompt();
            }

            // Limpiar referencia al área (será destruida en 1.5 segundos)
            currentNearbyArea = null;

            isSearching = false;
        }

        /// <summary>
        /// Verifica si el jugador está en movimiento
        /// </summary>
        private bool IsPlayerMoving()
        {
            if (characterController == null)
                return false;

            Vector3 velocity = characterController.velocity;
            velocity.y = 0; // Ignorar velocidad vertical
            return velocity.magnitude > movementThreshold;
        }

        /// <summary>
        /// Muestra un mensaje en la UI
        /// </summary>
        private void ShowMessage(string message, Color? color = null)
        {
            if (searchUI != null)
            {
                searchUI.ShowSearchResult(message, color ?? Color.white);
            }
            else
            {
                DebugLog($"Mensaje: {message}");
            }
        }

        /// <summary>
        /// Establece el área cercana actual
        /// </summary>
        public void SetNearbySearchArea(SearchableArea area)
        {
            if (currentNearbyArea == area)
                return;

            currentNearbyArea = area;

            // Mostrar prompt de interacción
            if (searchUI != null && area != null)
            {
                searchUI.ShowInteractionPrompt(area.interactionPrompt);
            }

            DebugLog($"Área cercana establecida: {area?.gameObject.name ?? "null"}");
        }

        /// <summary>
        /// Limpia el área cercana si es la actual
        /// </summary>
        public void ClearNearbySearchArea(SearchableArea area)
        {
            if (currentNearbyArea == area)
            {
                currentNearbyArea = null;

                // Ocultar prompt de interacción
                if (searchUI != null)
                {
                    searchUI.HideInteractionPrompt();
                }

                DebugLog("Área cercana limpiada");
            }
        }

        /// <summary>
        /// Log de debug condicional
        /// </summary>
        private void DebugLog(string message)
        {
            if (showDebugInfo)
            {
                Debug.Log($"[PlayerSearchController] {message}");
            }
        }

        // Getters públicos
        public SearchableArea GetCurrentArea() => currentNearbyArea;
        public bool IsSearching() => isSearching;
    }
}
