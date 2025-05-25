using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Assimp.Unmanaged;
using SharpFont.Cache;

namespace SWEN_Game
{
    public class Mummy : InterfaceEnemy
    {
        public Vector2 Position { get; private set; }
        public Rectangle Hitbox { get; set; }
        public bool IsAlive { get; private set; } = true;
        public float CurrentHealth { get; set; } = 45f;
        public Texture2D Texture { get; set; } = Globals.Content.Load<Texture2D>("Sprites/Entities/Enemies/Amaranhig");
        public float EnemyDamage { get; set; } = 5f;
        public float EnemySpeed { get; set; } = 60f;

        public Mummy(Vector2 startPosition)
        {
            Position = startPosition;
        }

        public void Update(List<Bullet> bulletList, Vector2 playerPostion)
        {
            Vector2 direction = playerPostion - Position;
            Position += Vector2.Normalize(direction) * EnemySpeed * (float)Globals.Time;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)(Position.X + 24), (int)(Position.Y + 24));
            if (GotHitByBullet(bulletList) && CurrentHealth <= 0)
            {
                IsAlive = false;
                System.Diagnostics.Debug.WriteLine("Enemy got killed: " + DateTime.Now);
            }

            System.Diagnostics.Debug.WriteLine("Updated an enemy" + DateTime.Now);
        }

        public void Draw()
        {
            float depth = Globals.SpriteManager.GetDepth(Position, 24);
            Globals.SpriteBatch.Draw(
                Texture,
                Position,
                new Rectangle(0, 0, 24, 24),
                Color.White,
                0,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                depth);
            System.Diagnostics.Debug.WriteLine("Drew an enemy" + DateTime.Now);
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth <= 0)
            {
                IsAlive = false;
            }
        }

        public bool GotHitByBullet(List<Bullet> bulletList)
        {
            foreach (var bullet in bulletList)
            {
                if (Hitbox.Intersects(bullet.bullet))
                {
                    TakeDamage(bullet._damage);
                    System.Diagnostics.Debug.WriteLine("Enemy got hit: " + DateTime.Now);
                    bullet._isVisible = false;
                    return true;
                }
            }

            return false;
        }
    }
}
