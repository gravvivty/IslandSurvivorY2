using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public static class PlayerGameData
    {
        /*
        Weapon Class Attributes:
        private float attackSpeed;
        private float shotSpeed;
        private float bulletSize;
        private float bulletSpread;
        private int bulletsPerShot;
        private float bulletDamage;
        */
        public static Weapon CurrentWeapon;
        public static Weapon BaseWeapon;
        public static Texture2D BulletTexture;
        public static Color BulletTint;

        public static float Speed = 130f;
        public static int CurrentHealth = 6;
        public static int MaxHealth = 6;
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
        public static int BulletPierce = 0;
        public static int MagazineSizeBonus = 0;

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
        }

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
            BulletPierce = 0;
            MagazineSizeBonus = 0;

            foreach (var powerup in Powerups.Values)
            {
                powerup.UpdatePlayerGameDataValues();
            }
        }
    }
}