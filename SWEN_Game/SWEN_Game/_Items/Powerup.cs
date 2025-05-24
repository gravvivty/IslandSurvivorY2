using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game
{
    public class Powerup
    {
        // Weapon Attribute Multipliers
        protected float attackSpeedMult = 1;
        protected float shotSpeedMult = 1;
        protected float bulletSizeMult = 1;
        protected float bulletSpreadMult = 1;
        protected int bulletsPerShotMult = 1;
        protected float bulletDamageMult = 1;

        // Weapon Attribute Flat Values
        protected float attackSpeedFlat = 0;
        protected float shotSpeedFlat = 0;
        protected float bulletSizeFlat = 0;
        protected float bulletSpreadFlat = 0;
        protected int bulletsPerShotFlat = 0;
        protected float bulletDamageFlat = 0;

        public int Level { get; protected set; } = 1;
        protected int itemID = 0;

        protected void UpdatePlayerGameDataValues()
        {
            attackSpeedMult += PlayerGameData.AttackSpeedMult;
            shotSpeedMult += PlayerGameData.ShotSpeedMult;
            bulletSizeMult += PlayerGameData.BulletSizeMult;
            bulletSpreadMult += PlayerGameData.BulletSpreadMult;
            bulletsPerShotMult += PlayerGameData.BulletsPerShotMult;
            bulletDamageMult += PlayerGameData.BulletDamageMult;

            attackSpeedFlat += PlayerGameData.AttackSpeedFlat;
            shotSpeedFlat += PlayerGameData.ShotSpeedFlat;
            bulletSizeFlat += PlayerGameData.BulletSizeFlat;
            bulletSpreadFlat += PlayerGameData.BulletSpreadFlat;
            bulletsPerShotFlat += PlayerGameData.BulletsPerShotFlat;
            bulletDamageFlat += PlayerGameData.BulletDamageFlat;
        }
    }
}
