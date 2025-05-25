using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class PlayerWeapon
    {
        /*
        Weapon Class Attributes:
        private float attackSpeed;
        private float shotSpeed;
        private float bulletSize;
        private float bulletSpread;
        private int   bulletsPerShot;
        private float bulletDamage;
        private float timeSinceLastShot;

        */

        /*
        Weapon Names:
        - Pistol
        - SMG
        - Sniper
        */

        private List<Bullet> _bullets = new List<Bullet>();
        private List<IWeaponModifier> _modifiers = new List<IWeaponModifier>();
        public List<Bullet> GetBullets() => _bullets;

        private float _timeSinceLastShot;

        public PlayerWeapon(WeaponManager weaponManager)
        {
            PlayerGameData.BulletTexture = Globals.Content.Load<Texture2D>("Sprites/Bullets/VanillaBullet");
            PlayerGameData.BulletTint = new Color(25, 106, 150);
        }

        protected float TimeSinceLastShot
        {
            get => _timeSinceLastShot;
            set => _timeSinceLastShot = value;
        }

        public void Update()
        {
            float gametime = Globals.Time;
            TimeSinceLastShot += (float)gametime;

            for (int i = 0; i < _bullets.Count; i++)
            {
                _bullets[i].HasProcessedThisFrame = false;
                _bullets[i].Update();
            }

            _bullets.RemoveAll(bullet => !bullet.IsVisible);
        }

        public void AddModifier(IWeaponModifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public List<IWeaponModifier> GetModifiers()
        {
            return _modifiers;
        }

        public void DrawBullets()
        {
            foreach (var bullet in GetBullets())
            {
                bullet.Draw(Globals.SpriteBatch);
            }
        }

        public void Shoot(Vector2 direction, Vector2 player_position)
        {
            System.Diagnostics.Debug.WriteLine("PlayerWeapon is now Trying to shoot" + DateTime.Now);
            if (TimeSinceLastShot >= PlayerGameData.CurrentWeapon.attackSpeed)
            {
                ShootInDirection(direction, player_position);

                foreach (var mod in _modifiers)
                {
                    mod.OnShoot(direction, player_position, this);
                }

                System.Diagnostics.Debug.WriteLine("PlayerWeapon is now shooting" + DateTime.Now);
                TimeSinceLastShot = 0f;
            }
        }

        public void ShootInDirection(Vector2 direction, Vector2 player_position, bool? isDemonBullet = null)
        {
            Color tint = PlayerGameData.BulletTint;
            bool isChild = isDemonBullet ?? false;

            if (isChild)
            {
                tint = Color.Black;
            }

            Animation anim = new Animation(
                PlayerGameData.BulletTexture,
                1,
                4,
                0.1f,
                16,
                1,
                tint,
                PlayerGameData.CurrentWeapon.bulletSize);
            _bullets.Add(new Bullet(anim, player_position, direction, PlayerGameData.CurrentWeapon.shotSpeed, PlayerGameData.CurrentWeapon.bulletSize, PlayerGameData.BulletPierce, this, PlayerGameData.CurrentWeapon.bulletDamage, isChild));
        }
    }
}
