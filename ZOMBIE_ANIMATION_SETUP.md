# Zombie Enemy Animation Setup Guide

## Overview

How to set up animations for your zombie enemy (idle, move, attack).

---

## Step 1: Prepare Your Animations

1. Import your animation clips into Unity:

   - `Zombie_Idle.anim`
   - `Zombie_Move.anim`
   - `Zombie_Attack.anim`

2. Create an Animator Controller:
   - Right-click in Assets/Animation folder
   - Create → Animator Controller
   - Name it `ZombieAnimator`

---

## Step 2: Set Up the Animator Controller

1. Double-click `ZombieAnimator` to open it
2. Create 3 states:

   - Right-click → Add State → Empty
   - Name them: `Idle`, `Move`, `Attack`

3. Set the default state to `Idle`:

   - Right-click on `Idle` → Set as Layer Default State

4. Assign animations:
   - Select each state and drag the animation to the Motion field
   - `Idle` → Zombie_Idle
   - `Move` → Zombie_Move
   - `Attack` → Zombie_Attack

---

## Step 3: Create Transitions

### Idle → Move

1. Right-click `Idle` → Make Transition → `Move`
2. Click the transition arrow, in Inspector:
   - Add condition: `isMoving` (bool) = true
   - Uncheck "Has Exit Time"
   - Transition Duration: 0.1

### Move → Idle

1. Right-click `Move` → Make Transition → `Idle`
2. Add condition: `isMoving` (bool) = false
3. Uncheck "Has Exit Time"
4. Transition Duration: 0.1

### Move → Attack (and Idle → Attack)

1. Right-click `Move` → Make Transition → `Attack`
2. Add condition: `isAttacking` (bool) = true
3. Uncheck "Has Exit Time"
4. Transition Duration: 0.05

5. Right-click `Idle` → Make Transition → `Attack`
6. Add condition: `isAttacking` (bool) = true
7. Uncheck "Has Exit Time"
8. Transition Duration: 0.05

### Attack → Idle (and Attack → Move)

1. Right-click `Attack` → Make Transition → `Idle`
2. Add condition: `isAttacking` (bool) = false
3. Check "Has Exit Time" = 0.9
4. Transition Duration: 0.1

5. Right-click `Attack` → Make Transition → `Move`
6. Add condition: `isAttacking` (bool) = false
7. Check "Has Exit Time" = 0.9
8. Transition Duration: 0.1

**Parameters to create:**

- `isMoving` (bool)
- `isAttacking` (bool)

---

## Step 4: Update Your Zombie Prefab

1. Select your Zombie prefab in the scene or Assets/Prefabs
2. In the Animator component:

   - Drag `ZombieAnimator` into the **Controller** field

3. Add the **EnemyAnimationController** script:

   - Add Component → EnemyAnimationController

4. In EnemyAnimationController Inspector:
   - **Attack Duration**: Set to match your attack animation length (e.g., 0.6 seconds)

---

## Step 5: Verify Components

Make sure your zombie has these scripts:

- ✅ EnemyMovement
- ✅ EnemyAttack
- ✅ EnemyAwareController
- ✅ EnemyHealth
- ✅ EnemyDeathNotifier
- ✅ **EnemyAnimationController** (NEW)
- Animator (with ZombieAnimator controller)
- Rigidbody2D

---

## How It Works

1. **Idle** - Enemy stands still, waiting
2. **Move** - When `isMoving = true`, plays walk animation while chasing player
3. **Attack** - When within attack range, plays attack animation
4. **Animation transitions back** - After attack animation finishes, returns to Idle/Move

---

## Customization

### Change Attack Duration

If your attack animation is 0.8 seconds instead of 0.5:

```
EnemyAnimationController → Attack Duration: 0.8
```

### Adjust Animation Speed

In each animation state in the Animator:

- Select the state
- In Inspector → Speed: 1.5 (faster) or 0.8 (slower)

### Add More States

You can add more animations (e.g., Death, Hit):

1. Create state in Animator Controller
2. Add transitions
3. Update EnemyAnimationController script to trigger them

---

## Troubleshooting

**Q: Animations not playing?**
A:

- Check Animator Controller is assigned in the Animator component
- Verify animation clips are assigned to states
- Check if parameters (isMoving, isAttacking) are correct type (bool)

**Q: Zombie stuck in one animation?**
A:

- Check transitions exist between states
- Verify conditions are set correctly
- Make sure "Has Exit Time" is checked/unchecked appropriately

**Q: Animations switching too fast?**
A:

- Increase Transition Duration in the animator
- Check "Has Exit Time" and adjust exit time value

**Q: Attack animation doesn't play?**
A:

- Verify Attack Duration matches your animation length
- Check if enemyAwareController.DistanceToPlayer is working (0.5f threshold)
- Increase attack range in EnemyAttack component if needed

---

## Animation State Flow Diagram

```
        ┌─────────────┐
        │    IDLE     │
        └──────┬──────┘
               │ isMoving=true
               ▼
        ┌─────────────┐
        │    MOVE     │
        └──────┬──────┘
               │ isMoving=false
               │ or isAttacking=true
               ▼
        ┌─────────────┐
        │   ATTACK    │
        └──────┬──────┘
               │ isAttacking=false + animation done
               ▼
        ┌─────────────┐
        └─ IDLE/MOVE ─┘
```
