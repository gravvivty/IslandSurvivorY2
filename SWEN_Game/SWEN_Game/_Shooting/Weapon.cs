using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class Weapon
    {
        public float AttackSpeed {  get; set; }
        public float ShotSpeed { get; set; }
        public float BulletSize { get; set; }
        public float BulletSpread { get; set; }
        public int   BulletsPerShot { get; set; }
        public float BulletDamage { get; set; }
        public float TimeSinceLastShot { get; set; }
        public int MagazineSize { get; set; }
        public int CurrentAmmo { get; set; }
        public bool IsReloading { get; set; } = false;
        public float ReloadTime { get; set; } = 1.0f;
        public float ReloadTimer { get; set; } = 0f;

        public Texture2D bulletTexture;
        public Texture2D ingameSprite;
        public Texture2D iconSprite;

        public Weapon(float attackspeed, float shotspeed, float bulletSize, float bulletSpread, int bulletsPerShot, float bulletDamage, int magSize, Texture2D bulletTexture, Texture2D icon, Texture2D ingame)
        {
            this.AttackSpeed = attackspeed;
            this.ShotSpeed = shotspeed;
            this.BulletSize = bulletSize;
            this.BulletSpread = bulletSpread;
            this.BulletsPerShot = bulletsPerShot;
            this.BulletDamage = bulletDamage;
            this.MagazineSize = magSize;
            this.CurrentAmmo = this.MagazineSize;

            this.bulletTexture = bulletTexture;
            this.ingameSprite = ingame;
            this.iconSprite = icon;
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
                this.AttackSpeed,
                this.ShotSpeed,
                this.BulletSize,
                this.BulletSpread,
                this.BulletsPerShot,
                this.BulletDamage,
                this.MagazineSize,
                this.bulletTexture,
                this.iconSprite,
                this.ingameSprite);
            return clone;
        }
    }
}
