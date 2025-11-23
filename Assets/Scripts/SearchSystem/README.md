# Sistema de Investigación - GameUtch

Sistema completo de investigación/búsqueda de objetos con probabilidades y feedback visual.

## 📁 Estructura

```
SearchSystem/
├── Core/
│   ├── SearchManager.cs          - Singleton que maneja la lógica de búsqueda y probabilidades
│   ├── SearchableArea.cs          - Component para marcar objetos investigables
│   └── PlayerSearchController.cs  - Controla la interacción del jugador
├── Items/
│   ├── ItemData.cs               - ScriptableObject que define items
│   └── InventorySystem.cs         - Sistema de almacenamiento de items
└── UI/
    └── SearchUI.cs                - Maneja la interfaz de usuario
```

## 🚀 Configuración Inicial

### Paso 1: Crear GameObjects Managers

1. **SearchManager** (vacío en la escena):
   - Crear GameObject vacío llamado "SearchManager"
   - Agregar component `SearchManager.cs`
   - Configurar:
     - `Minimum Search Chance`: 20% (probabilidad mínima tras múltiples búsquedas)
     - `Search Count Penalty`: 0.15 (penalización por búsqueda repetida)
     - `Show Debug Logs`: true (para testing)

2. **InventorySystem** (vacío en la escena):
   - Crear GameObject vacío llamado "InventorySystem"
   - Agregar component `InventorySystem.cs`
   - Configurar:
     - `Max Capacity`: 50 (o -1 para ilimitado)
     - `Allow Duplicates`: true
     - `Show Debug Logs`: true

### Paso 2: Configurar el Jugador

En el GameObject `ThirdPersonController`:

1. Agregar component `PlayerSearchController.cs`
2. Asegurarse de que tiene:
   - Component `PlayerInput` (ya lo tiene)
   - Tag "Player"
3. Configurar:
   - `Search UI`: (asignar después de crear la UI)
   - `Prevent Search While Moving`: false
   - `Show Debug Info`: true

### Paso 3: Crear la UI

1. **Crear SearchUI** (en Canvas):
   ```
   Canvas/
   └── SearchUI/
       ├── ResultPanel/
       │   ├── Background (Image)
       │   └── ResultText (TextMeshPro)
       └── PromptPanel/
           └── PromptText (TextMeshPro)
   ```

2. **Configurar SearchUI GameObject**:
   - Agregar component `SearchUI.cs`
   - Asignar referencias:
     - Result Panel → ResultPanel GameObject
     - Result Text → ResultText (TextMeshProUGUI)
     - Result Background → Background (Image)
     - Prompt Panel → PromptPanel GameObject
     - Prompt Text → PromptText (TextMeshProUGUI)
   - Configurar tiempos:
     - Fade Duration: 0.3s
     - Result Display Time: 2.5s

3. **Volver al PlayerSearchController** y asignar:
   - `Search UI` → SearchUI GameObject

### Paso 4: Crear Items (ScriptableObjects)

1. En Assets, clic derecho → Create → SearchSystem → Item Data
2. Configurar cada item:
   - **Ejemplo - Evidencia Común**:
     - Item Name: "Llave Vieja"
     - Description: "Una llave oxidada"
     - Item Type: Key
     - Rarity: Common

   - **Ejemplo - Documento Raro**:
     - Item Name: "Documento Clasificado"
     - Description: "Información importante"
     - Item Type: Document
     - Rarity: Rare

3. Crear varios items de diferentes rarezas

### Paso 5: Configurar Áreas Investigables

En objetos del police_office (escritorios, archivadores, cajas):

1. Seleccionar objeto (ej: file_cabinet_medium)
2. Agregar Component → Box Collider (o ajustar el existente)
   - Is Trigger: ✓ true
3. Agregar Component → `SearchableArea.cs`
4. Configurar:
   - `Base Search Chance`: 50% (ajustar según dificultad)
   - `Possible Items`: Arrastrar los ItemData creados
   - `Cooldown Time`: 0 (o 3s para evitar spam)
   - `Max Searches`: -1 (ilimitado) o un número específico
   - `Interaction Prompt`: "Presiona E para investigar"
   - `Interaction Range`: 2.0
   - `Use Highlight`: true (opcional)
   - `Show Debug Info`: true

5. Asegurarse de que el objeto o su padre tenga un Collider con IsTrigger

## 🎮 Uso en Juego

### Jugador:
1. Acercarse a un objeto investigable (escritorio, archivador, caja)
2. Ver el mensaje "Presiona E para investigar"
3. Presionar **E** para buscar
4. Ver resultado:
   - "¡Encontraste: [Item]!" (color según rareza)
   - "No encontraste nada"

### Sistema de Probabilidades:

**Fórmula de éxito**:
```
probabilidadFinal = baseChance × rarityModifier × searchPenalty
```

**Modificadores de Rareza**:
- Common: ×1.0 (100%)
- Uncommon: ×0.7 (70%)
- Rare: ×0.4 (40%)

**Penalización por Búsquedas Repetidas**:
- 1ra búsqueda: penalización ×1.0
- 2da búsqueda: penalización ×0.85
- 3ra búsqueda: penalización ×0.70
- Mínimo: 20% (configurable en SearchManager)

