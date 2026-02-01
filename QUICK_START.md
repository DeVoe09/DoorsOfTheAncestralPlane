# Quick Start Guide - Doors of the Ancestral Plane

## 🚀 5-Minute Setup

### 1. Open in Unity Hub
- Open Unity Hub
- Click "Add"
- Select folder: `D:\UnityProjects\DoorsOfTheAncestralPlane`
- Click "Add Project"

### 2. Create First Scene
- In Unity: **File → New Scene → 3D (Basic)**
- Save as: `Assets/Scenes/AncestralPlane.unity`
- Create empty GameObject named "SceneSetup"
- Add component: `SceneSetup`
- Click "Setup Scene" button

### 3. Test It
- Press **Play** (top center)
- **WASD** = Move
- **Mouse** = Look
- **Space** = Jump
- **Left Shift** = Run

---

## 🎮 Controls Reference

| Action | Key |
|--------|-----|
| Move Forward | W |
| Move Backward | S |
| Strafe Left | A |
| Strafe Right | D |
| Look Around | Mouse |
| Jump | Space |
| Run | Left Shift |
| Dash (Anger) | Left Ctrl |
| Slow-mo (Calm) | Left Alt |

---

## 🎯 Emotion Mechanics

### Anger Realm (Red Door)
- **Speed**: +40% faster
- **Ability**: Dash (Left Ctrl)
- **Visual**: Red distortion
- **Best for**: Fast-paced exploration

### Calm Realm (Blue Door)
- **Speed**: -30% slower
- **Ability**: Slow-mo perception (Left Alt)
- **Visual**: Blue tint
- **Best for**: Mindful puzzle-solving

### Joy Realm (Yellow Door)
- **Speed**: +20% faster
- **Ability**: Double jump
- **Visual**: Yellow glow
- **Best for**: Vertical exploration

---

## 🏗️ Project Structure

```
DoorsOfTheAncestralPlane/
├── Assets/
│   ├── _Scripts/
│   │   ├── Core/          # GameManager, scene management
│   │   ├── Emotions/      # EmotionManager, states
│   │   ├── Player/        # FirstPersonController
│   │   ├── Doors/         # DoorController
│   │   ├── UI/            # BalanceUI
│   │   └── Utils/         # SceneSetup
│   ├── Scenes/            # Your scenes go here
│   └── Prefabs/           # Your prefabs go here
├── ProjectSettings/
└── Packages/
```

---

## 🔧 Common Tasks

### Add a New Scene
1. **File → New Scene**
2. Save to `Assets/Scenes/`
3. Add SceneSetup component
4. Click "Setup Scene"

### Create a Door
1. Create Cube (GameObject → 3D Object → Cube)
2. Add `DoorController` component
3. Set target realm (1=Anger, 2=Calm, 3=Joy)
4. Ensure Collider has "Is Trigger" checked

### Change Emotion
1. Select EmotionManager GameObject
2. Change `Current State` in inspector
3. Press Play to test

### Check Balance
1. Add BalanceUI to Canvas
2. Assign slider and text fields
3. Balance updates automatically

---

## 📋 Git Commands

### Save Your Work
```bash
cd D:\UnityProjects\DoorsOfTheAncestralPlane
git add .
git commit -m "Added new scene"
git push
```

### Check Status
```bash
git status
```

### View History
```bash
git log --oneline
```

---

## 🐛 Troubleshooting

### "Script compilation errors"
- Wait 10 seconds for Unity to recompile
- If persists: **Assets → Reimport All**

### "CharacterController not found"
- Ensure FirstPersonController is on same GameObject as CharacterController
- Or add `[RequireComponent(typeof(CharacterController))]` attribute

### "Camera not found"
- Assign `cameraTransform` in FirstPersonController inspector
- Or ensure MainCamera tag is set on your camera

### Doors not working
- Check DoorController has Collider with "Is Trigger" = true
- Ensure player has "Player" tag

### Managers not persisting
- Check GameManager and EmotionManager have `DontDestroyOnLoad` in Awake()

---

## 📚 Resources

- **Unity Manual**: https://docs.unity3d.com/Manual/
- **C# Scripting**: https://docs.unity3d.com/Manual/CreatingAndUsingScripts.html
- **Input System**: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/

---

## 🎯 Next Goals

### Week 1: Prototype
- [ ] Set up basic movement
- [ ] Create Ancestral Plane hub
- [ ] Test emotion switching

### Week 2: First Realm
- [ ] Build Anger Realm (simplest)
- [ ] Add emotion-specific mechanics
- [ ] Test door transitions

### Week 3-4: More Realms
- [ ] Build Calm Realm
- [ ] Build Joy Realm
- [ ] Polish each realm

### Week 5-6: Polish
- [ ] Add visual effects
- [ ] Add sound effects
- [ ] Balance gameplay

### Week 7-8: Finalize
- [ ] Playtest all realms
- [ ] Fix bugs
- [ ] Build for Windows

---

## 💡 Tips for Beginners

1. **Start Small**: Build one room at a time
2. **Test Often**: Press Play frequently
3. **Save Often**: Commit to Git after each session
4. **Ask Questions**: I'm here to help!
5. **Have Fun**: This is creative work!

---

**Ready to start? Open Unity Hub and let's build! 🚀**
