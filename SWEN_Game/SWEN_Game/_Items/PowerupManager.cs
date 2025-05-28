using SWEN_Game._Entities;
using SWEN_Game._Items._ItemData;
using SWEN_Game._Shooting;
using SWEN_Game._Shooting._Modifiers;
using System;
using System.Collections.Generic;

namespace SWEN_Game._Items
{
    public class PowerupManager
    {
        public Dictionary<int, Powerup> savedPowerups = new Dictionary<int, Powerup>();
        private PlayerWeapon weapon;

        public PowerupManager(PlayerWeapon playerWpn)
        {
            weapon = playerWpn;
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
            PlayerGameData.Powerups[itemID] = newPowerup;

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
                    return new GunpowderPowerup(level);
                case 2:
                    return new ReverseShotPowerup(level);
                case 3:
                    return new PiercerPowerup(level);
                case 4:
                    return new AdrenalinePowerup(level);
                case 5:
                    return new RocketspeedPowerup(level);
                case 6:
                    return new RancidEnergyDrinkPowerup(level);
                case 7:
                    return new DemonBulletsPowerup(level);
                case 8:
                    return new QuickHandsPowerup(level);
                case 9:
                    return new SpicyNoodlesPowerup(level);

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
            savedPowerups = PlayerGameData.Powerups;
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
                        if (mod is ReverseShotModifier reverseMod)
                        {
                            reverseMod.SetReverseShotModifier(level);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ReverseShotModifier newReverseShotMod = new ReverseShotModifier();
                        newReverseShotMod.SetReverseShotModifier(level);
                        weapon.AddModifier(newReverseShotMod);
                    }

                    break;
                case 7: // DemonBullets

                    foreach (var mod in modifiers)
                    {
                        if (mod is DemonBulletsModifier demonMod)
                        {
                            demonMod.SetDemonBulletsModifier(level);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        DemonBulletsModifier newDemonBulletsMod = new DemonBulletsModifier();
                        newDemonBulletsMod.SetDemonBulletsModifier(level);
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