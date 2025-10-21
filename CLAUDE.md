# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**CrunchTime** is a Unity game project built with Unity 6000.2.6f2 using the Universal Render Pipeline (URP). The project is set up with the new Input System and includes mobile and PC rendering configurations.

## Development Environment

### Unity Version
- **Editor Version**: 6000.2.6f2
- **Product Name**: GameUtch
- **Render Pipeline**: Universal Render Pipeline (URP) 17.2.0

### Key Dependencies
- `com.unity.inputsystem`: 1.14.2 (New Input System)
- `com.unity.render-pipelines.universal`: 17.2.0 (URP)
- `com.unity.ai.navigation`: 2.0.9
- `com.unity.multiplayer.center`: 1.0.0
- `com.unity.test-framework`: 1.6.0
- `com.unity.timeline`: 1.8.9

## Project Structure

```
Assets/
├── Scenes/              # Unity scenes (SampleScene.unity)
├── Settings/            # Render pipeline and volume profile settings
│   ├── Mobile_RPAsset.asset / Mobile_Renderer.asset
│   ├── PC_RPAsset.asset / PC_Renderer.asset
│   ├── DefaultVolumeProfile.asset
│   └── UniversalRenderPipelineGlobalSettings.asset
├── TutorialInfo/        # Tutorial assets and readme editor
└── InputSystem_Actions.inputactions  # Input action mappings
```

## Input System Configuration

The project uses Unity's new Input System with two action maps:

### Player Action Map
- **Move**: WASD/Arrow keys, gamepad left stick, XR controller (Vector2)
- **Look**: Mouse delta, gamepad right stick (Vector2)
- **Attack**: Mouse left button, gamepad west button, Enter key, touch tap
- **Interact**: E key, gamepad north button (Hold interaction)
- **Crouch**: C key, gamepad east button
- **Jump**: Space, gamepad south button, XR secondary button
- **Sprint**: Left Shift, gamepad left stick press
- **Previous/Next**: 1/2 keys, gamepad D-pad left/right

### UI Action Map
Standard UI navigation actions (Navigate, Submit, Cancel, Point, Click, RightClick, MiddleClick, ScrollWheel, TrackedDevicePosition, TrackedDeviceOrientation)

### Control Schemes
- Keyboard&Mouse
- Gamepad
- Touch
- Joystick
- XR

## Render Pipeline Configuration

The project has dual rendering configurations:

- **Mobile**: Optimized settings for mobile platforms (`Mobile_RPAsset.asset`, `Mobile_Renderer.asset`)
- **PC**: Desktop-optimized settings (`PC_RPAsset.asset`, `PC_Renderer.asset`)

Both use URP with volume profiles for post-processing.

## Working with Unity Projects

### Opening the Project
1. Open Unity Hub
2. Add project from `/Users/fora/games/GameUtch`
3. Ensure Unity 6000.2.6f2 is installed

### Testing in Unity Editor
- Open `Assets/Scenes/SampleScene.unity`
- Press Play in the Unity Editor

### Building the Project
Use Unity's Build Settings (File > Build Settings) to configure and build for target platforms.

### Modifying Input Actions
- Open `InputSystem_Actions.inputactions` in the Unity Editor
- Use the Input Actions window to modify bindings
- Regenerate C# class if needed

## Code Architecture

Currently, the project is in early stages with minimal custom code. The existing C# files are:
- `Assets/TutorialInfo/Scripts/Readme.cs`: ScriptableObject for displaying tutorial information
- `Assets/TutorialInfo/Scripts/Editor/ReadmeEditor.cs`: Custom editor for Readme asset

### Adding New Scripts
Place gameplay scripts in `Assets/Scripts/` (create directory as needed). Common organization:
- `Assets/Scripts/Player/` - Player controllers and components
- `Assets/Scripts/Enemies/` - Enemy AI and behaviors
- `Assets/Scripts/Managers/` - Game managers and singleton systems
- `Assets/Scripts/UI/` - UI controllers and components
- `Assets/Scripts/Utilities/` - Helper and utility classes

## Version Control

The project uses Git with a Unity-specific `.gitignore` that excludes:
- `Library/` - Unity's cache (regenerated automatically)
- `Temp/` - Temporary files
- `Logs/` - Log files
- `UserSettings/` - User-specific settings
- `.vs/`, `.csproj`, `.sln` - IDE-generated files

Only commit:
- `Assets/`
- `Packages/manifest.json` and `packages-lock.json`
- `ProjectSettings/`
- Custom documentation and configuration files

## Important Notes

- This project uses the **new Input System** - do not use legacy `Input.GetKey()` or `Input.GetAxis()`
- URP requires specific shaders - do not use Built-in Render Pipeline shaders
- When creating new scenes, ensure URP volume profiles are configured
- The project supports multiple platforms (PC, Mobile, XR) - test platform-specific features accordingly
