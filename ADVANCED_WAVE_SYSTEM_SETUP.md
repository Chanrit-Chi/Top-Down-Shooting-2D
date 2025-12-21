# Advanced Wave System Setup Guide (Option 2)

## Overview

Three different wave modes:

1. **Predefined Waves Only** - Fixed waves you design
2. **Predefined + Infinite** - Your waves, then unlimited with scaling
3. **Infinite Waves** - Endless waves with automatic difficulty scaling

---

## Wave Mode Selection

### Mode 1: Predefined Waves Only

```
Use Predefined Waves: ✅ (enabled)
Infinite Waves After Predefined: ❌ (disabled)
```

Game ends after last predefined wave.

### Mode 2: Predefined + Infinite (RECOMMENDED)

```
Use Predefined Waves: ✅ (enabled)
Infinite Waves After Predefined: ✅ (enabled)
```

Use your custom waves, then infinite scaling after.

### Mode 3: Pure Infinite Waves

```
Use Predefined Waves: ❌ (disabled)
Predefined Waves Array: (leave empty)
```

Waves start with 3 enemies, +1 per wave, with auto-scaling difficulty.

---

## Step 1: Create Enemy Prefabs

Make sure you have separate prefabs for each enemy type:

- Enemy_Type_A
- Enemy_Type_B
- Enemy_Type_C
  (Each should have EnemyHealth, EnemyAttack, EnemyMovement, EnemyAwareController, and EnemyDeathNotifier scripts)

---

## Step 2: Create AdvancedWaveManager GameObject

1. Right-click in Hierarchy → Create Empty
2. Name it **"AdvancedWaveManager"**
3. Add two scripts to it:
   - **AdvancedWaveManager**
   - **AdvancedEnemySpawner**

---

## Step 3: Configure AdvancedWaveManager

In the Inspector, set:

### Basic Settings (All Modes)

- **Delay Between Waves**: 3 (seconds)
- **Initial Enemies Per Wave**: 3 (only for infinite mode)
- **Enemy Health Multiplier**: 1.1 (10% increase per wave)
- **Enemy Damage Multiplier**: 1.05 (5% increase per wave)
- **Randomize Enemy Types In Unlimited**: ✅ (mixes enemy types in infinite waves)

### For Predefined Waves (Mode 1 & 2)

- **Use Predefined Waves**: ✅ (enabled)
- **Infinite Waves After Predefined**: ✅ for Mode 2, ❌ for Mode 1

**Example Configuration:**

```
Predefined Waves: Size 5

Wave 0:
  Wave Number: 1
  Spawn Delay Override: 0.5
  Enemy Types: Size 1
    [0] Enemy Prefab: Enemy_Type_A, Count: 3

Wave 1:
  Wave Number: 2
  Spawn Delay Override: 0.5
  Enemy Types: Size 2
    [0] Enemy Prefab: Enemy_Type_A, Count: 2
    [1] Enemy Prefab: Enemy_Type_B, Count: 2

Wave 2:
  Wave Number: 3
  Spawn Delay Override: 0.4
  Enemy Types: Size 3
    [0] Enemy Prefab: Enemy_Type_A, Count: 1
    [1] Enemy Prefab: Enemy_Type_B, Count: 2
    [2] Enemy Prefab: Enemy_Type_C, Count: 2

Wave 3:
  Wave Number: 4
  Spawn Delay Override: 0.4
  Enemy Types: Size 3
    [0] Enemy Prefab: Enemy_Type_A, Count: 0
    [1] Enemy Prefab: Enemy_Type_B, Count: 3
    [2] Enemy Prefab: Enemy_Type_C, Count: 3

Wave 4:
  Wave Number: 5
  Spawn Delay Override: 0.3
  Enemy Types: Size 3
    [0] Enemy Prefab: Enemy_Type_B, Count: 2
    [1] Enemy Prefab: Enemy_Type_C, Count: 4
```

---

## Step 4: Configure AdvancedEnemySpawner

In the Inspector, set:

### Enemy Prefabs

Set array size to match your enemy types and assign them:

- [0]: Enemy_Type_A
- [1]: Enemy_Type_B
- [2]: Enemy_Type_C

### Spawning Settings

- **Spawn Delay**: 0.5 (default)
- **Use Random Spawning**: ✅ (enabled)
- **Random Spawn Radius**: 10 (units)
- **Spawn Center**: (leave empty, uses AdvancedWaveManager position)

### Randomize Enemy Types

- ❌ (disabled - we're using predefined waves, or Mode 3 uses the AdvancedWaveManager setting)

---

## Step 5: Update HealthUI (if needed)

The updated HealthUI automatically detects both systems. Just make sure:

1. Your Canvas has TextMeshPro text elements for:
   - Player health
   - Enemy count
   - Wave number

---

## Customization Examples

### Mode 3: Infinite Waves - Custom Difficulty Curve

Slower difficulty growth:

```
Initial Enemies Per Wave: 3
Enemy Health Multiplier: 1.05 (was 1.1)
Enemy Damage Multiplier: 1.02 (was 1.05)
```

Faster difficulty growth:

```
Initial Enemies Per Wave: 5
Enemy Health Multiplier: 1.2 (was 1.1)
Enemy Damage Multiplier: 1.1 (was 1.05)
```

### Mode 2: Design 5 Predefined Waves, Then Infinite

Set up your 5 custom waves, then:

```
Use Predefined Waves: ✅
Infinite Waves After Predefined: ✅
```

Wave 6+: Automatically scaled with difficulty

---

## How It Works

### Mode 1 (Predefined Only)

```
Wave 1-5: Use your custom configurations
Wave 6+: Game stops or loops back
```

### Mode 2 (Predefined + Infinite) ⭐ RECOMMENDED

```
Wave 1-5: Use your custom configurations
Wave 6+:
  - Wave 6: 4 enemies (3 + 1)
  - Wave 7: 5 enemies (3 + 2)
  - Wave 8: 6 enemies (3 + 3)
  Health: baseHealth × (1.1)^(wave-1)
  Damage: baseDamage × (1.05)^(wave-1)
```

### Mode 3 (Pure Infinite)

```
Wave 1: 3 enemies (initial)
Wave 2: 4 enemies (+1)
Wave 3: 5 enemies (+1)
...continues forever
```

---

## Debug Output

### Mode 2 Example

```
=== WAVE 1 STARTED (Predefined) ===
Spawned Enemy_Type_A (HP: x1.00, DMG: x1.00)
...
Wave 1 complete! Next wave in 3s...

=== WAVE 6 STARTED ===
Predefined waves complete! Continuing with infinite scaled waves...
Spawned Enemy_Type_A (HP: x1.61, DMG: x1.28)
Spawned Enemy_Type_B (HP: x1.61, DMG: x1.28)
Spawned Enemy_Type_C (HP: x1.61, DMG: x1.28)
Spawned Enemy_Type_A (HP: x1.61, DMG: x1.28)
```

---

## Troubleshooting

**Q: Enemies not spawning?**
A: Check that Enemy Prefabs array in AdvancedEnemySpawner has prefabs assigned

**Q: Wave won't end?**
A: Ensure all enemies have EnemyDeathNotifier script

**Q: Wrong enemy types spawning?**
A: Verify Predefined Waves are properly configured with correct prefab references

**Q: Spawning too fast/slow?**
A: Adjust "Spawn Delay Override" in each wave config or "Spawn Delay" in AdvancedEnemySpawner

**Q: Game too easy after predefined waves?**
A: Increase Enemy Health/Damage Multiplier or decrease Initial Enemies Per Wave
