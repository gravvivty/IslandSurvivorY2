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
        public int FrameWidth { get; protected set; }
        public int FrameHeight { get; protected set; }
        public AnimationManager AnimationManager { get; protected set; }
        public int DamageFlashFrames { get; protected set; } = 5;
        public int DamageFlashTimer { get; protected set; }

        public virtual void UpdateCustomBehavior(EnemyManager enemyManager)
        {
        }

        public virtual void Update(List<Bullet> bulletList, Vector2 playerPosition, EnemyManager enemyManager)
        {
            UpdateMovement(playerPosition);
            UpdateAnimation(playerPosition);
            UpdateHitbox();

            if (GotHitByBullet(bulletList) && CurrentHealth <= 0)
            {
                HandleDeath();
            }

            UpdateDamageFlashTimer();

            UpdateCustomBehavior(enemyManager);
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
            var hits = bulletList
                .Where(bullet => !bullet.HasProcessedThisFrame && !bullet.HasHit(this))
                .Where(bullet => Hitbox.Intersects(bullet.BulletHitbox))
                .ToList();

            foreach (var bullet in hits)
            {
                ProcessBulletHit(bullet);
                return true; // only one hit matters
            }

            return false;
        }

        private void ProcessBulletHit(Bullet bullet)
        {
            bullet.RegisterHit(this);
            bullet.HasProcessedThisFrame = true;

            HandleBulletModifiers(bullet);
            HandleBulletPiercing(bullet);
            ApplyBulletDamage(bullet);
        }

        private void HandleBulletModifiers(Bullet bullet)
        {
            if (bullet.IsDemonBullet)
            {
                return;
            }

            foreach (var mod in bullet.Weapon.GetModifiers())
            {
                if (mod is DemonBulletsModifier demonMod)
                {
                    demonMod.OnBulletCollision(bullet.Position, bullet.Weapon, true);
                }
            }
        }

        private void HandleBulletPiercing(Bullet bullet)
        {
            if (bullet.PiercingCount > 0)
            {
                bullet.PiercingCount--;
            }
            else
            {
                bullet.IsVisible = false;
                bullet.Timer = 0f;
            }
        }

        private void ApplyBulletDamage(Bullet bullet)
        {
            float damage = bullet.Damage;
            if (bullet.CritChance > 0f && new Random().NextDouble() <= bullet.CritChance)
            {
                damage *= 2f;
                System.Diagnostics.Debug.WriteLine($"CRIT! Bullet dealt {damage} damage at position {Position}");
            }

            TakeDamage(damage);
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
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, FrameWidth, FrameHeight);
        }

        private void UpdateDamageFlashTimer()
        {
            if (DamageFlashTimer > 0)
            {
                DamageFlashTimer--;
            }
        }

        private void HandleDeath()
        {
            IsAlive = false;
        }
    }
}
