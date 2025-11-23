using UnityEngine;
using System.Collections.Generic;

namespace GameUtch.SearchSystem
{
    /// <summary>
    /// Resultado de una búsqueda
    /// </summary>
    public class SearchResult
    {
        public bool success;
        public ItemData foundItem;
        public string message;

        public SearchResult(bool success, ItemData item = null, string message = "")
        {
            this.success = success;
            this.foundItem = item;
            this.message = message;
        }
    }

    /// <summary>
    /// Singleton que maneja la lógica de búsqueda y probabilidades
    /// </summary>
    public class SearchManager : MonoBehaviour
    {
        public static SearchManager Instance { get; private set; }

        [Header("Configuración de Búsqueda")]
        [Range(0f, 100f)]
        [Tooltip("Probabilidad base mínima después de múltiples búsquedas")]
        public float minimumSearchChance = 20f;

        [Range(0f, 0.3f)]
        [Tooltip("Penalización por cada búsqueda adicional en la misma área")]
        public float searchCountPenalty = 0.15f;

        [Header("Mensajes")]
        public string noItemFoundMessage = "No encontraste nada";
        public string itemFoundPrefix = "¡Encontraste: ";
        public string itemFoundSuffix = "!";

        [Header("Debug")]
        public bool showDebugLogs = true;

        // Registro de áreas buscadas
        private Dictionary<string, int> searchedAreas = new Dictionary<string, int>();

        void Awake()
        {
            // Implementación del patrón Singleton
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Realiza una búsqueda en un área con una lista de items posibles
        /// </summary>
        public SearchResult PerformSearch(SearchableArea area)
        {
            if (area == null)
            {
                LogDebug("Error: SearchableArea es null");
                return new SearchResult(false, null, "Error en la búsqueda");
            }

            // Obtener número de búsquedas previas en esta área
            string areaID = area.GetInstanceID().ToString();
            int previousSearches = GetSearchCount(areaID);

            // Calcular probabilidad final
            float finalChance = CalculateFinalSearchChance(area.baseSearchChance, previousSearches);

            LogDebug($"Búsqueda en {area.gameObject.name}:\n" +
                     $"- Probabilidad base: {area.baseSearchChance}%\n" +
                     $"- Búsquedas previas: {previousSearches}\n" +
                     $"- Probabilidad final: {finalChance}%");

            // Registrar la búsqueda
            IncrementSearchCount(areaID);

            // Tirar el dado
            float roll = Random.Range(0f, 100f);

            if (roll <= finalChance)
            {
                // ¡Éxito! Seleccionar un item aleatorio
                ItemData selectedItem = SelectRandomItem(area);

                if (selectedItem != null)
                {
                    string message = itemFoundPrefix + selectedItem.itemName + itemFoundSuffix;
                    LogDebug($"¡Éxito! (roll: {roll:F1} <= {finalChance:F1}%) - Item: {selectedItem.itemName}");
                    return new SearchResult(true, selectedItem, message);
                }
            }

            LogDebug($"Fracaso (roll: {roll:F1} > {finalChance:F1}%)");
            return new SearchResult(false, null, noItemFoundMessage);
        }

        /// <summary>
        /// Calcula la probabilidad final de encontrar algo
        /// </summary>
        private float CalculateFinalSearchChance(float baseChance, int previousSearches)
        {
            // Aplicar penalización por búsquedas repetidas
            float penalty = 1.0f - (previousSearches * searchCountPenalty);
            penalty = Mathf.Max(minimumSearchChance / 100f, penalty);

            float finalChance = baseChance * penalty;
            finalChance = Mathf.Max(minimumSearchChance, finalChance);

            return finalChance;
        }

        /// <summary>
        /// Selecciona un item aleatorio de la lista de items posibles
        /// Considera la rareza del item en la selección
        /// </summary>
        private ItemData SelectRandomItem(SearchableArea area)
        {
            if (area.possibleItems == null || area.possibleItems.Length == 0)
            {
                LogDebug("No hay items configurados en esta área");
                return null;
            }

            // Crear lista ponderada de items basada en rareza
            List<ItemData> weightedItems = new List<ItemData>();

            foreach (ItemData item in area.possibleItems)
            {
                if (item == null) continue;

                // Agregar el item múltiples veces según su rareza
                // Common: 10 veces, Uncommon: 7 veces, Rare: 4 veces
                int weight = GetItemWeight(item.rarity);
                for (int i = 0; i < weight; i++)
                {
                    weightedItems.Add(item);
                }
            }

            if (weightedItems.Count == 0)
            {
                return null;
            }

            // Seleccionar aleatoriamente de la lista ponderada
            int randomIndex = Random.Range(0, weightedItems.Count);
            return weightedItems[randomIndex];
        }

        /// <summary>
        /// Obtiene el peso de un item según su rareza para la selección aleatoria
        /// </summary>
        private int GetItemWeight(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                    return 10;
                case ItemRarity.Uncommon:
                    return 7;
                case ItemRarity.Rare:
                    return 4;
                default:
                    return 10;
            }
        }

        /// <summary>
        /// Obtiene el número de veces que se ha buscado en un área
        /// </summary>
        private int GetSearchCount(string areaID)
        {
            if (searchedAreas.ContainsKey(areaID))
            {
                return searchedAreas[areaID];
            }
            return 0;
        }

        /// <summary>
        /// Incrementa el contador de búsquedas en un área
        /// </summary>
        private void IncrementSearchCount(string areaID)
        {
            if (searchedAreas.ContainsKey(areaID))
            {
                searchedAreas[areaID]++;
            }
            else
            {
                searchedAreas[areaID] = 1;
            }
        }

        /// <summary>
        /// Reinicia el contador de búsquedas (útil para testing o reset de nivel)
        /// </summary>
        public void ResetSearchCounts()
        {
            searchedAreas.Clear();
            LogDebug("Contadores de búsqueda reiniciados");
        }

        /// <summary>
        /// Log de debug condicional
        /// </summary>
        private void LogDebug(string message)
        {
            if (showDebugLogs)
            {
                Debug.Log($"[SearchManager] {message}");
            }
        }
    }
}
