# Configuración Manual Final - Sistema de Investigación

## ✅ Lo que YA está configurado automáticamente

### 1. Managers Creados ✓
- **SearchManager** GameObject en la escena
  - Componente agregado y configurado
  - Probabilidad mínima: 20%
  - Penalización: 15%
  - Debug logs: activado

- **InventorySystem** GameObject en la escena
  - Componente agregado y configurado
  - Capacidad: 50 items
  - Permite duplicados: sí
  - Debug logs: activado

### 2. Jugador Configurado ✓
- **ThirdPersonController** tiene el componente `PlayerSearchController`
  - preventSearchWhileMoving: false
  - movementThreshold: 0.1
  - showDebugInfo: true
  - ⚠️ **FALTA**: Asignar referencia a SearchUI (ver abajo)

### 3. Items Creados ✓
8 ItemData ScriptableObjects en `Assets/Items/`:
- **Llaves**: Llave de Oficina (Common), Llave Maestra (Rare)
- **Documentos**: Informe Policial (Common), Expediente Clasificado (Uncommon), Evidencia Crítica (Rare)
- **Herramientas**: Linterna (Common), Radio Portátil (Uncommon)
- **Evidencia**: Huella Digital (Rare)

### 4. UI Básica Creada ✓
- **SearchUI** GameObject creado como hijo de Canvas
- Componente `SearchUI` agregado
- ⚠️ **FALTAN**: Panels y TextMeshPro (ver abajo)

### 5. Área Investigable de Ejemplo ✓
- **SearchArea_Escritorio** creado en police_office
- Tiene BoxCollider y SearchableArea
- ⚠️ **FALTAN**: Configuración completa (ver abajo)

---

## 🔧 Configuración Manual Requerida

### Paso 1: Configurar SearchUI (IMPORTANTE)

1. Seleccionar `Canvas/SearchUI` en la Hierarchy
2. Crear estructura de panels:

**Estructura requerida:**
```
SearchUI/
├─ ResultPanel (GameObject)
│  ├─ Agregar: Image component (opcional, para fondo)
│  └─ ResultText (GameObject con TextMeshProUGUI)
└─ PromptPanel (GameObject)
   └─ PromptText (GameObject con TextMeshProUGUI)
```

**Crear los panels:**

a) **ResultPanel**:
   - En SearchUI: Clic derecho → UI → Panel (renombrar a "ResultPanel")
   - Configurar RectTransform:
     - Anchors: Center
     - Pos Y: 200
     - Width: 600, Height: 100
   - En ResultPanel: Clic derecho → UI → Text - TextMeshPro (renombrar a "ResultText")
   - En ResultText:
     - Text: "" (vacío)
     - Font Size: 36
     - Alignment: Center
     - Color: White

b) **PromptPanel**:
   - En SearchUI: Clic derecho → UI → Panel (renombrar a "PromptPanel")
   - Configurar RectTransform:
     - Anchors: Bottom-Center
     - Pos Y: 100
     - Width: 500, Height: 80
   - En PromptPanel: Clic derecho → UI → Text - TextMeshPro (renombrar a "PromptText")
   - En PromptText:
     - Text: "" (vacío)
     - Font Size: 24
     - Alignment: Center
     - Color: Yellow

3. **Asignar referencias en SearchUI component:**
   - Result Panel → ResultPanel GameObject
   - Result Text → ResultText (TextMeshProUGUI)
   - Prompt Panel → PromptPanel GameObject
   - Prompt Text → PromptText (TextMeshProUGUI)
   - Fade Duration: 0.3
   - Result Display Time: 2.5

### Paso 2: Conectar SearchUI con PlayerSearchController

1. Seleccionar `ThirdPersonController` en Hierarchy
2. En el componente `PlayerSearchController`:
   - Search UI → Arrastrar el GameObject `SearchUI`

### Paso 3: Configurar SearchArea_Escritorio

1. Seleccionar `police_office/SearchArea_Escritorio`

2. **Configurar BoxCollider:**
   - Is Trigger: ✓ TRUE (MUY IMPORTANTE)
   - Size: (1.5, 1.5, 1.5) o ajustar según necesites

