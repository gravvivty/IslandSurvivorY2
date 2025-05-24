using System;
using System.Collections.Generic;

namespace SWEN_Game
{
    public class PowerupManager
    {
        public Dictionary<int, Powerup> savedPowerups = new Dictionary<int, Powerup>();
        private PlayerWeapon weapon;

        public PowerupManager(PlayerWeapon playerWpn)
        {
            weapon = playerWpn;
        }

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

                // Add more item cases here
                default:
                    throw new ArgumentException($"Unknown itemID: {itemID}");
            }
        }

        private void RetrieveCurrentItems()
        {
            savedPowerups = PlayerGameData.Powerups;
        }

        private void RegisterModifier(int itemID, int level)
        {
            switch (itemID)
            {
                case 2: // ReverseShot
                    List<IWeaponModifier> modifiers = weapon.GetModifiers();
                    bool found = false;

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
                        ReverseShotModifier newMod = new ReverseShotModifier();
                        newMod.SetReverseShotModifier(level);
                        weapon.AddModifier(newMod);
                    }

                    break;

                // Add other powerup cases

                default:
                    break;
            }
        }
    }
}