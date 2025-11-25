# Instrucciones de Configuración del Sistema de Minijuegos

## 📦 Resumen del Sistema

Has implementado un sistema de minijuegos MVP para CrunchTime que permite:
- ✅ Interactuar con computadoras presionando E
- ✅ Jugar un minijuego de typing simple
- ✅ Regresar automáticamente a la escena principal en la misma posición

**Tiempo total estimado**: 40-50 minutos en Unity Editor

## ✅ Scripts Creados (Ya están listos)
1. `Assets/Scripts/GameStateManager.cs` - Guarda estado del jugador entre escenas
2. `Assets/Scripts/ComputerInteractable.cs` - Hace computadoras interactivas
3. `Assets/Scripts/WorkMiniGameController.cs` - Controla el minijuego
4. `Assets/Scripts/PlayerStateRestorer.cs` - Restaura posición del jugador
5. `Assets/Scripts/SearchSystem/Core/PlayerSearchController.cs` - Modificado para soportar computadoras

## 📋 Checklist Rápido

- [ ] **PASO 1**: Agregar GameStateManager a Tutorial Level (5 min)
- [ ] **PASO 2**: Configurar computadora con ComputerInteractable (10 min)
- [ ] **PASO 3**: Crear escena WorkMiniGame con UI (15-20 min)
- [ ] **PASO 4**: Agregar WorkMiniGame a Build Settings (5 min)
- [ ] **PASO 5**: Agregar PlayerStateRestorer al jugador (5 min)
- [ ] **TEST**: Probar el flujo completo

---

## 📋 Pasos Manuales en Unity Editor

### PASO 1: Agregar GameStateManager a la Escena Tutorial Level (5 min)

1. **Abre Unity Editor** y carga el proyecto CrunchTime
2. **Abre la escena**: `Assets/Scenes/Tutorial Level.unity`
3. **En la Hierarchy**, haz clic derecho → `Create Empty`
4. **Renombra** el GameObject a `GameStateManager`
5. **Con GameStateManager seleccionado**, en el Inspector:
   - Haz clic en `Add Component`
   - Busca `GameStateManager`
   - Selecciona el script
6. **Configuración** (opcional):
   - Marca `Show Debug Info` si quieres ver logs en la consola
7. **Guarda la escena** (Ctrl+S / Cmd+S)

---

### PASO 2: Configurar la Computadora Interactiva (10 min)

1. **En la escena Tutorial Level**, localiza tu computadora en la Hierarchy
   - Si no la ves, búscala por nombre o mírala en la Scene view

2. **Selecciona** el GameObject de la computadora

3. **Agrega el componente ComputerInteractable**:
   - En el Inspector, clic en `Add Component`
   - Busca `Computer Interactable`
   - Selecciona el script

4. **Configura el Collider**:
   - Si la computadora NO tiene un Collider:
     - `Add Component` → busca `Box Collider`
   - **MUY IMPORTANTE**: Marca la casilla `Is Trigger`
   - Ajusta el tamaño del collider para cubrir el área donde el jugador puede interactuar
     - Usa los handles verdes en la Scene view para ajustar el tamaño

5. **Configura ComputerInteractable** (en el Inspector):
   - **Interaction Prompt**: "Presiona E para trabajar" (o el texto que quieras)
   - **Mini Game Scene Name**: `WorkMiniGame` (exactamente así)
   - **Use Highlight**: Desmarca si NO quieres que la computadora brille al acercarte
   - **Show Debug Info**: Marca si quieres ver logs

6. **(Opcional) Configurar Highlight**:
   - Si dejaste `Use Highlight` marcado:
     - Busca un material amarillo/verde en tu proyecto (o crea uno)
     - Arrastra el material al campo `Highlight Material`

7. **(Opcional) Agregar Sonido**:
   - Si tienes un sonido de clic/interacción:
     - Arrastra el AudioClip al campo `Interact Sound`

8. **Guarda la escena** (Ctrl+S / Cmd+S)

