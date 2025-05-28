using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Items;
using SWEN_Game._Managers;
using SWEN_Game._Shooting;

namespace SWEN_Game._Entities
{
    /// <summary>
    /// Stores all Player info regarding the actual Game like Speed, Health, Damage, etc.
    /// </summary>
    public static class PlayerGameData
    {
        public static Weapon CurrentWeapon;
        public static Weapon BaseWeapon;
        public static Texture2D BulletTexture;
        public static Color BulletTint;

        public static int CurrentHealth = 6;
        public static int MaxHealth = 6;
        public static float Speed = 130f;
        public static Dictionary<int, Powerup> Powerups = new Dictionary<int, Powerup>();

        // Weapon Attribute Multipliers
        public static float AttackSpeedMult = 1; // Higher Value -> Slower shooting
        public static float ShotSpeedMult = 1;
        public static float BulletSizeMult = 1;
        public static float BulletSpreadMult = 1;
        public static int BulletsPerShotMult = 1;
        public static float BulletDamageMult = 1;

        // Weapon Attribute Flat Values
        public static float AttackSpeedFlat = 0;
        public static float ShotSpeedFlat = 0;
        public static float BulletSizeFlat = 0;
        public static float BulletSpreadFlat = 0;
        public static int BulletsPerShotFlat = 0;
        public static float BulletDamageFlat = 0;

        // Special Powerups
        public static int ReverseShotLevel = 0;
        public static int BulletPierceBonus = 0;
        public static int MagazineSizeBonus = 0;
        public static float ReloadSpeedBonus = 0; // Lower = faster
        public static float SpeedBonus = 0;

        /// <summary>
        /// Updates the current weapon's attributes based on the multipliers and flat values.
        /// </summary>
        public static void UpdateWeaponGameData()
        {
            UpdatePlayerGameData();

            // Update the current weapon with the new multipliers and flat values
            CurrentWeapon.AttackSpeed = (BaseWeapon.AttackSpeed * AttackSpeedMult) + AttackSpeedFlat;
            CurrentWeapon.ShotSpeed = (BaseWeapon.ShotSpeed * ShotSpeedMult) + ShotSpeedFlat;
            CurrentWeapon.BulletSize = (BaseWeapon.BulletSize * BulletSizeMult) + BulletSizeFlat;
            CurrentWeapon.BulletSpread = (BaseWeapon.BulletSpread * BulletSpreadMult) + BulletSpreadFlat;
            CurrentWeapon.BulletsPerShot = (int)((BaseWeapon.BulletsPerShot * BulletsPerShotMult) + BulletsPerShotFlat);
            CurrentWeapon.BulletDamage = (BaseWeapon.BulletDamage * BulletDamageMult) + BulletDamageFlat;

            CurrentWeapon.ReloadTime = BaseWeapon.ReloadTime + ReloadSpeedBonus;
            CurrentWeapon.Pierce = BaseWeapon.Pierce + BulletPierceBonus;

            float newSpeed = Speed + SpeedBonus;
            InputManager.SetSpeed(newSpeed);
        }

        /// <summary>
        /// Resets the additional upgrades and adds them back going through the collection of powerups.
        /// </summary>
        /// <remarks>
        /// Happens every time the player gains a new powerup.
        /// </remarks>
        private static void UpdatePlayerGameData()
        {
            MaxHealth = 6;

            // Reset to base before applying powerups
            AttackSpeedMult = 1;
            ShotSpeedMult = 1;
            BulletSizeMult = 1;
            BulletSpreadMult = 1;
            BulletsPerShotMult = 1;
            BulletDamageMult = 1;

            AttackSpeedFlat = 0;
            ShotSpeedFlat = 0;
            BulletSizeFlat = 0;
            BulletSpreadFlat = 0;
            BulletsPerShotFlat = 0;
            BulletDamageFlat = 0;

            ReverseShotLevel = 0;
            BulletPierceBonus = 0;
            MagazineSizeBonus = 0;
            ReloadSpeedBonus = 0;
            SpeedBonus = 0;

            foreach (var powerup in Powerups.Values)
            {
                powerup.UpdatePlayerGameDataValues();
            }
        }
    }
}