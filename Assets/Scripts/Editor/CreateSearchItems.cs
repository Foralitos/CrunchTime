using UnityEngine;
using UnityEditor;
using GameUtch.SearchSystem;

/// <summary>
/// Script de editor para crear items de ejemplo del sistema de búsqueda
/// </summary>
public class CreateSearchItems
{
    [MenuItem("Tools/SearchSystem/Create Example Items")]
    public static void CreateExampleItems()
    {
        // Asegurar que la carpeta existe
        if (!AssetDatabase.IsValidFolder("Assets/Items"))
        {
            AssetDatabase.CreateFolder("Assets", "Items");
        }

        // Llave de Oficina - Common
        CreateItem("LlaveOficina", "Llave de Oficina",
            "Una llave común de oficina. Podría abrir algún cajón o armario.",
            ItemType.Key, ItemRarity.Common);

        // Llave Maestra - Rare
        CreateItem("LlaveMaestra", "Llave Maestra",
            "Una llave especial que abre múltiples puertas importantes.",
            ItemType.Key, ItemRarity.Rare);

        // Informe Policial - Common
        CreateItem("InformePolicial", "Informe Policial",
            "Un informe estándar de rutina policial.",
            ItemType.Document, ItemRarity.Common);

        // Expediente Clasificado - Uncommon
        CreateItem("ExpedienteClasificado", "Expediente Clasificado",
            "Un expediente con información sensible. Acceso restringido.",
            ItemType.Document, ItemRarity.Uncommon);

        // Evidencia Crítica - Rare
        CreateItem("EvidenciaCritica", "Evidencia Crítica",
            "Evidencia crucial para el caso. ¡Un hallazgo importante!",
            ItemType.Document, ItemRarity.Rare);

        // Linterna - Common
        CreateItem("Linterna", "Linterna",
            "Una linterna estándar. Útil para áreas oscuras.",
            ItemType.Tool, ItemRarity.Common);

        // Radio Portátil - Uncommon
        CreateItem("RadioPortatil", "Radio Portátil",
            "Un radio de comunicación policial. Aún funciona.",
            ItemType.Tool, ItemRarity.Uncommon);

        // Huella Digital - Rare
        CreateItem("HuellaDigital", "Huella Digital",
            "Una huella dactilar preservada. Evidencia forense valiosa.",
            ItemType.Evidence, ItemRarity.Rare);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[CreateSearchItems] ¡8 items de ejemplo creados exitosamente en Assets/Items/!");
    }

    private static void CreateItem(string fileName, string itemName, string description,
                                   ItemType type, ItemRarity rarity)
    {
        string path = $"Assets/Items/{fileName}.asset";

        // Verificar si ya existe
        ItemData existing = AssetDatabase.LoadAssetAtPath<ItemData>(path);
        if (existing != null)
        {
            Debug.LogWarning($"[CreateSearchItems] Item ya existe: {path}");
            return;
        }

        // Crear el ScriptableObject
        ItemData item = ScriptableObject.CreateInstance<ItemData>();
        item.itemName = itemName;
        item.itemDescription = description;
        item.itemType = type;
        item.rarity = rarity;
        item.isQuestItem = false;

        // Guardar el asset
        AssetDatabase.CreateAsset(item, path);
        Debug.Log($"[CreateSearchItems] Creado: {itemName} ({rarity}) en {path}");
    }
}