---

### PASO 3: Crear la Escena del Minijuego (15-20 min)

#### 3.1 Crear la Escena

1. **File** → **New Scene** → **Basic (Built-in)** o **Basic (URP)**
2. **File** → **Save As**
   - Nombre: `WorkMiniGame`
   - Ubicación: `Assets/Scenes/WorkMiniGame.unity`

#### 3.2 Eliminar Objetos Innecesarios

- Elimina **Main Camera** si existe (el minijuego solo necesita UI)
- Elimina **Directional Light** si existe

#### 3.3 Crear el Canvas UI

1. **Hierarchy** → Clic derecho → **UI** → **Canvas**
   - Se creará automáticamente Canvas + EventSystem

2. **Selecciona Canvas**, en el Inspector:
   - **Render Mode**: Screen Space - Overlay
   - **Canvas Scaler**:
     - UI Scale Mode: Scale With Screen Size
     - Reference Resolution: 1920 x 1080

#### 3.4 Crear Panel de Fondo

1. **Hierarchy** → Clic derecho en Canvas → **UI** → **Panel**
2. **Renombra** a `BackgroundPanel`
3. **Con BackgroundPanel seleccionado**:
   - **Rect Transform**: Stretch to fill (Alt+Shift+clic en el cuadrado de anchors)
   - **Image** component:
     - Color: Negro semi-transparente (R:0, G:0, B:0, A:180)

#### 3.5 Crear Texto de Instrucciones

1. **Clic derecho en Canvas** → **UI** → **Text - TextMeshPro**
   - Si aparece ventana "Import TMP Essentials", haz clic en `Import TMP Essentials`
2. **Renombra** a `InstructionsText`
3. **Rect Transform**:
   - Posición X: 0, Y: 350
   - Width: 800, Height: 100
4. **TextMeshProUGUI** component:
   - **Text**: "¡Presiona las teclas que aparecen!"
   - **Font Size**: 32
   - **Alignment**: Center y Middle
   - **Color**: Blanco

#### 3.6 Crear Texto de Letra (Grande)

1. **Clic derecho en Canvas** → **UI** → **Text - TextMeshPro**
2. **Renombra** a `LetterText`
3. **Rect Transform**:
   - Posición X: 0, Y: 0
   - Width: 400, Height: 400
4. **TextMeshProUGUI** component:
   - **Text**: "A"
   - **Font Size**: 200
   - **Alignment**: Center y Middle
   - **Color**: Amarillo o Verde brillante

#### 3.7 Crear Texto de Timer

1. **Clic derecho en Canvas** → **UI** → **Text - TextMeshPro**
2. **Renombra** a `TimerText`
3. **Rect Transform**:
   - Posición X: 0, Y: -250
   - Width: 400, Height: 60
4. **TextMeshProUGUI** component:
   - **Text**: "Tiempo: 60s"
   - **Font Size**: 36
   - **Alignment**: Center y Middle
   - **Color**: Blanco

#### 3.8 Crear Texto de Puntaje

1. **Clic derecho en Canvas** → **UI** → **Text - TextMeshPro**
2. **Renombra** a `ScoreText`
3. **Rect Transform**:
   - Posición X: 0, Y: -320
   - Width: 400, Height: 60
4. **TextMeshProUGUI** component:
   - **Text**: "Puntaje: 0"
   - **Font Size**: 32
   - **Alignment**: Center y Middle
   - **Color**: Blanco

#### 3.9 Crear Controlador del Minijuego

1. **Hierarchy** → Clic derecho → **Create Empty**
2. **Renombra** a `MiniGameController`
3. **Add Component** → busca `Work Mini Game Controller`
4. **Arrastra las referencias** en el Inspector:
   - **Letter Text**: Arrastra `LetterText` aquí
   - **Timer Text**: Arrastra `TimerText` aquí
   - **Score Text**: Arrastra `ScoreText` aquí
   - **Instructions Text**: Arrastra `InstructionsText` aquí
