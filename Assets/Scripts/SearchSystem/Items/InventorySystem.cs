using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GameUtch.SearchSystem
{
    /// <summary>
    /// Sistema simple de inventario para almacenar items encontrados
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem Instance { get; private set; }

        [Header("Configuración")]
        [Tooltip("Capacidad máxima del inventario. -1 = ilimitado")]
        public int maxCapacity = 50;

        [Tooltip("Permitir items duplicados")]
        public bool allowDuplicates = true;

        [Header("Debug")]
        public bool showDebugLogs = true;

        // Almacenamiento de items
        private List<ItemData> items = new List<ItemData>();

        // Eventos para notificar cambios en el inventario
        public System.Action<ItemData> OnItemAdded;
        public System.Action<ItemData> OnItemRemoved;
        public System.Action OnInventoryChanged;

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

            LogDebug("InventorySystem inicializado");
        }

        /// <summary>
        /// Agrega un item al inventario
        /// </summary>
        public bool AddItem(ItemData item)
        {
            if (item == null)
            {
                LogDebug("Error: Intentando agregar item null");
                return false;
            }

            // Verificar capacidad
            if (maxCapacity >= 0 && items.Count >= maxCapacity)
            {
                LogDebug($"Inventario lleno ({items.Count}/{maxCapacity})");
                return false;
            }

            // Verificar duplicados si no están permitidos
            if (!allowDuplicates && HasItem(item))
            {
                LogDebug($"Item duplicado no permitido: {item.itemName}");
                return false;
            }

            // Agregar item
            items.Add(item);
            LogDebug($"Item agregado: {item.itemName} ({items.Count}/{(maxCapacity < 0 ? "∞" : maxCapacity.ToString())})");

            // Notificar eventos
            OnItemAdded?.Invoke(item);
            OnInventoryChanged?.Invoke();

            return true;
        }

        /// <summary>
        /// Remueve un item del inventario
        /// </summary>
        public bool RemoveItem(ItemData item)
        {
            if (item == null || !items.Contains(item))
            {
                LogDebug($"Item no encontrado en inventario: {item?.itemName ?? "null"}");
                return false;
            }

            items.Remove(item);
            LogDebug($"Item removido: {item.itemName}");

            // Notificar eventos
            OnItemRemoved?.Invoke(item);
            OnInventoryChanged?.Invoke();

            return true;
        }

        /// <summary>
        /// Remueve todos los items de un tipo específico
        /// </summary>
        public int RemoveItemsByType(ItemType type)
        {
            int removedCount = items.RemoveAll(item => item.itemType == type);

            if (removedCount > 0)
            {
                LogDebug($"Removidos {removedCount} items de tipo {type}");
                OnInventoryChanged?.Invoke();
            }

            return removedCount;
        }

        /// <summary>
        /// Verifica si el inventario contiene un item específico
        /// </summary>
        public bool HasItem(ItemData item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// Verifica si el inventario contiene items de un tipo específico
        /// </summary>
        public bool HasItemOfType(ItemType type)
        {
            return items.Any(item => item.itemType == type);
        }

        /// <summary>
        /// Obtiene la cantidad de un item específico
        /// </summary>
        public int GetItemCount(ItemData item)
        {
            return items.Count(i => i == item);
        }

        /// <summary>
        /// Obtiene la cantidad de items de un tipo
        /// </summary>
        public int GetItemCountByType(ItemType type)
        {
            return items.Count(item => item.itemType == type);
        }

        /// <summary>
        /// Obtiene todos los items del inventario
        /// </summary>
        public List<ItemData> GetAllItems()
        {
            return new List<ItemData>(items);
        }

        /// <summary>
        /// Obtiene todos los items de un tipo específico
        /// </summary>
        public List<ItemData> GetItemsByType(ItemType type)
        {
            return items.Where(item => item.itemType == type).ToList();
        }

        /// <summary>
        /// Obtiene todos los items de una rareza específica
        /// </summary>
        public List<ItemData> GetItemsByRarity(ItemRarity rarity)
        {
            return items.Where(item => item.rarity == rarity).ToList();
        }

        /// <summary>
        /// Limpia todo el inventario
        /// </summary>
        public void ClearInventory()
        {
            int count = items.Count;
            items.Clear();
            LogDebug($"Inventario limpiado ({count} items removidos)");

            OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// Obtiene el número de items en el inventario
        /// </summary>
        public int GetItemCount()
        {
            return items.Count;
        }

        /// <summary>
        /// Obtiene el espacio disponible en el inventario
        /// </summary>
        public int GetAvailableSpace()
        {
            if (maxCapacity < 0)
                return int.MaxValue;

            return maxCapacity - items.Count;
        }

        /// <summary>
        /// Verifica si el inventario está lleno
        /// </summary>
        public bool IsFull()
        {
            if (maxCapacity < 0)
                return false;

            return items.Count >= maxCapacity;
        }

        /// <summary>
        /// Verifica si el inventario está vacío
        /// </summary>
        public bool IsEmpty()
        {
            return items.Count == 0;
        }

        /// <summary>
        /// Obtiene estadísticas del inventario
        /// </summary>
        public string GetInventoryStats()
        {
            int commonCount = GetItemsByRarity(ItemRarity.Common).Count;
            int uncommonCount = GetItemsByRarity(ItemRarity.Uncommon).Count;
            int rareCount = GetItemsByRarity(ItemRarity.Rare).Count;

            return $"Inventario ({items.Count}/{(maxCapacity < 0 ? "∞" : maxCapacity.ToString())})\n" +
                   $"Comunes: {commonCount}\n" +
                   $"Poco Comunes: {uncommonCount}\n" +
                   $"Raros: {rareCount}";
        }

        /// <summary>
        /// Busca un item por nombre
        /// </summary>
        public ItemData FindItemByName(string name)
        {
            return items.FirstOrDefault(item => item.itemName.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Busca items que contengan un texto en su nombre
        /// </summary>
        public List<ItemData> SearchItemsByName(string searchText)
        {
            return items.Where(item => item.itemName.ToLower().Contains(searchText.ToLower())).ToList();
        }

        /// <summary>
        /// Obtiene items de quest si los hay
        /// </summary>
        public List<ItemData> GetQuestItems()
        {
            return items.Where(item => item.isQuestItem).ToList();
        }

        /// <summary>
        /// Log de debug condicional
        /// </summary>
        private void LogDebug(string message)
        {
            if (showDebugLogs)
            {
                Debug.Log($"[InventorySystem] {message}");
            }
        }

        /// <summary>
        /// Debug: Imprime todos los items del inventario
        /// </summary>
        [ContextMenu("Print Inventory")]
        public void PrintInventory()
        {
            Debug.Log("=== INVENTARIO ===");
            Debug.Log(GetInventoryStats());
            Debug.Log("\nItems:");

            foreach (var item in items)
            {
                Debug.Log($"- {item.itemName} ({item.rarity}) [{item.itemType}]");
            }

            Debug.Log("==================");
        }
    }
}
