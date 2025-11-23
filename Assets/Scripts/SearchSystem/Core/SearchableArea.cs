using UnityEngine;
using UnityEngine.Events;

namespace GameUtch.SearchSystem
{
    /// <summary>
    /// Component que marca un objeto como investigable
    /// Maneja la detección de proximidad y configuración de búsqueda
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class SearchableArea : MonoBehaviour
    {
        [Header("Configuración de Búsqueda")]
        [Range(0f, 100f)]
        [Tooltip("Probabilidad base de encontrar algo (%)")]
        public float baseSearchChance = 50f;

        [Tooltip("Items que se pueden encontrar en esta área")]
        public ItemData[] possibleItems;

        [Header("Límites")]
        [Tooltip("Tiempo de espera entre búsquedas (segundos). 0 = sin cooldown")]
        public float cooldownTime = 0f;

        [Tooltip("Número máximo de búsquedas. -1 = ilimitado")]
        public int maxSearches = -1;

        [Header("UI/Interacción")]
        [Tooltip("Texto que se muestra al acercarse")]
        public string interactionPrompt = "Presiona E para investigar";

        [Tooltip("Distancia máxima para poder investigar")]
        public float interactionRange = 2f;

        [Header("Visual")]
        [Tooltip("Material para highlight cuando el jugador está cerca")]
        public Material highlightMaterial;

        [Tooltip("Activar highlight automático al acercarse")]
        public bool useHighlight = true;

        [Header("Audio")]
        public AudioClip searchSound;
        public AudioClip successSound;
        public AudioClip failSound;

        [Header("Eventos")]
        public UnityEvent onSearchStart;
        public UnityEvent onSearchSuccess;
        public UnityEvent onSearchFail;

        [Header("Debug")]
        public bool showDebugInfo = false;

        // Estado interno
        private int searchCount = 0;
        private float lastSearchTime = -999f;
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
                Debug.LogWarning($"[SearchableArea] El collider en {gameObject.name} debería ser trigger. Configurando automáticamente.");
                col.isTrigger = true;
            }

            // Obtener renderers para highlight
            renderers = GetComponentsInChildren<Renderer>();
            if (useHighlight && renderers.Length > 0)
            {
                SaveOriginalMaterials();
            }

            // Configurar AudioSource si es necesario
            if (searchSound != null || successSound != null || failSound != null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 1f; // 3D sound
            }

            DebugLog($"Área investigable inicializada: {possibleItems?.Length ?? 0} items posibles");
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
                    controller.SetNearbySearchArea(this);
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
                    controller.ClearNearbySearchArea(this);
                }
            }
        }

        /// <summary>
        /// Intenta realizar una búsqueda
        /// </summary>
        public SearchResult TrySearch()
        {
            // Verificar si se puede buscar
            if (!CanSearch(out string reason))
            {
                DebugLog($"No se puede buscar: {reason}");
                return new SearchResult(false, null, reason);
            }

            // Ejecutar evento y sonido de inicio
            onSearchStart?.Invoke();
            PlaySound(searchSound);

            // Registrar tiempo de búsqueda
            lastSearchTime = Time.time;
            searchCount++;

            DebugLog($"Búsqueda #{searchCount} iniciada");

            // Realizar búsqueda a través del SearchManager
            SearchResult result = SearchManager.Instance.PerformSearch(this);

            // Ejecutar eventos y sonidos según resultado
            if (result.success)
            {
                onSearchSuccess?.Invoke();
                PlaySound(successSound);
            }
            else
            {
                onSearchFail?.Invoke();
                PlaySound(failSound);
            }

            // Destruir el área después de la búsqueda (gameplay de una sola vez)
            DebugLog($"Área '{gameObject.name}' será destruida en 1.5 segundos");
            Destroy(gameObject, 1.5f);

            return result;
        }

        /// <summary>
        /// Verifica si se puede realizar una búsqueda
        /// </summary>
        public bool CanSearch(out string reason)
        {
            // Verificar límite de búsquedas
            if (maxSearches >= 0 && searchCount >= maxSearches)
            {
                reason = "Esta área ya fue investigada completamente";
                return false;
            }

            // Verificar cooldown
            if (cooldownTime > 0f)
            {
                float timeSinceLastSearch = Time.time - lastSearchTime;
                if (timeSinceLastSearch < cooldownTime)
                {
                    float timeRemaining = cooldownTime - timeSinceLastSearch;
                    reason = $"Espera {timeRemaining:F1} segundos antes de volver a investigar";
                    return false;
                }
            }

            // Verificar que el jugador esté en rango
            if (!playerInRange)
            {
                reason = "Debes estar más cerca";
                return false;
            }

            // Verificar que exista SearchManager
            if (SearchManager.Instance == null)
            {
                reason = "Error: SearchManager no encontrado";
                Debug.LogError("[SearchableArea] SearchManager.Instance es null!");
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
        /// Reinicia el estado del área (útil para testing)
        /// </summary>
        public void ResetArea()
        {
            searchCount = 0;
            lastSearchTime = -999f;
            DebugLog("Área reiniciada");
        }

        /// <summary>
        /// Log de debug condicional
        /// </summary>
        private void DebugLog(string message)
        {
            if (showDebugInfo)
            {
                Debug.Log($"[SearchableArea:{gameObject.name}] {message}");
            }
        }

        // Gizmos para visualizar el área en el editor
        void OnDrawGizmosSelected()
        {
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                Gizmos.color = playerInRange ? Color.green : Color.yellow;
                Gizmos.DrawWireSphere(transform.position, interactionRange);
            }
        }

        // Getters públicos para información
        public int GetSearchCount() => searchCount;
        public float GetCooldownRemaining() => Mathf.Max(0f, cooldownTime - (Time.time - lastSearchTime));
        public bool IsPlayerInRange() => playerInRange;
        public int GetRemainingSearches() => maxSearches < 0 ? -1 : maxSearches - searchCount;
    }
}
