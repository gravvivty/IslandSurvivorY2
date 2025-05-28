using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game._Utils;
using SWEN_Game._Anims;

namespace SWEN_Game._Shooting
{
    /// <summary>
    /// Represents a bullet fired by the player, managing its position, animation,
    /// collision detection, piercing capability, and rendering.
    /// </summary>
    public class Bullet
    {
        private HashSet<IEnemy> _hitEnemies = new();
        public bool HasProcessedThisFrame { get; set; } = false;
        public Rectangle BulletHitbox { get; set; }
        public float Damage { get; set; }
        public IPlayerWeapon Weapon { get; set; }
        public bool IsVisible { get; set; } = true;
        public float Timer { get; set; }
        public bool IsDemonBullet { get; set; }
        public int PiercingCount { get; set; } = 0;
        public Vector2 Position { get; private set; }
        private Vector2 _shotSpeed;
        private float _bulletSize;
        private float _visibilityTime = 1f;
        private Animation _animation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bullet"/> class with provided parameters.
        /// </summary>
        /// <param name="animation">The animation object used to render the bullet.</param>
        /// <param name="startposition">The initial position of the bullet.</param>
        /// <param name="direction">The normalized direction in which the bullet travels.</param>
        /// <param name="shotSpeed">The speed at which the bullet travels.</param>
        /// <param name="bulletSize">The size of the bullet for collision purposes.</param>
        /// <param name="piercingCount">How many enemies the bullet can pierce through.</param>
        /// <param name="weapon">The weapon instance that created the bullet.</param>
        /// <param name="dmg">The damage this bullet deals.</param>
        /// <param name="isChild">Indicates if this bullet is a child (demon) bullet with modified damage.</param>
        public Bullet(Animation animation, Vector2 startposition, Vector2 direction, float shotSpeed, float bulletSize, int piercingCount, IPlayerWeapon weapon, float dmg, bool? isChild = null)
        {
            _animation = animation;
            _animation.Reset();
            _animation.Start();
            Vector2 origin = new Vector2(_animation.frameSize / 2f, _animation.frameSize / 2f);
            Position = startposition - origin * (_animation._scale - 1f);
            _shotSpeed = Vector2.Normalize(direction) * shotSpeed;
            _bulletSize = bulletSize;
            PiercingCount = piercingCount;
            Weapon = weapon;
            IsDemonBullet = isChild ?? false;
            if (IsDemonBullet)
            {
                Damage = dmg / 3;
            }
            else
            {
                Damage = dmg;
            }
        }

        /// <summary>
        /// Updates the bullet's position, animation, visibility, and collision status.
        /// </summary>
        public void Update()
        {
            Position += _shotSpeed * (float)Globals.Time;
            BulletHitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)(_bulletSize + 4f), (int)(_bulletSize + 4f));
            Timer += (float)Globals.Time;
            _animation.Update();
            System.Diagnostics.Debug.WriteLine("Trying to update Bullet location" + DateTime.Now);

            if (Timer >= _visibilityTime)
            {
                IsVisible = false;
                Timer = 0f;
            }

            if (Globals.IsCollidingHitbox(BulletHitbox))
            {
                IsVisible = false;
                Timer = 0f;
            }
        }

        /// <summary>
        /// Draws the bullet to the screen using its animation if it is visible.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render the bullet.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                _animation.Draw(Position);
                System.Diagnostics.Debug.WriteLine("Trying to draw the Bullet" + DateTime.Now);
            }
        }

        /// <summary>
        /// Checks if the bullet has already hit the given enemy to avoid double damage.
        /// </summary>
        /// <param name="enemy">The enemy to check against the hit history.</param>
        /// <returns>True if the bullet has hit the enemy; otherwise, false.</returns>
        public bool HasHit(IEnemy enemy)
        {
            return _hitEnemies.Contains(enemy);
        }

        /// <summary>
        /// Registers that this bullet has hit the specified enemy.
        /// </summary>
        /// <param name="enemy">The enemy to register as hit.</param>
        public void RegisterHit(IEnemy enemy)
        {
            _hitEnemies.Add(enemy);
        }
    }
}
