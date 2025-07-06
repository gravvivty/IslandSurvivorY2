using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Items;

namespace SWEN_Game._Shooting
{
    public class Weapon : IWeapon
    {
        public virtual float AttackSpeed { get; set; }
        public virtual float ShotSpeed { get; set; }
        public virtual float BulletSize { get; set; }
        public virtual float BulletSpread { get; set; }
        public virtual int BulletsPerShot { get; set; }
        public virtual float BulletDamage { get; set; }
        public virtual float TimeSinceLastShot { get; set; }
        public virtual int MagazineSize { get; set; }
        public virtual int CurrentAmmo { get; set; }
        public virtual bool IsReloading { get; set; } = false;
        public virtual float ReloadTime { get; set; } = 1.0f;
        public virtual float ReloadTimer { get; set; } = 0f;
        public virtual int Pierce { get; set; } = 0;
        public virtual string Name { get; set; }

        public virtual Texture2D BulletTexture { get; set; }
        public virtual Texture2D IngameSprite { get; set; }
        public virtual Texture2D IconSprite { get; set; }

        public Weapon(string name, float attackspeed, float shotspeed, float bulletSize, float bulletSpread, int bulletsPerShot, float bulletDamage, int magSize, float reloadTime, int pierce, Texture2D bulletTexture, Texture2D icon, Texture2D ingame)
        {
            this.Name = name;
            this.AttackSpeed = attackspeed;
            this.ShotSpeed = shotspeed;
            this.BulletSize = bulletSize;
            this.BulletSpread = bulletSpread;
            this.BulletsPerShot = bulletsPerShot;
            this.BulletDamage = bulletDamage;
            this.MagazineSize = magSize;
            this.CurrentAmmo = this.MagazineSize;
            this.ReloadTime = reloadTime;
            this.Pierce = pierce;

            this.BulletTexture = bulletTexture;
            this.IngameSprite = ingame;
            this.IconSprite = icon;
        }

        /// <summary>
        /// Copies the weapon so they are completely independent.
        /// </summary>
        /// /// <remarks>
        /// Had to implement this since there was some pointer reference stuff that did not work.
        /// </remarks>
        /// <returns>A copied weapon.</returns>
        public Weapon Clone()
        {
            Weapon clone = new Weapon(
                this.Name,
                this.AttackSpeed,
                this.ShotSpeed,
                this.BulletSize,
                this.BulletSpread,
                this.BulletsPerShot,
                this.BulletDamage,
                this.MagazineSize,
                this.ReloadTime,
                this.Pierce,
                this.BulletTexture,
                this.IconSprite,
                this.IngameSprite);
            return clone;
        }
    }
}
