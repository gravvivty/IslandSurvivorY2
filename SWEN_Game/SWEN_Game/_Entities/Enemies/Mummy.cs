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
    public class Mummy : IEnemy
    {
        public Vector2 Position { get; private set; }
        public Rectangle Hitbox { get; set; }
        public bool IsAlive { get; private set; } = true;
        public float CurrentHealth { get; set; } = 15f;
        public Texture2D Texture { get; set; } = Globals.Content.Load<Texture2D>("Sprites/Entities/Enemies/Amaranhig");
        public float EnemyDamage { get; set; } = 5f;
        public float EnemySpeed { get; set; } = 60f;
        public int FrameSize { get; set; } = 24;
        public AnimationManager AnimationManager { get; set; }

        public Mummy(Vector2 startPosition)
        {
            Position = startPosition;

            Animation walkLeft = new Animation(Texture, 1, 3, 0.2f, FrameSize, 1);
            Animation walkRight = new Animation(Texture, 1, 3, 0.2f, FrameSize, 2);

            this.AnimationManager = new AnimationManager();
            this.AnimationManager.AddAnimation("WalkLeft", walkLeft);
            this.AnimationManager.AddAnimation("WalkRight", walkRight);
        }

        public void Update(List<Bullet> bulletList, Vector2 playerPostion)
        {
            Vector2 direction = playerPostion - Position;

            if (direction.X >= 0)
            {
                this.AnimationManager.Update("WalkRight");
            }
            else
            {
                this.AnimationManager.Update("WalkLeft");
            }

            Position += Vector2.Normalize(direction) * EnemySpeed * (float)Globals.Time;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, FrameSize, FrameSize);
            if (GotHitByBullet(bulletList) && CurrentHealth <= 0)
            {
                IsAlive = false;
                System.Diagnostics.Debug.WriteLine("Enemy got killed: " + DateTime.Now);
            }

            System.Diagnostics.Debug.WriteLine("Updated an enemy" + DateTime.Now);
        }

        public void Draw()
        {
            this.AnimationManager.Draw(Position);
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
