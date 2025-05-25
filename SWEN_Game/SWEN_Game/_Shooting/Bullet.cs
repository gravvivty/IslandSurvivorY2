using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class Bullet
    {
        private HashSet<IEnemy> _hitEnemies = new();
        public bool HasProcessedThisFrame { get; set; } = false;
        public Rectangle BulletHitbox { get; set; }
        public float Damage { get; set; }
        public PlayerWeapon Weapon { get; set; }
        public bool IsVisible { get; set; } = true;
        public float Timer { get; set; }
        public bool IsDemonBullet { get; set; }
        public int PiercingCount { get; set; } = 0;
        public Vector2 Position { get; private set; }
        private Vector2 _shotSpeed;
        private float _bulletSize;
        private float _visibilityTime = 1f;
        private Animation _animation;

        public Bullet(Animation animation, Vector2 startposition, Vector2 direction, float shotSpeed, float bulletSize, int piercingCount, PlayerWeapon weapon, float dmg, bool? isChild = null)
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
                Damage = dmg / 2;
            }
            else
            {
                Damage = dmg;
            }
        }

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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                _animation.Draw(Position);
                System.Diagnostics.Debug.WriteLine("Trying to draw the Bullet" + DateTime.Now);
            }
        }
        public bool HasHit(IEnemy enemy)
        {
            return _hitEnemies.Contains(enemy);
        }

        public void RegisterHit(IEnemy enemy)
        {
            _hitEnemies.Add(enemy);
        }
    }
}
