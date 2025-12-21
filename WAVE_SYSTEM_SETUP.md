# Wave System Setup Guide

## Overview

The wave system automatically spawns waves of enemies with increasing difficulty each wave.

## Setup Instructions

### 1. Create an Empty GameObject for Wave Management

- Right-click in Hierarchy → Create Empty
- Name it "WaveManager"
- Add two scripts to it:
  - **WaveManager** (this component)
  - **EnemySpawner** (this component)

### 2. Configure WaveManager Component

In the Inspector, set:

- **Enemy Spawner**: Drag the **WaveManager** GameObject itself (the one you just created with the EnemySpawner script)
- **Delay Between Waves**: 3 (seconds to wait after wave ends)
- **Initial Enemies Per Wave**: 3 (starts with 3 enemies)
- **Enemy Health Multiplier**: 1.1 (10% more health each wave)
- **Enemy Damage Multiplier**: 1.05 (5% more damage each wave)

### 3. Configure EnemySpawner Component

- **Enemy Prefab**: Drag your enemy prefab here
- **Spawn Points**: Set array size and drag spawn point transforms
  - Create empty GameObjects in the scene as spawn points
  - Assign them to this array
- **Spawn Delay**: 0.5 (seconds between spawning each enemy)

### 4. Add EnemyDeathNotifier to Enemy Prefab

- Select your enemy prefab
- Add the **EnemyDeathNotifier** script component

### 5. Add UI Display (Optional)

- Create a Canvas with two TextMeshPro text elements
- One for wave number, one for enemy count
- Add **WaveUI** script to an object in the Canvas
- Assign the text elements in the Inspector

## How It Works

1. **Wave 1**: Spawns 3 enemies with normal stats
2. **Wave 2**: Spawns 4 enemies with 10% more health and 5% more damage
3. **Wave 3**: Spawns 5 enemies with 21% more health and 10.25% more damage
4. And so on...

## Difficulty Scaling Formula

- **Health**: baseHealth × (1.1)^(wave-1)
- **Damage**: baseDamage × (1.05)^(wave-1)

## Customization

### Slower Difficulty Increase

Change multipliers to smaller values:

```
Enemy Health Multiplier: 1.05 (instead of 1.1)
Enemy Damage Multiplier: 1.02 (instead of 1.05)
```

### Faster Difficulty Increase

Change multipliers to larger values:

```
Enemy Health Multiplier: 1.2
Enemy Damage Multiplier: 1.1
```

### More/Fewer Enemies Per Wave

Change "Initial Enemies Per Wave" or modify the formula in WaveManager.cs line 43:

```csharp
int enemiesToSpawn = initialEnemiesPerWave + (currentWave - 1);
// Change to:
int enemiesToSpawn = initialEnemiesPerWave + (currentWave - 1) * 2; // +2 per wave instead of +1
```

## Debug Console Output

The system logs wave progress to the Console:

- "=== WAVE X STARTED ===" - Wave begins
- "Enemy defeated! Remaining: X" - Enemy count
- "Wave X complete! Next wave in 3s..." - Wave completed