3. **Configurar SearchableArea component:**
   - Base Search Chance: 50
   - Possible Items: Expandir y configurar Size = 4
     - Element 0: Llave de Oficina
     - Element 1: Informe Policial
     - Element 2: Linterna
     - Element 3: Expediente Clasificado
   - Cooldown Time: 0 (para testing)
   - Max Searches: -1 (ilimitado)
   - Interaction Prompt: "Presiona E para investigar"
   - Interaction Range: 2
   - Use Highlight: false
   - Show Debug Info: true

### Paso 4: Crear Más Áreas Investigables (OPCIONAL)

Para crear más áreas de búsqueda:

1. En Hierarchy: Clic derecho en `police_office` → Create Empty
2. Renombrar (ej: "SearchArea_Archivador")
3. Agregar Component → Box Collider
   - Is Trigger: ✓ TRUE
   - Size: (1.5, 1.5, 1.5)
4. Agregar Component → SearchableArea
5. Configurar:
   - Base Search Chance: 30-50 (varía por área)
   - Possible Items: Asignar 3-4 items de Assets/Items/
   - Resto igual que arriba
6. Posicionar el GameObject cerca de objetos físicos del edificio

**Sugerencia de probabilidades:**
- Escritorios: 50%
- Archivadores: 40%
- Estanterías: 45%
- Cajas: 35%
- Papeleras: 30%

---

## 🎮 Testeo del Sistema

### Verificación Paso a Paso:

1. **Play Mode**:
   - Presionar Play en Unity
   - Acercarse a SearchArea_Escritorio

2. **Verificar Prompt**:
   - Debería aparecer "Presiona E para investigar" en la parte inferior

3. **Buscar**:
   - Presionar **E**
   - Debería aparecer mensaje:
     - "¡Encontraste: [item]!" (si éxito)
     - "No encontraste nada" (si falla)

4. **Revisar Console**:
   - Con debug logs activados, verás:
```
[SearchManager] Búsqueda en SearchArea_Escritorio:
- Probabilidad base: 50%
- Búsquedas previas: 0
- Probabilidad final: 50%
¡Éxito! (roll: 32.4 <= 50.0%) - Item: Llave de Oficina

[PlayerSearchController] Item encontrado y agregado: Llave de Oficina
[InventorySystem] Item agregado: Llave de Oficina (1/50)
```

5. **Verificar Inventario**:
   - Seleccionar InventorySystem en Hierarchy
   - Inspector → Clic derecho en component → Print Inventory
   - Console mostrará todos los items encontrados

### Problemas Comunes:

**"Presiona E" no aparece:**
- Verificar que SearchUI esté asignado en PlayerSearchController
- Verificar que BoxCollider.isTrigger = true
- Verificar que el jugador tenga tag "Player"

**No se encuentra nada nunca:**
- Verificar que Possible Items tenga items asignados
- Intentar poner Base Search Chance = 100 para testing

**Error en Console:**
- Leer el mensaje de error
- Verificar que todos los managers estén en la escena
- Verificar que no haya referencias null

---

## 📝 Próximos Pasos Recomendados

### Mejoras Visuales:
1. Agregar sprites/iconos a los items
2. Mejorar estilo de la UI (fondos, colores)
3. Agregar Material de highlight para SearchableArea

### Mejoras Funcionales:
1. Crear UI de inventario visual
2. Agregar sonidos (buscar, éxito, fallo)
3. Configurar más áreas investigables

### Testing:
1. Testear con diferentes probabilidades
2. Ajustar Balance según gameplay deseado
3. Probar con múltiples áreas

---

## 🎯 Checklist Final

Antes de considerar el sistema completo:

- [ ] SearchUI tiene ResultPanel y PromptPanel configurados
- [ ] TextMeshPro texts creados y asignados
- [ ] PlayerSearchController.searchUI apunta a SearchUI
- [ ] SearchArea_Escritorio tiene BoxCollider.isTrigger = true
- [ ] SearchArea_Escritorio tiene 3-4 items asignados
- [ ] Sistema testado en Play Mode
- [ ] Mensajes aparecen correctamente
- [ ] Items se agregan al inventario

---

**Versión**: 1.0
**Fecha**: 2025-01-22
**Estado**: Configuración automática completa - Requiere setup manual de UI
