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
    public class Shroom : IEnemy
    {
        public Vector2 Position { get; private set; }
        public Rectangle Hitbox { get; set; }
        public bool IsAlive { get; private set; } = true;
        public float CurrentHealth { get; set; } = 25f;
        public Texture2D Texture { get; set; } = Globals.Content.Load<Texture2D>("Sprites/Entities/Enemies/Dwende-Red");
        public int EnemyDamage { get; set; } = 1;
        public float EnemySpeed { get; set; } = 100f;
        public int FrameSize { get; set; } = 24;
        public AnimationManager AnimationManager { get; set; }
        public int DamageFlashTimer { get; set; }
        public int DamageFlashFrames { get; set; } = 5;

        public Shroom(Vector2 startPosition)
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

            // Decrement Flash Timer
            if (DamageFlashTimer > 0)
            {
                DamageFlashTimer--;
            }

            System.Diagnostics.Debug.WriteLine("Updated an enemy" + DateTime.Now);
        }

        public void Draw()
        {
            Color drawColor = Color.White;

            // Red Flash
            if (DamageFlashTimer > 0)
            {
                drawColor = Color.Red;
            }

            this.AnimationManager.Draw(Position, drawColor);
            System.Diagnostics.Debug.WriteLine("Drew an enemy" + DateTime.Now);
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            DamageFlashTimer = DamageFlashFrames;
            if (CurrentHealth <= 0)
            {
                IsAlive = false;
            }

            DamageFlashTimer = DamageFlashFrames;
        }

        public bool GotHitByBullet(List<Bullet> bulletList)
        {
            foreach (var bullet in bulletList)
            {
                if (!bullet.IsVisible || bullet.HasProcessedThisFrame || bullet.HasHit(this))
                {
                    continue;
                }

                if (Hitbox.Intersects(bullet.BulletHitbox))
                {
                    bullet.RegisterHit(this);
                    bullet.HasProcessedThisFrame = true;

                    foreach (var mod in bullet.Weapon.GetModifiers())
                    {
                        if (!bullet.IsDemonBullet && mod is DemonBulletsModifier demonMod)
                        {
                            demonMod.OnBulletCollision(bullet.Position, bullet.Weapon, true);
                        }
                    }

                    if (bullet.PiercingCount > 0)
                    {
                        bullet.PiercingCount--;
                    }
                    else
                    {
                        bullet.IsVisible = false;
                        bullet.Timer = 0f;
                    }

                    TakeDamage(bullet.Damage);
                    System.Diagnostics.Debug.WriteLine("Enemy got hit: " + DateTime.Now);
                    return true;
                }
            }

            return false;
        }
    }
}
