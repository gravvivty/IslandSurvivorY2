﻿using SWEN_Game._Interfaces;
using SWEN_Game._Items._ItemData;
using SWEN_Game._Items._Modifiers;
using SWEN_Game._Shooting;

namespace SWEN_Game._Items
{
    public class PowerupManager
    {
        public Dictionary<int, Powerup> savedPowerups = new Dictionary<int, Powerup>();
        private PlayerWeapon weapon;
        private IPlayerStats stats;

        public PowerupManager(PlayerWeapon playerWpn, IPlayerStats playerStats)
        {
            weapon = playerWpn;
            stats = playerStats;
        }

        /// <summary>
        /// Adds or levels up an item with itemID to the powerup list of the player as well as registering the Weapon Modifier if the powerup is one.
        /// </summary>
        /// <param name="itemID">ID of the item.</param>
        public void AddItem(int itemID)
        {
            RetrieveCurrentItems();

            int newLevel = 1;

            if (savedPowerups.TryGetValue(itemID, out Powerup existingPowerup))
            {
                if (existingPowerup.Level < 3)
                {
                    newLevel = existingPowerup.Level + 1;
                }
                else
                {
                    return; // max level
                }
            }

            Powerup newPowerup = CreatePowerup(itemID, newLevel);
            PlayerGameData.Instance.Powerups[itemID] = newPowerup;

            // Modifier logic
            RegisterModifier(itemID, newLevel);
        }

        /// <summary>
        /// Creates a new powerup based on itemID and level.
        /// </summary>
        /// <param name="itemID">ID of the item.</param>
        /// <param name="level">Level of the powerup.</param>
        /// <returns>A new Powerup with a level.</returns>
        /// <exception cref="ArgumentException">Powerup does not exist.</exception>
        private Powerup CreatePowerup(int itemID, int level)
        {
            switch (itemID)
            {
                case 1:
                    return new GunpowderPowerup(level, stats);
                case 2:
                    return new MultiShotPowerup(level, stats);
                case 3:
                    return new PiercerPowerup(level, stats);
                case 4:
                    return new AdrenalinePowerup(level, stats);
                case 5:
                    return new RocketspeedPowerup(level, stats);
                case 6:
                    return new RancidEnergyDrinkPowerup(level, stats);
                case 7:
                    return new ShadowBulletsPowerup(level, stats);
                case 8:
                    return new QuickHandsPowerup(level, stats);
                case 9:
                    return new SpicyNoodlesPowerup(level, stats);
                case 10:
                    return new DeadeyePowerup(level, stats);
                case 11:
                    return new HeavyMagsPowerup(level, stats);
                case 12:
                    return new ExtremeTeapowderPowerup(level, stats);
                case 13:
                    return new FrozenTearsPowerup(level, stats);
                case 14:
                    return new SpeedColaPowerup(level, stats);

                // Add more item cases here
                default:
                    throw new ArgumentException($"Unknown itemID: {itemID}");
            }
        }

        /// <summary>
        /// Retrieves the current Powerups the player has.
        /// </summary>
        /// <remarks>
        /// This should get called at the start of adding an Item to get the most recent changes.
        /// </remarks>
        private void RetrieveCurrentItems()
        {
            savedPowerups = PlayerGameData.Instance.Powerups;
        }

        /// <summary>
        /// Creates or levels up the Modifiers for the weapon.
        /// </summary>
        /// <remarks>
        /// Treat it as a powerup but since these need custom code that manipulate bullets it is something extra.
        /// </remarks>
        /// <param name="itemID">ID of the item.</param>
        /// <param name="level">Level of the item.</param>
        private void RegisterModifier(int itemID, int level)
        {
            List<IWeaponModifier> modifiers = weapon.GetModifiers();
            bool found = false;

            switch (itemID)
            {
                case 2: // ReverseShot

                    foreach (var mod in modifiers)
                    {
                        if (mod is MultiShotModifier multiMod)
                        {
                            multiMod.SetMultiShotModifier(level);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        MultiShotModifier newReverseShotMod = new MultiShotModifier();
                        newReverseShotMod.SetMultiShotModifier(level);
                        weapon.AddModifier(newReverseShotMod);
                    }

                    break;
                case 7: // ShadowBullets

                    foreach (var mod in modifiers)
                    {
                        if (mod is ShadowBulletsModifier demonMod)
                        {
                            demonMod.SetShadowBulletsModifier(level);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ShadowBulletsModifier newDemonBulletsMod = new ShadowBulletsModifier();
                        newDemonBulletsMod.SetShadowBulletsModifier(level);
                        weapon.AddModifier(newDemonBulletsMod);
                    }

                    break;

                // Add other powerup cases

                default:
                    break;
            }
        }
    }
}