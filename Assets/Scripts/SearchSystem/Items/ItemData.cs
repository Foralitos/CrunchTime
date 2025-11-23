using UnityEngine;

namespace GameUtch.SearchSystem
{
    /// <summary>
    /// Tipos de items que se pueden encontrar
    /// </summary>
    public enum ItemType
    {
        Evidence,   // Evidencia
        Document,   // Documentos
        Tool,       // Herramientas
        Key,        // Llaves
        Misc        // Misceláneos
    }

    /// <summary>
    /// Rareza del item (afecta probabilidad de encontrarlo)
    /// </summary>
    public enum ItemRarity
    {
        Common,     // Común (100%)
        Uncommon,   // Poco común (70%)
        Rare        // Raro (40%)
    }

    /// <summary>
    /// ScriptableObject que define un item encontrable
    /// </summary>
    [CreateAssetMenu(fileName = "NewItem", menuName = "SearchSystem/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Información del Item")]
        public string itemName = "Nuevo Item";
        [TextArea(3, 5)]
        public string itemDescription = "Descripción del item";
        public Sprite itemIcon;

        [Header("Clasificación")]
        public ItemType itemType = ItemType.Misc;
        public ItemRarity rarity = ItemRarity.Common;

        [Header("Gameplay")]
        public bool isQuestItem = false;
        public string questID = "";

        /// <summary>
        /// Obtiene el modificador de rareza para cálculos de probabilidad
        /// </summary>
        public float GetRarityModifier()
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                    return 1.0f;
                case ItemRarity.Uncommon:
                    return 0.7f;
                case ItemRarity.Rare:
                    return 0.4f;
                default:
                    return 1.0f;
            }
        }

        /// <summary>
        /// Obtiene el color asociado a la rareza
        /// </summary>
        public Color GetRarityColor()
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                    return Color.white;
                case ItemRarity.Uncommon:
                    return new Color(0.2f, 0.8f, 0.2f); // Verde
                case ItemRarity.Rare:
                    return new Color(0.5f, 0.3f, 1.0f); // Púrpura
                default:
                    return Color.white;
            }
        }
    }
}
