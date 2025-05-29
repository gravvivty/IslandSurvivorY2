using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Managers;
using SWEN_Game._PlayerData;

namespace SWEN_Game._Items
{
    /// <summary>
    /// Stores all Player info regarding the actual Game like Speed, Health, Damage, etc.
    /// </summary>
    public class PlayerGameData : IPlayerStats
    {
        private static PlayerGameData _instance;
        public static PlayerGameData Instance => _instance;

        public IWeapon CurrentWeapon;
        public IWeapon BaseWeapon;
        public Texture2D BulletTexture;
        public Color BulletTint;

        public int CurrentHealth = 6;
        public int MaxHealth = 6;
        public float Speed = 130f;
        public Dictionary<int, Powerup> Powerups = new Dictionary<int, Powerup>();

        // Weapon Attribute Multipliers
        public float AttackSpeedMult = 1; // Higher Value -> Slower shooting
        public float ShotSpeedMult = 1;
        public float BulletSizeMult = 1;
        public float BulletSpreadMult = 1;
        public int BulletsPerShotMult = 1;
        public float BulletDamageMult = 1;

        // Weapon Attribute Flat Values
        public float AttackSpeedFlat = 0;
        public float ShotSpeedFlat = 0;
        public float BulletSizeFlat = 0;
        public float BulletSpreadFlat = 0;
        public int BulletsPerShotFlat = 0;
        public float BulletDamageFlat = 0;

        // Special Powerups
        public int ReverseShotLevel = 0;
        public int BulletPierceBonus = 0;
        public int MagazineSizeBonus = 0;
        public float ReloadSpeedBonus = 0; // Lower = faster
        public float SpeedBonus = 0;

        public PlayerGameData()
        {
            _instance = this;
        }

        public void AddAttackSpeedMult(float value) => _instance.AttackSpeedMult += value;
        public void AddShotSpeedMult(float value) => _instance.ShotSpeedMult += value;
        public void AddBulletSizeMult(float value) => _instance.BulletSizeMult += value;
        public void AddBulletSpreadMult(float value) => _instance.BulletSpreadMult += value;
        public void AddBulletsPerShotMult(int value) => _instance.BulletsPerShotMult += value;
        public void AddBulletDamageMult(float value) => _instance.BulletDamageMult += value;

        public void AddAttackSpeedFlat(float value) => _instance.AttackSpeedFlat += value;
        public void AddShotSpeedFlat(float value) => _instance.ShotSpeedFlat += value;
        public void AddBulletSizeFlat(float value) => _instance.BulletSizeFlat += value;
        public void AddBulletSpreadFlat(float value) => _instance.BulletSpreadFlat += value;
        public void AddBulletsPerShotFlat(int value) => _instance.BulletsPerShotFlat += value;
        public void AddBulletDamageFlat(float value) => _instance.BulletDamageFlat += value;

        public void AddBulletPierceBonus(int value) => _instance.BulletPierceBonus += value;
        public void AddMagazineSizeBonus(int value) => _instance.MagazineSizeBonus += value;
        public void AddMaxHealth(int value) => _instance.MaxHealth += value;
        public void AddReloadSpeedBonus(float value) => _instance.ReloadSpeedBonus += value;
        public void AddSpeedBonus(float value) => _instance.SpeedBonus += value;
        public int GetCurrentHealth() => _instance.CurrentHealth;
        public void SetCurrentHealth(int value) => _instance.CurrentHealth = value;

        /// <summary>
        /// Updates the current weapon's attributes based on the multipliers and flat values.
        /// </summary>
        public void UpdateWeaponGameData()
        {
            UpdatePlayerGameData();

            // Update the current weapon with the new multipliers and flat values
            CurrentWeapon.AttackSpeed = BaseWeapon.AttackSpeed * AttackSpeedMult + AttackSpeedFlat;
            CurrentWeapon.ShotSpeed = BaseWeapon.ShotSpeed * ShotSpeedMult + ShotSpeedFlat;
            CurrentWeapon.BulletSize = BaseWeapon.BulletSize * BulletSizeMult + BulletSizeFlat;
            CurrentWeapon.BulletSpread = BaseWeapon.BulletSpread * BulletSpreadMult + BulletSpreadFlat;
            CurrentWeapon.BulletsPerShot = BaseWeapon.BulletsPerShot * BulletsPerShotMult + BulletsPerShotFlat;
            CurrentWeapon.BulletDamage = BaseWeapon.BulletDamage * BulletDamageMult + BulletDamageFlat;

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
        private void UpdatePlayerGameData()
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