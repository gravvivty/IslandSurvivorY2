using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Interfaces;
using SWEN_Game._Managers;
using SWEN_Game._Sound;

namespace SWEN_Game._Items
{
    /// <summary>
    /// Stores all Player info regarding the actual Game like Speed, Health, Damage, etc.
    /// </summary>
    public class PlayerGameData : IPlayerStats
    {
        private static PlayerGameData _instance;
        public static PlayerGameData Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Diagnostics.Debug.WriteLine("PlayerGameData could not be created.");
                }

                return _instance;
            }
        }

        public static void ResetInstanceForTests()
        {
            _instance = null;
        }

        public IWeapon CurrentWeapon;
        public IWeapon BaseWeapon;
        public Texture2D BulletTexture;
        public Color BulletTint;
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
        public float ReloadSpeedMultBonus = 1; // Lower = faster
        public float SpeedBonus = 0;
        public float CritChance = 0.05f;
        public float SlowChance = 0;

        public float PlayerSpeed { get; private set; } = 130f;
        private float xp = 0;
        private int level = 1;
        private int currentHealth = 12;
        private int maxHealth = 12;

        private IGameStateManager _gameStateManager;

        public PlayerGameData(IGameStateManager manager)
        {
            _instance = this;
            _gameStateManager = manager;
        }

        public int GetLevel() => _instance.level;
        public IWeapon GetWeapon() => _instance.CurrentWeapon;
        public IWeapon GetBaseWeapon() => _instance.BaseWeapon;
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
        public void AddMaxHealth(int value) => _instance.maxHealth += value;
        public void AddReloadSpeedBonus(float value) => _instance.ReloadSpeedBonus += value;
        public void AddReloadSpeedMultBonus(float value) => _instance.ReloadSpeedBonus += value;
        public void AddSpeedBonus(float value) => _instance.SpeedBonus += value;
        public void AddCritChance(float value) => _instance.CritChance += value;
        public void AddSlowChance(float value) => _instance.SlowChance += value;
        public int GetCurrentHealth() => _instance.currentHealth;
        public void SetCurrentHealth(int value) => _instance.currentHealth = value;
        public int GetMaxHealth() => _instance.maxHealth;
        public Texture2D SetBulletTexture(Texture2D value) => _instance.BulletTexture = value;
        public float GetXP() => _instance.xp;
        public void AddXP(float amount)
        {
            xp += amount;
            CheckLevelUp();
        }

        public float GetRequiredXPForLevel()
        {
            return 20f + (this.level * 75f) + (this.level * this.level * 5f);
        }

        /// <summary>
        /// Updates the current weapon's attributes based on the multipliers and flat values.
        /// </summary>
        public void UpdateWeaponGameData()
        {
            UpdatePlayerGameData();

            // Update the current weapon with the new multipliers and flat values
            float calculatedAttackSpeed = (BaseWeapon.AttackSpeed + AttackSpeedFlat) * AttackSpeedMult;
            CurrentWeapon.AttackSpeed = Math.Max(0.02f, calculatedAttackSpeed);
            CurrentWeapon.ShotSpeed = BaseWeapon.ShotSpeed * ShotSpeedMult + ShotSpeedFlat;
            CurrentWeapon.BulletSize = BaseWeapon.BulletSize * BulletSizeMult + BulletSizeFlat;
            CurrentWeapon.BulletSpread = BaseWeapon.BulletSpread * BulletSpreadMult + BulletSpreadFlat;
            CurrentWeapon.BulletsPerShot = BaseWeapon.BulletsPerShot * BulletsPerShotMult + BulletsPerShotFlat;
            CurrentWeapon.BulletDamage = BaseWeapon.BulletDamage * BulletDamageMult + BulletDamageFlat;

            float calculatedReloadTime = BaseWeapon.ReloadTime * ReloadSpeedMultBonus + ReloadSpeedBonus;
            CurrentWeapon.ReloadTime = Math.Max(0.25f, calculatedReloadTime);
            CurrentWeapon.Pierce = BaseWeapon.Pierce + BulletPierceBonus;
            CurrentWeapon.MagazineSize = BaseWeapon.MagazineSize + MagazineSizeBonus;

            PlayerSpeed = Speed + SpeedBonus;
        }

        /// <summary>
        /// Resets the additional upgrades and adds them back going through the collection of powerups.
        /// </summary>
        /// <remarks>
        /// Happens every time the player gains a new powerup.
        /// </remarks>
        private void UpdatePlayerGameData()
        {
            maxHealth = 12;

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
            ReloadSpeedMultBonus = 1;
            SpeedBonus = 0;
            CritChance = 0.05f;
            SlowChance = 0;
            PlayerSpeed = 130;

            foreach (var powerup in Powerups.Values)
            {
                powerup.UpdatePlayerGameDataValues();
            }
        }

        private void CheckLevelUp()
        {
            float requiredXP = GetRequiredXPForLevel();
            while (xp >= requiredXP)
            {
                xp -= requiredXP;
                level++;
                requiredXP = level * 200f;

                System.Diagnostics.Debug.WriteLine($"Player leveled up! New level: {level}");
                SFXManager.Instance.Play("levelUp");
                _gameStateManager.CaptureLastFrame();
                _gameStateManager.ChangeGameState(GameState.LevelUp);

                requiredXP = GetRequiredXPForLevel();
            }
        }
    }
}