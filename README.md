**This project is currently still ongoing. - the UI and Powerup Selection UI are missing**<br>
For our Software Engineering Course we had to develop a game within the MonoGame Framework in C#.<br>
I lead the entirety of the project while doing every code review (usually 1h online per feature).<br>
The entire code base has been engineered, supervised and mostly written by me (gravvivty / Steven Gayer).<br>
With the exception of the UI namespace and the GameStateManager class.

# Concept

This game is based on the mechanics and style of "Vampire Survivors."
The goal is to develop a 2D pixel art game with a top-down view, where the player can move around in a 2D space, shoot projectiles with a firearm, and occasionally select/unlock upgrades through level-ups, power-ups, modifiers, etc.

The game loop follows an arcade format, where the player fights endless waves of enemies to achieve a high score (possibly also unlocking items after a run, similar to a roguelite).

![Gameplay](showcase.gif)

# Architecture/Namespaces

## Anims

The SWEN_Game._Anims namespace provides a simple animation system for 2D sprites using a spritesheet.
It includes the Animation class, which manages frame-based animation playback (timing, frame switching, drawing), and the AnimationManager class, which allows an entity to manage and switch between multiple animations using keys.
This system supports starting, stopping, resetting animations, and drawing the current frame with optional color tint and depth sorting.

## Entities

### Bosses, Enemies, ShootingEntities

The SWEN_Game._Entities.Enemies namespace defines enemy and boss entities for the game, each inheriting from a common Enemy base class. It includes specific enemy types like Mummy, Baumbart, and the boss SlimeBoss, each with unique stats such as health, speed, and damage.
These classes also set up their own animations and, in the case of SlimeBoss, include custom behavior like spawning additional enemies over time.
This namespace is responsible for managing the visual appearance and behavior of hostile in-game characters.

This class differs a tiny bit from the regular enemies:
The Witch class in the SWEN_Game._Entities.Enemies namespace defines a ranged enemy that periodically shoots bullets at the player.
It inherits from Enemy and includes a shooting timer (shootCooldown, shootTimer) and a list of Bullet instances it fires.
The UpdateCustomBehavior method handles cooldown tracking and bullet updates, while the ShootAtPlayer method creates and adds a new bullet directed toward the player’s position.
The Draw method ensures both the enemy and its bullets are rendered. This design adds gameplay variety by introducing enemies with ranged attack behavior instead of purely melee movement.

### EnemySpawning

The SWEN_Game._Entities.EnemySpawning namespace manages the dynamic spawning of enemies during gameplay.
It includes the EnemySpawner class, which spawns enemies near the player at intervals based on weighted probabilities, and the EnemyStageManager, which controls enemy spawn behavior over time using predefined stages.
Each EnemySpawnStage defines spawn weights and active time windows, allowing the game to gradually increase difficulty and introduce new enemy types or bosses (like the SlimeBoss).
The IEnemyConsumer interface ensures that spawned enemies can be passed to systems like an EnemyManager for integration into the game world. Overall, this namespace orchestrates the progressive challenge escalation in enemy encounters.

### Rest of _Entities

The SWEN_Game._Entities namespace contains the core classes for managing game entities, including the abstract Enemy base class with movement, health, damage, and animation handling, and the EnemyManager that spawns, updates, and draws all enemies while handling collisions via the EnemyCollisionHandler.
It also includes the Player class, managing player state, movement, health, invincibility frames, and animations. Together, these classes form the foundation for entity behavior, interaction, and lifecycle within the game.

# Items

## ItemData

The SWEN_Game._Items._ItemData namespace defines specific power-up items that enhance player abilities, such as AdrenalinePowerup which improves attack speed, and DeadeyePowerup which increases critical hit chance.
Each power-up adjusts its effects based on level and interacts directly with the player’s stats, providing scalable upgrades to gameplay performance.

## Rest of _Items

The SWEN_Game._Items namespace manages the player's weapons and power-ups, including interfaces like IWeapon and core classes such as PlayerGameData that track player stats and dynamically update weapon attributes based on active power-ups.
The Powerup base class defines how upgrades affect player stats, while the PowerupManager handles adding, leveling, and applying power-ups, including specialized weapon modifiers that enhance gameplay mechanics.
This system enables scalable, modular enhancements to the player's combat capabilities.

# Managers

Disclaimer: These classes are tightly coupled - they have yet to be refactored if there is time within the project deadline

The SWEN_Game._Managers namespace contains core management classes that handle key game systems such as game state transitions (GameStateManager), player input (InputManager and MouseManager), and overall game logic coordination (GameManager).
It integrates player movement, shooting mechanics, enemy updates, rendering, and UI updates, providing a centralized control layer to update and draw the game each frame while managing interactions between entities, weapons, and the game world.

