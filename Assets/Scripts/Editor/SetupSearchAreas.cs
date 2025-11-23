using UnityEngine;
using UnityEditor;
using GameUtch.SearchSystem;
using System.Linq;

/// <summary>
/// Script de editor para configurar áreas investigables en el police_office
/// </summary>
public class SetupSearchAreas
{
    [MenuItem("Tools/SearchSystem/Setup Search Areas in police_office")]
    public static void SetupPoliceOfficeSearchAreas()
    {
        // Buscar el police_office
        GameObject policeOffice = GameObject.Find("police_office");
        if (policeOffice == null)
        {
            Debug.LogError("[SetupSearchAreas] No se encontró 'police_office' en la escena!");
            return;
        }

        Debug.Log("[SetupSearchAreas] Configurando áreas investigables en police_office...");

        // Cargar todos los items disponibles
        ItemData[] allItems = LoadAllItems();
        if (allItems.Length == 0)
        {
            Debug.LogWarning("[SetupSearchAreas] No se encontraron items. Ejecuta 'Create Example Items' primero.");
            return;
        }

        Debug.Log($"[SetupSearchAreas] {allItems.Length} items encontrados");

        // Buscar objetos candidatos en los hijos de police_office
        Transform[] allChildren = policeOffice.GetComponentsInChildren<Transform>(true);
        Debug.Log($"[SetupSearchAreas] {allChildren.Length} objetos en police_office");

        int areasCreadas = 0;
        int maxAreas = 5;

        // Buscar objetos con nombres que sugieran que son investigables
        string[] searchableKeywords = new string[]
        {
            "file_cabinet", "desk", "cabinet", "drawer", "bookshelf",
            "shelf", "box", "container", "locker", "table", "furniture"
        };

        foreach (Transform child in allChildren)
        {
            if (areasCreadas >= maxAreas) break;
            if (child == policeOffice.transform) continue; // Skip root

            string childName = child.name.ToLower();

            // Verificar si el nombre contiene alguna keyword
            bool isCandidate = searchableKeywords.Any(keyword => childName.Contains(keyword));

            if (isCandidate && child.GetComponent<SearchableArea>() == null)
            {
                SetupSearchableArea(child.gameObject, allItems, areasCreadas);
                areasCreadas++;
                Debug.Log($"[SetupSearchAreas] Área {areasCreadas}/{maxAreas} configurada: {child.name}");
            }
        }

        // Si no encontramos suficientes, agregar en objetos genéricos
        if (areasCreadas < maxAreas)
        {
            Debug.LogWarning($"[SetupSearchAreas] Solo se encontraron {areasCreadas} candidatos automáticos.");
            Debug.LogWarning("[SetupSearchAreas] Agregando áreas en objetos genéricos...");

            foreach (Transform child in allChildren)
            {
                if (areasCreadas >= maxAreas) break;
                if (child == policeOffice.transform) continue;
                if (child.GetComponent<SearchableArea>() != null) continue;

                // Evitar objetos muy pequeños o de UI
                if (child.childCount == 0 && !child.GetComponent<Renderer>()) continue;

                SetupSearchableArea(child.gameObject, allItems, areasCreadas);
                areasCreadas++;
                Debug.Log($"[SetupSearchAreas] Área {areasCreadas}/{maxAreas} configurada (genérica): {child.name}");
            }
        }

        EditorUtility.SetDirty(policeOffice);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        Debug.Log($"[SetupSearchAreas] ¡{areasCreadas} áreas investigables configuradas exitosamente!");
    }

    private static void SetupSearchableArea(GameObject obj, ItemData[] allItems, int areaIndex)
    {
        // Agregar o obtener SearchableArea
        SearchableArea area = obj.GetComponent<SearchableArea>();
        if (area == null)
        {
            area = obj.AddComponent<SearchableArea>();
        }

        // Configurar probabilidad base según el índice
        float[] probabilities = { 50f, 40f, 45f, 35f, 30f };
        area.baseSearchChance = probabilities[areaIndex % probabilities.Length];

        // Asignar 3-4 items aleatorios
        int itemCount = Random.Range(3, 5);
        area.possibleItems = GetRandomItems(allItems, itemCount);

        // Configuración general
        area.cooldownTime = 0f; // Sin cooldown para testing
        area.maxSearches = -1; // Ilimitado
        area.interactionPrompt = "Presiona E para investigar";
        area.interactionRange = 2f;
        area.useHighlight = false; // Sin highlight por ahora
        area.showDebugInfo = true;

        // Agregar o configurar Collider
        Collider col = obj.GetComponent<Collider>();
        if (col == null)
        {
            // Crear un BoxCollider
            BoxCollider box = obj.AddComponent<BoxCollider>();
            box.isTrigger = true;
            box.size = new Vector3(1.5f, 1.5f, 1.5f); // Tamaño razonable
        }
        else
        {
            col.isTrigger = true;
        }

        EditorUtility.SetDirty(obj);
    }

    private static ItemData[] LoadAllItems()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Items" });
        ItemData[] items = new ItemData[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            items[i] = AssetDatabase.LoadAssetAtPath<ItemData>(path);
        }

        return items;
    }

    private static ItemData[] GetRandomItems(ItemData[] allItems, int count)
    {
        // Shuffle y tomar los primeros 'count'
        ItemData[] shuffled = allItems.OrderBy(x => Random.value).ToArray();
        return shuffled.Take(count).ToArray();
    }
}
