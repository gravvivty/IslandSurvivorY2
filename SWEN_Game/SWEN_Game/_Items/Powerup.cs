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

        public int Level { get; protected set; } = 1;
        protected int itemID = 0;

        public void UpdatePlayerGameDataValues()
        {
            PlayerGameData.AttackSpeedMult += attackSpeedMult;
            PlayerGameData.ShotSpeedMult += shotSpeedMult;
            PlayerGameData.BulletSizeMult += bulletSizeMult;
            PlayerGameData.BulletSpreadMult += bulletSpreadMult;
            PlayerGameData.BulletsPerShotMult += bulletsPerShotMult;
            PlayerGameData.BulletDamageMult += bulletDamageMult;

            PlayerGameData.AttackSpeedFlat += attackSpeedFlat;
            PlayerGameData.ShotSpeedFlat += shotSpeedFlat;
            PlayerGameData.BulletSizeFlat += bulletSizeFlat;
            PlayerGameData.BulletSpreadFlat += bulletSpreadFlat;
            PlayerGameData.BulletsPerShotFlat += bulletsPerShotFlat;
            PlayerGameData.BulletDamageFlat += bulletDamageFlat;

            PlayerGameData.BulletPierce += bulletPierce;
        }
    }
}
