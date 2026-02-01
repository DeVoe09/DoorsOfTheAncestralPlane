# Doors of the Ancestral Plane

A psychedelic first-person exploration prototype where the player enters colored emotion doors from an Ancestral Plane (Black spiritual void). Each door warps perception and gameplay through Gita-inspired emotional mechanics.

## Project Details

- **Engine**: Unity 6000.3.6f1 LTS
- **Platform**: PC (Windows)
- **Genre**: First-person emotional exploration
- **Target Session**: 8-12 minutes
- **Controller**: Keyboard/Mouse

## Project Structure

```
DoorsOfTheAncestralPlane/
├── Assets/
│   ├── _Scripts/
│   │   ├── Core/          # GameManager, scene management
│   │   ├── Emotions/      # EmotionManager, emotional states
│   │   ├── Player/        # FirstPersonController, movement
│   │   ├── Doors/         # DoorController, realm transitions
│   │   ├── UI/            # BalanceUI, HUD elements
│   │   └── Utils/         # SceneSetup, helper scripts
│   ├── Scenes/            # Unity scenes (to be created)
│   ├── Prefabs/           # Prefabs (to be created)
│   └── Materials/         # Materials for visual effects
├── ProjectSettings/       # Unity project settings
└── Packages/              # Unity package dependencies
```

## Core Mechanics

### Emotion System
Each emotion fundamentally changes movement and abilities:

- **Anger** (Red Door)
  - +40% movement speed
  - Dash ability (Left Control)
  - Red visual distortion
  - Aggressive gameplay focus

- **Calm** (Blue Door)
  - -30% movement speed
  - Slow-mo perception
  - Blue visual tint
  - Mindful gameplay focus

- **Joy** (Yellow Door)
  - +20% movement speed
  - Double jump ability
  - Yellow visual glow
  - Energetic gameplay focus

### Balance System
- Starts at 50%
- Chaotic actions decrease balance
- Mindful actions increase balance
- High balance (>75%) unlocks White Door ending
- Low balance (<25%) creates chaos effects

### Realm Structure
1. **Ancestral Plane** (Hub World)
   - Black spiritual void
   - Three colored doors (Anger, Calm, Joy)
   - White Door (ending, requires 75% balance)

2. **Emotion Realms**
   - Each realm is a short exploration area
   - Unique mechanics based on emotion
   - Completing returns to Ancestral Plane

## Setup Instructions

### Prerequisites
- Unity Hub installed
- Unity 6000.3.6f1 LTS installed

### Opening the Project
1. Open Unity Hub
2. Click "Add" and select the project folder: `D:\UnityProjects\DoorsOfTheAncestralPlane`
3. Unity will automatically import the project
4. Wait for the import to complete (may take a few minutes)

### Creating Your First Scene
1. In Unity, go to **File > New Scene**
2. Select "3D (Basic)" template
3. Save the scene as `AncestralPlane.unity` in the `Assets/Scenes/` folder
4. Add the SceneSetup component to an empty GameObject:
   - Create empty GameObject (GameObject > Create Empty)
   - Name it "SceneSetup"
   - Add component: `SceneSetup`
   - Check "Setup On Start"
   - Click "Setup Scene" button

### Testing the Game
1. Press **Play** button in Unity Editor
2. Use **WASD** to move
3. Use **Mouse** to look around
4. Press **Space** to jump
5. Walk into colored doors to test realm transitions

## Script Components

### GameManager.cs
- Manages game state across scenes
- Tracks current realm, balance meter, session time
- Handles scene transitions
- Singleton pattern (persists across scenes)

### EmotionManager.cs
- Manages emotional states (Neutral, Anger, Calm, Joy)
- Applies emotion-specific effects
- Tracks abilities (double jump, dash, slow-mo)
- Singleton pattern

### FirstPersonController.cs
- First-person movement and camera control
- WASD movement, mouse look
- Jumping with gravity
- Emotion-based abilities
- Requires CharacterController component

### DoorController.cs
- Controls door behavior
- Handles realm transitions
- Balance-based locking/unlocking
- Visual feedback (locked/unlocked states)

### BalanceUI.cs
- Displays balance meter, realm, emotion, timer
- Updates in real-time
- Color-coded balance indicator

### SceneSetup.cs
- Helper for quick scene setup
- Creates player, managers, doors
- Useful for prototyping

## Input Controls

| Action | Key |
|--------|-----|
| Move | W, A, S, D |
| Look | Mouse |
| Jump | Space |
| Run | Left Shift |
| Dash (Anger) | Left Control |
| Slow-mo (Calm) | Left Alt |

## Development Workflow

### 1. Prototype Phase (Weeks 1-2)
- Set up basic movement and camera
- Create Ancestral Plane hub
- Implement emotion switching
- Test core mechanics

### 2. Emotion Realms (Weeks 3-5)
- Design and build Anger Realm
- Design and build Calm Realm
- Design and build Joy Realm
- Add emotion-specific mechanics

### 3. Polish & UI (Weeks 6-7)
- Implement Balance UI
- Add visual effects (post-processing)
- Add sound effects
- Polish transitions

### 4. Testing & Build (Week 8)
- Playtest all realms
- Fix bugs
- Build for Windows
- Create final documentation

## Git Workflow

### Initial Setup (Already Done)
```bash
cd D:\UnityProjects\DoorsOfTheAncestralPlane
git init
git add .
git commit -m "Initial project setup"
git remote add origin https://github.com/DeVoe09/DoorsOfTheAncestralPlane.git
git push -u origin main
```

### Daily Development
```bash
# After making changes
git add .
git commit -m "Description of changes"
git push
```

### Useful Git Commands
```bash
# Check status
git status

# View commit history
git log --oneline

# Create a new branch for features
git checkout -b feature/your-feature-name

# Merge feature branch
git checkout main
git merge feature/your-feature-name
```

## Troubleshooting

### Common Issues

**Issue**: Scripts show compilation errors
- **Solution**: Wait for Unity to recompile, or restart Unity

**Issue**: CharacterController not found
- **Solution**: Ensure FirstPersonController is on the same GameObject as CharacterController

**Issue**: Camera not found
- **Solution**: Assign cameraTransform in FirstPersonController inspector, or ensure MainCamera tag is set

**Issue**: Managers not persisting across scenes
- **Solution**: Ensure GameManager and EmotionManager have DontDestroyOnLoad in Awake()

**Issue**: Doors not working
- **Solution**: Check that DoorController has Collider with "Is Trigger" enabled

### Getting Help
- Check Unity Console for error messages
- Review script comments for usage
- Test components individually in isolation

## Next Steps

1. **Open Unity Hub** and add the project
2. **Create first scene**: AncestralPlane.unity
3. **Run SceneSetup** to create basic environment
4. **Test movement** with WASD and mouse
5. **Create emotion doors** and test transitions
6. **Build first realm** (start with Anger for simplicity)
7. **Iterate and polish**

## Resources

- Unity Documentation: https://docs.unity3d.com/Manual/index.html
- Unity Input System: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/index.html
- C# Scripting: https://docs.unity3d.com/Manual/CreatingAndUsingScripts.html

## License

This project is for educational and prototyping purposes.

---

**Last Updated**: 2026-02-01
**Unity Version**: 6000.3.6f1 LTS
**Status**: Project Setup Complete - Ready for Development
