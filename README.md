# Doors of the Ancestral Plane

A Unity 6.3 LTS game project exploring emotional states and realm navigation.

## Features

- **Emotion System**: Dynamic emotional state management (Neutral, Anger, Calm, Joy)
- **Balance Meter**: Track emotional balance (-1 to 1)
- **First-Person Controller**: Smooth movement and camera controls
- **Realm System**: Complete 3 different realms
- **Session Tracking**: Track playtime and progress

## Project Structure

```
Assets/
├── _Scripts/
│   ├── Core/          # GameManager, core systems
│   ├── Emotions/      # EmotionManager, emotional states
│   └── Player/        # FirstPersonController
├── Scenes/            # Unity scenes
├── Prefabs/           # Prefab assets
├── Materials/         # Materials
├── Textures/          # Textures
├── Audio/             # Audio files
├── Shaders/           # Shaders
└── UI/                # UI elements
```

## Getting Started

1. Open the project in Unity 6.3 LTS
2. Ensure your player GameObject has a CharacterController component
3. Assign the camera transform to the FirstPersonController
4. Press Play to test

## Controls

- **WASD**: Move
- **Mouse**: Look around
- **Space**: Jump
- **ESC**: Unlock cursor (in editor)

## Development

- **Unity Version**: 6000.0.23f1 (Unity 6.3 LTS)
- **Language**: C#
- **Platform**: PC/Windows

## Git Setup

```bash
# Clone the repository
git clone https://github.com/DeVoe09/DoorsOfTheAncestralPlane.git

# Navigate to project
cd DoorsOfTheAncestralPlane

# Open in Unity Hub
# Add project from disk: D:\UnityProjects\DoorsOfTheAncestralPlane
```

## License

MIT License
