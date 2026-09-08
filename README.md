# 🛸 Alien Escape 3D - The Game

An immersive 3D alien abduction escape game with cinematic sequences, progression systems, and competitive leaderboards.

## 🎮 Features

### Core Gameplay
- **Cinematic Opening**: Full video cutscene of abduction with stunning visual effects
- **3D Realistic Environment**: Highly detailed alien spacecraft interiors
- **Gender Selection**: Choose your character at game start
- **Difficulty Levels**: Easy, Medium, Hard with progressive complexity
- **Progressive Sections**: Navigate through multiple rooms (more rooms on harder difficulties)

### Progression & Rewards
- **Coin System**: Earn coins based on difficulty and level completion
- **Upgrade Shop**: Purchase permanent upgrades
  - Revives: Get a second chance if caught
  - Checkpoint Return: Go back to your last checkpoint instead of losing progress
- **Level System**: Starts at Level 1, max Level 999
  - Gain XP each playthrough
  - Level up every 5-10 successful runs
  - Unlock new features and areas with higher levels

### Consequences & Gameplay Loop
- **Getting Caught**: Screen blurs, you get knocked out
- **Wake Up Sequence**: Return to your cell with awareness of failure
- **Restart Mechanic**: Begin section again (or use upgrades to mitigate)

### User Interface
- **Dynamic HUD**: Level display, health, stamina, coins
- **Leaderboard System**: Track
  - Win Streaks (consecutive successful escapes)
  - Day Streaks (consecutive daily plays)
  - Highest Levels Achieved
- **Main Menu**: Character selection, difficulty selection, settings
- **Pause Menu**: Settings, upgrades shop, profile stats

## 📋 Project Structure

```
alien-escape-3d-game/
├── Assets/
│   ├── Scripts/
│   │   ├── Player/
│   │   ├── UI/
│   │   ├── Game/
│   │   ├── Enemies/
│   │   ├── Progression/
│   │   └── Utilities/
│   ├── Models/
│   ├── Materials/
│   ├── Animations/
│   ├── Audio/
│   ├── Video/
│   └── Scenes/
├── ProjectSettings/
├── Documentation/
└── Build/
```

## 🔧 Tech Stack

- **Engine**: Unity 2022 LTS or Higher
- **Language**: C#
- **Target Platforms**: PC, Console, Mobile
- **Graphics**: URP (Universal Render Pipeline)

## 📖 Game Flow

1. **Main Menu** → Select Gender & Difficulty
2. **Opening Cutscene** → Video abduction sequence
3. **Wake Up** → In alien cell, start escape
4. **Navigate Sections** → Multiple rooms with increasing difficulty
5. **Avoid/Outsmart Guards** → Different AI behaviors per difficulty
6. **Reach Escape Pod** → Victory!
7. **Consequences or Success** → Coins earned, level progression
8. **Leaderboard Update** → Win streak, day streak tracking

## 🎯 Difficulty Breakdown

### Easy
- 3-4 rooms to navigate
- Slower, predictable enemy AI
- More ammo/resources
- 1x Coin multiplier

### Medium
- 5-6 rooms to navigate
- Medium-speed adaptive AI
- Standard resources
- 2x Coin multiplier

### Hard
- 7-8 rooms to navigate
- Fast, intelligent enemy AI
- Scarce resources
- 4x Coin multiplier

## 💰 Coin Economy

| Action | Coins Earned |
|--------|-------------|
| Easy Complete | 50 |
| Medium Complete | 100 |
| Hard Complete | 200 |
| Finding Secret Areas | 25-50 |
| Bonus Challenges | 50-100 |

## 🏪 Upgrade Shop

| Upgrade | Cost | Effect |
|---------|------|--------|
| Revive | 300 | Extra life on current run |
| Checkpoint Return | 500 | Return to last checkpoint if caught |
| Enhanced Vision | 200 | See guard patrol routes |
| Silent Steps | 250 | Reduced detection radius |
| Extra Speed | 200 | +20% movement speed |

## 🏆 Leaderboard Categories

1. **Win Streaks**: Most consecutive successful escapes
2. **Day Streaks**: Most consecutive daily plays
3. **Highest Level**: Maximum player level achieved
4. **Total Coins**: Lifetime coin accumulation
5. **Best Time**: Fastest escape completion

## 🎬 Cutscene Details

### Opening Sequence
- Player walking through city at night
- Bright light appears overhead
- Tractor beam activates
- Player gets pulled up and knocked out (blur effect)
- Fade to black
- Wake up in alien cell

## 🚀 Getting Started

### Prerequisites
- Unity 2022 LTS or higher
- Git LFS (for large video/model files)

### Installation
```bash
git clone https://github.com/theRealJosef-M/alien-escape-3d-game.git
cd alien-escape-3d-game
git lfs pull
# Open in Unity Hub
```

### First Run
1. Open Main Menu scene
2. Select character gender
3. Choose difficulty
4. Enjoy the opening cutscene!

## 📝 Development Roadmap

- [x] Project initialization
- [ ] Main menu and UI system
- [ ] Character customization (gender selection)
- [ ] Opening cutscene video
- [ ] 3D level design (4 difficulty variants)
- [ ] Player controller and movement
- [ ] Enemy AI system
- [ ] Catch/knockout mechanic and blur effect
- [ ] Coin and progression system
- [ ] Upgrade shop
- [ ] Level up system (1-999)
- [ ] Leaderboard system
- [ ] Settings and save system
- [ ] Audio implementation
- [ ] Visual effects and polish
- [ ] Performance optimization
- [ ] Platform builds (PC, Console, Mobile)

## 🤝 Contributing

This is a solo project for now, but contributions are welcome! Please create a branch and submit a pull request.

## 📜 License

MIT License - feel free to use and modify

## 👨‍💻 Developer

**theRealJosef-M**

---

**May you escape the aliens... or become one of them.** 👽🛸
