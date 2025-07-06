# Vampire Survivors X Enter the Gungeon-Inspired MonoGame Project

###### Author: Steven Gayer (gravvivty)

###### Project Type: Software Engineering Coursework

<p align="center">
<img src="showcase.gif">
</p>

---

### Project Overview

This repository contains the source code and documentation for a 2D roguelike bullet-hell game inspired by *Vampire Survivors* and *Enter the Gungeon*. The game is developed using the MonoGame framework in C# as part of a Software Engineering module.

The player controls a character in a top-down 2D pixel-art world, fighting endless waves of enemies and unlocking upgrades through level-ups. The core gameplay loop revolves around surviving until the 10 Minute mark has been passed.

---

### Code Architecture

Below is an overview of the major namespaces and classes in the project.

---

#### Anims

> Provides a simple animation system for 2D sprites using a spritesheet. Includes:

 - **Animation**: Handles frame-based playback, timing, drawing.
 - **AnimationManager**: Manages and switches multiple animations for an entity.

---

#### Entities

> Defines all enemy, boss, and player entities in the game.

- **Enemies, Bosses, ShootingEntities**: Derived from a common `Enemy` base class, with unique stats and behaviors (e.g. SlimeBoss spawns extra enemies).  
- **Witch**: A ranged enemy shooting projectiles at the player with custom cooldown and bullet behavior.
- **EnemySpawning**: Manages enemy spawning logic, using weighted probabilities and stage-based progression via `EnemySpawner` and `EnemyStageManager`.
- **EnemyManager**: Spawns, updates, and draws all enemies, handles collision paired with `EnemyCollisionHandler`.

---

#### Player

> Manages player entity, stats, and mechanics.

- Handles movement, animations, health, invincibility frames, and player state logic.

---

#### Items

> Contains all power-ups and items that modify player abilities.

- **ItemData**: Defines individual power-ups like `AdrenalinePowerup` (increases attack speed) and `DeadeyePowerup` (boosts crit chance).
- **PowerupManager**: Adds, levels, and applies power-ups to influence gameplay dynamically.

---

#### Shooting

> Handles all shooting mechanics, bullet behavior, and weapon systems.

- **Modifiers**: Classes like `ShadowBulletsModifier` and `MultiShotModifier` alter shooting patterns and add unique bullet effects.
- **PlayerWeapon**: Manages shooting logic, ammo, reloading, and applies weapon modifiers.
- **WeaponManager**: Initializes and stores weapon data and textures.

---

#### Managers

> Core management systems for game functionality.

- **GameStateManager**: Handles transitions between game states and menus.
- **InputManager, MouseManager**: Manages player input.
- **GameManager**: Coordinates gameplay loop, updates, and rendering.

---

#### Sprite Management

> Systems for tile-based sprite rendering and depth sorting.

- **SpriteManager**: Maps tiles to positions, manages tilesets.
- **SpriteCalculator**: Computes depth values for layered sprites for proper rendering order.

---

#### UI

> Implements a modular UI system using the MLEM library.

- Includes main menu, options menu, pause menu, custom UI elements and various screens e.g. `LevelUpUI`.
- Supports dynamic resolution, window modes, and audio adjustments.

---

#### Debug

> Tools for debugging during development.

- Useful hotkeys:
    - `F1`: Assign 1 Level of each Item per press (see commented code in Debug.cs)
    - `F4`: Render debug overlays like hitboxes and collisions.

To spawn specific enemies/bosses at game start, adjust strings in:
```
_Entities.EnemySpawning → EnemyStageManager → StageIndex == 0
```
e.g. Insert `_spawner.SpawnEnemy("ENEMY_NAME", pos);` into the `else if` Statement (Line 63) and replace `ENEMY_NAME` with another enemy string type from the switch case in `EnemyFactory` (Starting Line 12).

---

### Installation

To build and run the project:

(The dependencies should already come with the project so no need to install anything on your own but incase it does not work:)
- Install MonoGame framework.
- Add required NuGet packages listed below.
- Build the solution.

---

### Resources

**Software:**

- [LDtk](https://ldtk.io/)

**MonoGame compatibility:**

- [LDtkMonogame](https://www.nuget.org/packages/LDtkMonogame/)
- [LDtkMonogame.Codegen](https://www.nuget.org/packages/LDtkMonogame.Codegen/)
- [LDtkMonogame.ContentPipeline](https://www.nuget.org/packages/LDtkMonogame.ContentPipeline/)

**UI:**

- [MLEM](https://www.nuget.org/packages/MLEM/)
- [MLEM.Ui](https://www.nuget.org/packages/MLEM.Ui)

**StyleCop:**

- [StyleCop.Analyzers](https://www.nuget.org/packages/StyleCop.Analyzers/)

**Art Assets:**

- Player: [axulart - Small 8-Direction Characters](https://axulart.itch.io/small-8-direction-characters)
- Tilesets:
    - [kenmi-art - Cute Fantasy RPG](https://kenmi-art.itch.io/cute-fantasy-rpg)
    - [Pixel-boy - Ninja Adventure Asset Pack](https://pixel-boy.itch.io/ninja-adventure-asset-pack)
- Enemies:
    - [phantomcooper - Free Slime](https://phantomcooper.itch.io/free-slime)
    - [bruno-farias - Sidescroller Character Boss](https://bruno-farias.itch.io/sidescroller-character-boss)
    - [merchant-shade - Myth Creatures](https://merchant-shade.itch.io/ph-myth-creatures)
    - [toadzillart - Monster Pack](https://toadzillart.itch.io/monster-pack)
    - [darkpixel-kronovi - Mecha Golem](https://darkpixel-kronovi.itch.io/mecha-golem-free)
- VFX: [bdragon1727 - Effects & Bullets](https://bdragon1727.itch.io/free-effect-and-bullet-16x16)
- Pickups: [greatdocbrown - Coins, Gems, etc.](https://greatdocbrown.itch.io/coins-gems-etc)
- Weapons: [zanipixels - Survival Horror](https://zanipixels.itch.io/survival-horror)
- UI: [redreeh - Pixel Hearts](https://redreeh.itch.io/pixelhearts-16x16)

**Music:**

- [Track 1](https://youtu.be/BZ6Cge1bRJ8?si=fJEk6YORfH0qlcq0)
- [Track 2](https://www.youtube.com/watch?v=mD3v1B_aXw0)
- Some tracks created by me :)

**SFX:**

- Some SFX created by me :)
- Some SFX from Toby Fox’s *Undertale*
- Some SFX from the `Dark Souls` Trilogy by FromSoftware

---
