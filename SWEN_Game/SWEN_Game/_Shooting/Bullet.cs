using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Anims;
using SWEN_Game._Entities;
using SWEN_Game._Utils;

namespace SWEN_Game._Shooting
{
    /// <summary>
    /// Represents a bullet fired by the player, managing its position, animation,
    /// collision detection, piercing capability, and rendering.
    /// </summary>
    public class Bullet
    {
        private HashSet<Enemy> _hitEnemies = new();
        public bool HasProcessedThisFrame { get; set; } = false;
        public Rectangle BulletHitbox { get; set; }
        public float Damage { get; set; }
        public IPlayerWeapon Weapon { get; set; }
        public bool IsVisible { get; set; } = true;
        public float Timer { get; set; }
        public bool IsShadowBullet { get; set; }
        public int PiercingCount { get; set; } = 0;
        public Vector2 Position { get; private set; }
        public float CritChance { get; set; } = 0f;
        public float SlowChance { get; set; } = 0f;
        private Vector2 _shotSpeed;
        private float _bulletSize;
        private float _visibilityTime = 2f;
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
        /// <param name="critChance">The Crit Chance of the player.</param>
        /// <param name="slowChance">The Slow Chance of the player.</param>
        /// <param name="weapon">The weapon instance that created the bullet.</param>
        /// <param name="dmg">The damage this bullet deals.</param>
        /// <param name="isChild">Indicates if this bullet is a child (demon) bullet with modified damage.</param>
        public Bullet(Animation animation, Vector2 startposition, Vector2 direction, float shotSpeed, float bulletSize, int piercingCount, float critChance, float slowChance, IPlayerWeapon weapon, float dmg, bool? isChild = null)
        {
            _animation = animation;
            _animation.Reset();
            _animation.Start();
            Vector2 origin = new Vector2(_animation.frameWidth / 2f, _animation.frameHeight / 2f);
            Position = startposition - origin * (_animation._scale - 1f);
            _shotSpeed = Vector2.Normalize(direction) * shotSpeed;
            _bulletSize = bulletSize;
            PiercingCount = piercingCount;
            Weapon = weapon;
            CritChance = critChance;
            SlowChance = slowChance;
            IsShadowBullet = isChild ?? false;
            if (IsShadowBullet)
            {
                Damage = dmg / 5;
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
            BulletHitbox = new Rectangle((int)Position.X + 4, (int)Position.Y + 4, (int)(_bulletSize * 10), (int)(_bulletSize * 10));
            Timer += (float)Globals.Time;
            _animation.Update();

            if (Timer >= _visibilityTime)
            {
                IsVisible = false;
                Timer = 0f;
            }

            if (Globals.IsCollidingHitbox(BulletHitbox))
            {
                PiercingCount--;
                if (PiercingCount <= 0)
                {
                    IsVisible = false;
                    Timer = 0f;
                }
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
            }
        }

        /// <summary>
        /// Checks if the bullet has already hit the given enemy to avoid double damage.
        /// </summary>
        /// <param name="enemy">The enemy to check against the hit history.</param>
        /// <returns>True if the bullet has hit the enemy; otherwise, false.</returns>
        public bool HasHit(Enemy enemy)
        {
            return _hitEnemies.Contains(enemy);
        }

        /// <summary>
        /// Registers that this bullet has hit the specified enemy.
        /// </summary>
        /// <param name="enemy">The enemy to register as hit.</param>
        public void RegisterHit(Enemy enemy)
        {
            _hitEnemies.Add(enemy);
        }
    }
}