5. **Configuración**:
   - **Game Time**: 60 (segundos)
   - **Return Scene Name**: `Tutorial Level` (exactamente así)
   - **Possible Keys**: Deja las que están (A, S, D, F, J, K, L)
   - **Show Debug Info**: Marca si quieres ver logs

#### 3.10 Guardar la Escena

- **File** → **Save** (Ctrl+S / Cmd+S)

---

### PASO 4: Agregar la Escena del Minijuego a Build Settings (5 min)

**MUY IMPORTANTE** - Si no haces esto, el juego no encontrará la escena del minijuego.

1. **File** → **Build Settings**
2. **Verifica que "Tutorial Level" esté en la lista**:
   - Si NO está: Abre `Tutorial Level.unity` → File → Build Settings → `Add Open Scenes`
3. **Arrastra** `Assets/Scenes/WorkMiniGame.unity` a la ventana "Scenes In Build"
   - Debe aparecer con un índice (0, 1, 2, etc.)
4. **Cierra** Build Settings

---

### PASO 5: Agregar Restauración al Jugador (5 min)

Este script restaura al jugador cuando regresa del minijuego.

1. **Abre** `Assets/Scenes/Tutorial Level.unity`
2. **Busca el GameObject del jugador** en la Hierarchy
   - Busca un objeto llamado "Player", "PlayerController", o similar
   - Es el que tiene el script ThirdPersonController
3. **Selecciona** el GameObject del jugador
4. **Add Component** → busca `Player State Restorer`
5. **Configuración** (opcional):
   - Marca `Show Debug Info` si quieres ver logs
6. **Guarda la escena** (Ctrl+S / Cmd+S)

---

## 🎮 Cómo Probar

### Test 1: Compilación
1. Abre Unity
2. Espera a que compile (mira la barra de progreso abajo a la derecha)
3. Si hay errores en la Console, avísame

### Test 2: Interacción con Computadora
1. Presiona **Play** en Unity
2. Mueve al jugador hacia la computadora
3. **Debería aparecer**: "Presiona E para trabajar"
4. Presiona **E**
5. **Debería cargar** la escena del minijuego

### Test 3: Minijuego
1. **Deberías ver**: Una letra grande en el centro
2. **Presiona la tecla** que aparece
3. **Debería**: Generar una nueva letra
4. **El timer** debería contar hacia abajo
5. **El puntaje** debería aumentar con cada tecla correcta
6. **Presiona ESC** para salir

### Test 4: Regreso
1. Después de salir del minijuego
2. **Deberías**: Regresar a Tutorial Level
3. **El jugador debería**: Estar en la misma posición donde estaba antes

---

## ❗ Problemas Comunes

### "Scene 'WorkMiniGame' couldn't be loaded"
- **Solución**: Revisa PASO 4 - Agregar escena a Build Settings

### "Presiona E" no aparece
- **Solución**:
  - Verifica que el Collider de la computadora tenga `Is Trigger` marcado
  - Verifica que el jugador tenga el tag "Player"
  - Revisa la consola para ver errores

### El jugador no regresa a la misma posición
- **Solución**: Verifica que GameStateManager esté en la escena Tutorial Level

### Nada pasa al presionar una tecla en el minijuego
- **Solución**:
  - Verifica que todas las referencias de UI estén conectadas en MiniGameController
  - Revisa la consola para ver errores

---

## 📝 Próximos Pasos (Opcional)

Una vez que funcione el MVP básico, puedes:
1. Agregar más letras/teclas al minijuego
2. Aumentar dificultad (menos tiempo, más teclas)
3. Agregar recompensas (items al inventario)
4. Agregar efectos visuales (partículas, animaciones)
5. Agregar más tipos de minijuegos

---

## 🆘 Si Necesitas Ayuda

Si algo no funciona:
1. Copia el error de la Console de Unity
2. Dime en qué paso estás
3. Descríbeme qué pasa vs qué debería pasar
4. Te ayudaré a resolverlo
