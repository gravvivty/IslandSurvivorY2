using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game._Utils;
using SWEN_Game._Anims;
using SWEN_Game._Items;

namespace SWEN_Game._Shooting
{
    /// <summary>
    /// Represents a weapon used by the player, managing shooting, bullets, and weapon modifiers.
    /// </summary>
    public class PlayerWeapon : IPlayerWeapon
    {
        private List<Bullet> _bullets = new List<Bullet>();
        private List<IWeaponModifier> _modifiers = new List<IWeaponModifier>();
        public List<Bullet> GetBullets() => _bullets;

        private float _timeSinceLastShot;

        public PlayerWeapon(WeaponManager weaponManager)
        {
            PlayerGameData.Instance.BulletTexture = Globals.Content.Load<Texture2D>("Sprites/Bullets/VanillaBullet");
            PlayerGameData.Instance.BulletTint = Color.White;
        }

        /// <summary>
        /// Gets or sets the time since the last shot was fired.
        /// </summary>
        protected float TimeSinceLastShot
        {
            get => _timeSinceLastShot;
            set => _timeSinceLastShot = value;
        }

        /// <summary>
        /// Updates the state of the weapon, including bullets and reloading.
        /// </summary>
        public void Update()
        {
            float gametime = Globals.Time;
            TimeSinceLastShot += (float)gametime;

            Reload(gametime);

            for (int i = 0; i < _bullets.Count; i++)
            {
                _bullets[i].HasProcessedThisFrame = false;
                _bullets[i].Update();
            }

            _bullets.RemoveAll(bullet => !bullet.IsVisible);
        }

        /// <summary>
        /// Adds a modifier that can affect the weapon's shooting behavior.
        /// </summary>
        /// <param name="modifier">The modifier to add.</param>
        public void AddModifier(IWeaponModifier modifier)
        {
            _modifiers.Add(modifier);
        }

        /// <summary>
        /// Gets the list of weapon modifiers applied to this weapon.
        /// </summary>
        /// <returns>
        /// List of all Weapon Modifiers.
        /// </returns>
        public List<IWeaponModifier> GetModifiers()
        {
            return _modifiers;
        }

        /// <summary>
        /// Draws all currently active bullets.
        /// </summary>
        public void DrawBullets()
        {
            foreach (var bullet in GetBullets())
            {
                bullet.Draw(Globals.SpriteBatch);
            }
        }

        /// <summary>
        /// Attempts to shoot the weapon in a given direction from the player's position.
        /// </summary>
        /// <param name="direction">The direction to shoot in.</param>
        /// <param name="player_position">The position of the player.</param>
        public void Shoot(Vector2 direction, Vector2 player_position)
        {
            if (PlayerGameData.Instance.CurrentWeapon.IsReloading)
            {
                return;
            }

            if (TimeSinceLastShot >= PlayerGameData.Instance.CurrentWeapon.AttackSpeed)
            {
                if (PlayerGameData.Instance.CurrentWeapon.CurrentAmmo > 0)
                {
                    ShootInDirection(direction, player_position);
                    PlayerGameData.Instance.CurrentWeapon.CurrentAmmo--;

                    foreach (var mod in _modifiers)
                    {
                        mod.OnShoot(direction, player_position, this);
                    }

                    TimeSinceLastShot = 0f;
                }
                else
                {
                    // Start reloading
                    PlayerGameData.Instance.CurrentWeapon.IsReloading = true;
                    PlayerGameData.Instance.CurrentWeapon.ReloadTimer = 0f;

                    if (PlayerGameData.Instance.CurrentWeapon.CurrentAmmo == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Reloading...");
                    }
                }
            }
        }

        /// <summary>
        /// Instantiates and fires a bullet in a specified direction.
        /// </summary>
        /// <param name="direction">Direction to shoot the bullet.</param>
        /// <param name="player_position">Starting position of the bullet.</param>
        /// <param name="isDemonBullet">Optional flag to mark the bullet as a "demon" bullet.</param>
        public void ShootInDirection(Vector2 direction, Vector2 player_position, bool? isDemonBullet = null)
        {
            Color tint = PlayerGameData.Instance.BulletTint;
            bool isChild = isDemonBullet ?? false;

            if (isChild)
            {
                tint = Color.Black;
            }

            Animation anim = new Animation(
                PlayerGameData.Instance.BulletTexture,
                1,
                4,
                0.1f,
                16,
                16,
                1,
                tint,
                PlayerGameData.Instance.CurrentWeapon.BulletSize);
            _bullets.Add(
                new Bullet(
                    anim,
                    player_position,
                    direction,
                    PlayerGameData.Instance.CurrentWeapon.ShotSpeed,
                    PlayerGameData.Instance.CurrentWeapon.BulletSize,
                    PlayerGameData.Instance.CurrentWeapon.Pierce,
                    PlayerGameData.Instance.CritChance,
                    this,
                    PlayerGameData.Instance.CurrentWeapon.BulletDamage,
                    isChild));
        }

        /// <summary>
        /// Handles the reloading process based on elapsed game time.
        /// </summary>
        /// <param name="gametime">The amount of game time that has passed since the last frame.</param>
        private void Reload(float gametime)
        {
            if (PlayerGameData.Instance.CurrentWeapon.IsReloading)
            {
                PlayerGameData.Instance.CurrentWeapon.ReloadTimer += gametime;
                if (PlayerGameData.Instance.CurrentWeapon.ReloadTimer >= PlayerGameData.Instance.CurrentWeapon.ReloadTime)
                {
                    PlayerGameData.Instance.CurrentWeapon.CurrentAmmo = PlayerGameData.Instance.CurrentWeapon.MagazineSize;
                    PlayerGameData.Instance.CurrentWeapon.IsReloading = false;
                    PlayerGameData.Instance.CurrentWeapon.ReloadTimer = 0f;
                }
            }
        }
    }
}