# SpriteManager and SpriteCalculator

These classes provide a core functionality for managing and rendering tile-based sprites in a game world. It includes the SpriteManager class, which organizes tiles by enum tags, loads tileset textures, maps tile IDs to world positions, and calculates rendering depth based on tile position and layer hierarchy.
Complementing this, the SpriteCalculator class computes depth values for grouped sprites by identifying anchor tiles near the player, enabling consistent and efficient layering of complex sprite groups for correct visual overlap during rendering.

# PlayerData

The SWEN_Game._PlayerData namespace defines the IPlayerStats interface, which standardizes how player-related stats—such as attack speed, shot speed, bullet properties, movement speed, health, and critical chance—are accessed and modified.
This interface enables clean decoupling between different game systems or classes that need to interact with player statistics, promoting modularity and easier maintenance by abstracting the underlying implementation details.

# Shooting

## Modifiers

The SWEN_Game._Shooting._Modifiers namespace contains weapon modifier classes that enhance shooting mechanics by altering bullet behavior.
For example, the DemonBulletsModifier spawns additional demon bullets upon bullet collision, with the number of extra bullets scaling by modifier level.
The ReverseShotModifier adds extra shots fired in reverse and perpendicular directions based on its level, enriching gameplay variety and strategic depth.
These modifiers implement the IWeaponModifier interface to allow modular and extensible weapon behavior customization.

## Rest of Shooting

The SWEN_Game._Shooting namespace implements a comprehensive player shooting system, including weapon management, bullet behavior, and weapon modifiers.
It features classes to handle bullet creation, movement, animation, collision detection, and piercing logic.
The PlayerWeapon class manages shooting mechanics, ammo, reloads, and applies modifiers that can alter shooting behavior dynamically.
WeaponManager initializes and stores various weapons with unique stats and textures, providing cloned instances for gameplay. Overall, this namespace encapsulates the core functionality for player weaponry and projectile handling in the game.

# UI

This namespace implements a modular UI system for a MonoGame project using the MLEM library, featuring a main menu, options menu, and pause menu.
It manages game state transitions through GameStateManager, supports fullscreen and borderless window modes, dynamic resolution changes, and adjustable audio volume via sliders.
The UIManager handles input, rendering, and updates, including a render target to display the paused game state faded in the background. Custom styled UI elements such as buttons, checkboxes, and sliders ensure a polished, responsive interface for the game.

# Debug

This namespace provides essential debugging tools for development, including keyboard-driven toggles for player invincibility and power-up activation.
It also visually renders debug overlays such as player hitboxes, collision boundaries, and precise position rectangles, aiding in collision detection and gameplay testing.
This utility enhances real-time feedback and troubleshooting during game development.<br>

F1 - Get items -> set them in Debug.cs, you'll see the commented code block
Space - Simulate getting hit by triggering iFrames
R - Reload Weapon
If you want to spawn enemies/bosses at the Start: <br>
_Entities.EnemySpawning -> EnemyStageManager Line 51 -> Replace "Witch" string with any of the strings from the switch case in EnemySpawner Line 48


# Resources

Software:<br>
https://ldtk.io/ <br>

MonoGame compatibility:<br>
https://www.nuget.org/packages/LDtkMonogame/ <br>
https://www.nuget.org/packages/LDtkMonogame.Codegen/ <br>
https://www.nuget.org/packages/LDtkMonogame.ContentPipeline/ <br>

UI:<br>
https://www.nuget.org/packages/MLEM/ <br>
https://www.nuget.org/packages/MLEM.Ui <br>

StyleCop:<br>
https://www.nuget.org/packages/StyleCop.Analyzers/ <br>

Player:<br>
https://axulart.itch.io/small-8-direction-characters <br>
Tilesets:<br>
https://kenmi-art.itch.io/cute-fantasy-rpg <br>
https://pixel-boy.itch.io/ninja-adventure-asset-pack <br>
Enemies:<br>
https://phantomcooper.itch.io/free-slime <br>
https://bruno-farias.itch.io/sidescroller-character-boss <br>
https://merchant-shade.itch.io/ph-myth-creatures <br>
https://toadzillart.itch.io/monster-pack <br>
https://darkpixel-kronovi.itch.io/mecha-golem-free <br>
VFX:<br>
https://bdragon1727.itch.io/free-effect-and-bullet-16x16 <br>
Pickups:<br>
https://greatdocbrown.itch.io/coins-gems-etc <br>
Weapons:<br>
https://zanipixels.itch.io/survival-horror <br>
UI:<br>
https://redreeh.itch.io/pixelhearts-16x16 <br>
