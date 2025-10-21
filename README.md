# CrunchTime

Un proyecto de videojuego desarrollado en Unity con soporte multiplataforma (PC, Mobile, XR).

## 🎮 Descripción

CrunchTime es un proyecto de juego en Unity que utiliza el Universal Render Pipeline (URP) y el nuevo Input System de Unity, diseñado para ofrecer una experiencia de juego optimizada en múltiples plataformas.

## 🛠️ Tecnologías

- **Motor**: Unity 6000.2.6f2
- **Render Pipeline**: Universal Render Pipeline (URP) 17.2.0
- **Input System**: Unity Input System 1.14.2 (New Input System)
- **Scripting**: C# (.NET)

## 📋 Características

### Sistema de Input Completo
- **Movimiento**: WASD, flechas, gamepad, touch, XR
- **Cámara**: Mouse, gamepad stick, touch
- **Acciones**: Ataque, Salto, Sprint, Agacharse, Interacción
- **Navegación de Armas**: Previous/Next
- **Soporte Multiplataforma**: Keyboard & Mouse, Gamepad, Touch, Joystick, XR

### Configuraciones de Renderizado
- **Mobile**: Configuración optimizada para dispositivos móviles
- **PC**: Configuración de alta calidad para escritorio
- **Post-procesamiento**: Volume Profiles configurables

### Plataformas Soportadas
- 🖥️ PC (Windows, macOS, Linux)
- 📱 Mobile (iOS, Android)
- 🥽 XR (VR/AR)
- 🎮 Consolas (con configuración adicional)

## 🚀 Requisitos

### Para Desarrollo
- Unity Hub instalado
- Unity Editor 6000.2.6f2 o superior
- Visual Studio 2022 o JetBrains Rider (recomendado)
- Git para control de versiones

### Dependencias Principales
```json
{
  "com.unity.inputsystem": "1.14.2",
  "com.unity.render-pipelines.universal": "17.2.0",
  "com.unity.ai.navigation": "2.0.9",
  "com.unity.multiplayer.center": "1.0.0",
  "com.unity.test-framework": "1.6.0",
  "com.unity.timeline": "1.8.9"
}
```

## 📦 Instalación

### Clonar el Repositorio
```bash
git clone https://github.com/Foralitos/CrunchTime.git
cd CrunchTime
```

### Abrir en Unity
1. Abre Unity Hub
2. Click en "Add" y selecciona la carpeta del proyecto
3. Asegúrate de tener Unity 6000.2.6f2 instalado
4. Haz click en el proyecto para abrirlo

### Primera Ejecución
1. Unity importará y compilará todos los assets (puede tomar varios minutos)
2. Abre la escena `Assets/Scenes/SampleScene.unity`
3. Presiona el botón Play para probar el juego

## 🎯 Controles

### Teclado y Mouse
- **WASD / Flechas**: Movimiento
- **Mouse**: Control de cámara
- **Click Izquierdo / Enter**: Ataque
- **Espacio**: Saltar
- **Shift Izquierdo**: Sprint
- **C**: Agacharse
- **E**: Interactuar (mantener presionado)
- **1 / 2**: Cambiar arma anterior/siguiente

### Gamepad
- **Stick Izquierdo**: Movimiento
- **Stick Derecho**: Cámara
- **Botón Oeste (X/Square)**: Ataque
- **Botón Sur (A/Cross)**: Saltar
- **Press Stick Izquierdo**: Sprint
- **Botón Este (B/Circle)**: Agacharse
- **Botón Norte (Y/Triangle)**: Interactuar
- **D-Pad Izquierda/Derecha**: Cambiar arma

## 📁 Estructura del Proyecto

```
CrunchTime/
├── Assets/
│   ├── Scenes/                    # Escenas del juego
│   ├── Settings/                  # Configuraciones URP
│   │   ├── Mobile_RPAsset.asset
│   │   ├── PC_RPAsset.asset
│   │   └── DefaultVolumeProfile.asset
│   ├── TutorialInfo/              # Assets del tutorial
│   └── InputSystem_Actions.inputactions  # Configuración de inputs
├── Packages/
│   ├── manifest.json              # Dependencias del proyecto
│   └── packages-lock.json
├── ProjectSettings/               # Configuración del proyecto Unity
├── README.md                      # Este archivo
├── CLAUDE.md                      # Guía para Claude Code
└── .gitignore                     # Archivos ignorados por Git
```

## 💻 Desarrollo

### Agregar Nuevos Scripts
Se recomienda organizar los scripts en:
```
Assets/Scripts/
├── Player/       # Controladores y componentes del jugador
├── Enemies/      # IA y comportamientos de enemigos
├── Managers/     # Managers y sistemas singleton
├── UI/           # Controladores de interfaz
└── Utilities/    # Clases helper y utilidades
```

### Modificar Input Actions
1. Abre `InputSystem_Actions.inputactions` en Unity
2. Usa la ventana Input Actions para modificar bindings
3. Guarda y regenera la clase C# si es necesario

### Configurar Renderizado
- Usa `Mobile_RPAsset.asset` para builds móviles
- Usa `PC_RPAsset.asset` para builds de escritorio
- Ajusta los Volume Profiles para post-procesamiento

## 🏗️ Build

### Build para PC
1. File > Build Settings
2. Selecciona plataforma (Windows, macOS, Linux)
3. Click "Build" o "Build and Run"

### Build para Mobile
1. File > Build Settings
2. Selecciona iOS o Android
3. Configura Player Settings (Bundle ID, permisos, etc.)
4. Click "Build"

## 🧪 Testing

### En el Editor
- Presiona Play en Unity Editor
- Usa la ventana Game para ver el resultado
- Usa la consola para ver logs y errores

### Tests Automatizados
```bash
# Unity Test Framework está incluido
# Ejecuta tests desde: Window > General > Test Runner
```

## 🤝 Contribuir

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

### Convenciones de Código
- Usa PascalCase para clases y métodos públicos
- Usa camelCase para variables privadas
- Comenta código complejo
- Sigue los principios SOLID

## 📝 Notas Importantes

- ⚠️ **Usa el New Input System**: No uses `Input.GetKey()` o `Input.GetAxis()` legacy
- ⚠️ **URP Shaders Only**: No uses shaders del Built-in Render Pipeline
- ⚠️ **Volume Profiles**: Configura volume profiles en nuevas escenas
- ⚠️ **Git**: No commits de Library/, Temp/, Logs/, o archivos .sln/.csproj

## 🔗 Enlaces Útiles

- [Unity Documentation](https://docs.unity3d.com/)
- [URP Documentation](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest)
- [Input System Documentation](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest)
- [Repositorio del Proyecto](https://github.com/Foralitos/CrunchTime)

## 📄 Licencia

[Especificar licencia aquí]

## 👥 Autores

- **Foralitos** - [GitHub](https://github.com/Foralitos)

## 🙏 Agradecimientos

- Unity Technologies por el motor y herramientas
- La comunidad de desarrollo de Unity
- Contribuidores del proyecto

---

**Desarrollado con ❤️ usando Unity**
