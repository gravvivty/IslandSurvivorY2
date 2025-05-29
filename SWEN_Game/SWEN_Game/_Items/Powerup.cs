using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWEN_Game._Entities;
using SWEN_Game._PlayerData;

namespace SWEN_Game._Items
{
    public class Powerup
    {
        protected IPlayerStats _playerStats;

        // Weapon Attribute Multipliers
        protected float attackSpeedMult = 0;
        protected float shotSpeedMult = 0;
        protected float bulletSizeMult = 0;
        protected float bulletSpreadMult = 0;
        protected int bulletsPerShotMult = 0;
        protected float bulletDamageMult = 0;

        // Weapon Attribute Flat Values
        protected float attackSpeedFlat = 0;
        protected float shotSpeedFlat = 0;
        protected float bulletSizeFlat = 0;
        protected float bulletSpreadFlat = 0;
        protected int bulletsPerShotFlat = 0;
        protected float bulletDamageFlat = 0;

        protected int bulletPierce = 0;
        protected int magazineSizeBonus = 0;
        protected int maxHealthBonus = 0;
        protected float reloadSpeedBonus = 0;
        protected float speedBonus = 0;

        public int Level { get; protected set; } = 1;
        protected int itemID = 0;

        public Powerup(IPlayerStats playerStats)
        {
            _playerStats = playerStats;
        }

        /// <summary>
        /// Updates the PlayerGameData Values of the Player with nem powerup values.
        /// </summary>
        public void UpdatePlayerGameDataValues()
        {
            _playerStats.AddAttackSpeedMult(attackSpeedMult);
            _playerStats.AddShotSpeedMult(shotSpeedMult);
            _playerStats.AddBulletSizeMult(bulletSizeMult);
            _playerStats.AddBulletSpreadMult(bulletSpreadMult);
            _playerStats.AddBulletsPerShotMult(bulletsPerShotMult);
            _playerStats.AddBulletDamageMult(bulletDamageMult);

            _playerStats.AddAttackSpeedFlat(attackSpeedFlat);
            _playerStats.AddShotSpeedFlat(shotSpeedFlat);
            _playerStats.AddBulletSizeFlat(bulletSizeFlat);
            _playerStats.AddBulletSpreadFlat(bulletSpreadFlat);
            _playerStats.AddBulletsPerShotFlat(bulletsPerShotFlat);
            _playerStats.AddBulletDamageFlat(bulletDamageFlat);

            _playerStats.AddBulletPierceBonus(bulletPierce);
            _playerStats.AddMagazineSizeBonus(magazineSizeBonus);
            _playerStats.AddReloadSpeedBonus(reloadSpeedBonus);
            _playerStats.AddSpeedBonus(speedBonus);
        }
    }
}
