using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Anims;
using SWEN_Game._Shooting;
using SWEN_Game._Shooting._Modifiers;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities
{
    public abstract class Enemy
    {
        public Vector2 Position { get; protected set; }
        public Rectangle Hitbox { get; protected set; }
        public bool IsAlive { get; protected set; } = true;
        public float CurrentHealth { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public int EnemyDamage { get; protected set; }
        public float EnemySpeed { get; protected set; }
        public int FrameSize { get; protected set; }
        public AnimationManager AnimationManager { get; protected set; }
        public int DamageFlashFrames { get; protected set; } = 5;
        public int DamageFlashTimer { get; protected set; }

        public virtual void Update(List<Bullet> bulletList, Vector2 playerPosition)
        {
            UpdateMovement(playerPosition);
            UpdateAnimation(playerPosition);
            UpdateHitbox();

            if (CheckBulletCollision(bulletList))
            {
                HandleDeath();
            }

            UpdateDamageFlashTimer();
        }

        public virtual void Draw()
        {
            Color drawColor = DamageFlashTimer > 0 ? Color.Red : Color.White;
            AnimationManager.Draw(Position, drawColor);
        }

        public virtual void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            DamageFlashTimer = DamageFlashFrames;

            if (CurrentHealth <= 0)
            {
                IsAlive = false;
            }
        }

        public virtual bool GotHitByBullet(List<Bullet> bulletList)
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

                    if (bullet.CritChance > 0f && new Random().NextDouble() <= bullet.CritChance)
                    {
                        TakeDamage(bullet.Damage * 2f); // Crit damage
                        System.Diagnostics.Debug.WriteLine($"CRIT! Bullet dealt {bullet.Damage * 2} damage at position {Position}");
                    }
                    else
                    {
                        TakeDamage(bullet.Damage);
                    }

                    return true;
                }
            }

            return false;
        }

        private void UpdateMovement(Vector2 playerPosition)
        {
            Vector2 direction = playerPosition - Position;
            Position += Vector2.Normalize(direction) * EnemySpeed * (float)Globals.Time;
        }

        private void UpdateAnimation(Vector2 playerPosition)
        {
            Vector2 direction = playerPosition - Position;
            string animationKey = direction.X >= 0 ? "WalkRight" : "WalkLeft";
            AnimationManager.Update(animationKey);
        }

        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, FrameSize, FrameSize);
        }

        private void UpdateDamageFlashTimer()
        {
            if (DamageFlashTimer > 0)
            {
                DamageFlashTimer--;
            }
        }

        private bool CheckBulletCollision(List<Bullet> bullets)
        {
            return GotHitByBullet(bullets) && CurrentHealth <= 0;
        }

        private void HandleDeath()
        {
            IsAlive = false;
        }
    }
}