**Ejemplo**:
```
Área con baseChance = 50%
Item Rare (×0.4)

1ra búsqueda: 50% × 0.4 × 1.0 = 20% de éxito
2da búsqueda: 50% × 0.4 × 0.85 = 17% de éxito
3ra búsqueda: 50% × 0.4 × 0.70 = 14% pero se aplica mínimo → 20%
```

## 🔧 Componentes Principales

### SearchManager
**Responsabilidad**: Cálculo de probabilidades y gestión global de búsquedas

**Métodos clave**:
- `PerformSearch(SearchableArea)`: Ejecuta una búsqueda
- `ResetSearchCounts()`: Reinicia contadores

### SearchableArea
**Responsabilidad**: Marca objetos como investigables y maneja la interacción local

**Propiedades importantes**:
- `baseSearchChance`: Probabilidad base (0-100%)
- `possibleItems`: Array de ItemData que se pueden encontrar
- `cooldownTime`: Tiempo entre búsquedas
- `maxSearches`: Límite de búsquedas (-1 = ilimitado)

**Métodos clave**:
- `TrySearch()`: Intenta realizar búsqueda
- `CanSearch(out reason)`: Verifica si se puede buscar
- `ResetArea()`: Reinicia el área

### PlayerSearchController
**Responsabilidad**: Maneja input del jugador y comunicación con SearchableArea

**Integración**:
- Usa Input System (acción "Interact")
- Detecta áreas cercanas automáticamente
- Actualiza UI con resultados

### InventorySystem
**Responsabilidad**: Almacenamiento y gestión de items encontrados

**Métodos útiles**:
- `AddItem(ItemData)`: Agregar item
- `GetAllItems()`: Obtener todos los items
- `GetItemsByType(ItemType)`: Filtrar por tipo
- `PrintInventory()`: Debug en consola

### ItemData (ScriptableObject)
**Responsabilidad**: Define propiedades de un item

**Propiedades**:
- `itemName`: Nombre del item
- `itemType`: Evidence/Document/Tool/Key/Misc
- `rarity`: Common/Uncommon/Rare
- `isQuestItem`: Para quests específicos

## 📊 Debug y Testing

### Consola de Debug
Con `showDebugLogs` activado, verás:
```
[SearchManager] Búsqueda en file_cabinet_001:
- Probabilidad base: 50%
- Búsquedas previas: 0
- Probabilidad final: 50%
¡Éxito! (roll: 32.4 <= 50.0%) - Item: Llave Vieja

[PlayerSearchController] Item encontrado y agregado: Llave Vieja
[InventorySystem] Item agregado: Llave Vieja (1/50)
```

### Comandos de Context Menu
En InventorySystem (Inspector):
- Clic derecho → `Print Inventory`: Muestra todos los items

### Gizmos en Scene View
SearchableArea muestra:
- Esfera amarilla: Área no activa
- Esfera verde: Jugador en rango

## 🎨 Personalización

### Colores de Rareza
En `ItemData.cs`:
```csharp
Common: Color.white
Uncommon: Verde (0.2, 0.8, 0.2)
Rare: Púrpura (0.5, 0.3, 1.0)
```

### Mensajes Personalizados
En `SearchManager`:
- `noItemFoundMessage`: "No encontraste nada"
- `itemFoundPrefix`: "¡Encontraste: "
- `itemFoundSuffix`: "!"

### Audio
En `SearchableArea`:
- `searchSound`: Sonido al iniciar búsqueda
- `successSound`: Sonido al encontrar item
- `failSound`: Sonido al no encontrar nada

### Highlight Visual
En `SearchableArea`:
- `highlightMaterial`: Material para resaltar objeto
- `useHighlight`: Activar/desactivar highlight

## 🐛 Troubleshooting

**Problema**: "Presiona E" no aparece
- Verificar que SearchUI esté asignado en PlayerSearchController
- Verificar que el objeto tenga Collider con IsTrigger = true
- Verificar que el jugador tenga tag "Player"

**Problema**: No funciona la tecla E
- Verificar que PlayerInput esté presente
- Verificar que la acción "Interact" exista en InputSystem_Actions
- Verificar que esté mapeada a la tecla E

**Problema**: Siempre encuentra items
- Reducir `baseSearchChance` en SearchableArea
- Usar items con Rarity = Rare
- Verificar que SearchManager esté en la escena

**Problema**: Inventario no guarda items
- Verificar que InventorySystem esté en la escena
- Verificar que `maxCapacity` no sea 0
- Verificar logs en consola con `showDebugLogs = true`

## 📝 Próximas Mejoras (Fase 2-4)

- [ ] UI de inventario visual con iconos
- [ ] Persistencia de inventario entre sesiones
- [ ] Efectos de partículas al encontrar items
- [ ] Sistema de quests integrado
- [ ] Crafteo con items encontrados
- [ ] Items consumibles
- [ ] Trading system

## 📚 Referencias

- Input System: `InputSystem_Actions.inputactions`
- Dialogue System: HeneGames (ejemplo de UI pattern)
- TextMeshPro: Para todos los textos UI

---

**Versión**: 1.0 - Fase 1 Completa
**Autor**: Claude Code
**Fecha**: 2025-01-22
