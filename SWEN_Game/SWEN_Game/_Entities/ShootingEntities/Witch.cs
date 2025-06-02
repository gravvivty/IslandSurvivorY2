using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Anims;
using SWEN_Game._Shooting;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities.Enemies
{
    public class Witch : Enemy
    {
        private float shootCooldown = 2f;  // seconds
        private float shootTimer = 0f;
        private List<Bullet> enemyBullets = new();

        public Witch(Vector2 spawnPosition)
        {
            Position = spawnPosition;
            EnemySpeed = 70f;
            CurrentHealth = 100f;
            EnemyDamage = 1;
            FrameWidth = 24;
            FrameHeight = 24;

            Texture = Globals.Content.Load<Texture2D>("Sprites/Entities/Enemies/Witch");

            Animation walkLeft = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 1);
            Animation walkRight = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 2);

            this.AnimationManager = new AnimationManager();
            this.AnimationManager.AddAnimation("WalkLeft", walkLeft);
            this.AnimationManager.AddAnimation("WalkRight", walkRight);
        }

        public override void UpdateCustomBehavior(EnemyManager enemyManager)
        {
            shootTimer += Globals.Time;

            if (shootTimer >= shootCooldown)
            {
                ShootAtPlayer(enemyManager);
                shootTimer = 0f;
            }

            // Update enemy bullets
            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                enemyBullets[i].Update();
                if (!enemyBullets[i].IsVisible)
                {
                    enemyBullets.RemoveAt(i);
                }
            }
        }

        public override void Draw()
        {
            base.Draw();

            // Draw enemy bullets
            foreach (var bullet in enemyBullets)
            {
                bullet.Draw(Globals.SpriteBatch);
            }
        }

        public List<Bullet> GetBullets()
        {
            return enemyBullets;
        }

        private void ShootAtPlayer(EnemyManager enemyManager)
        {
            Vector2 direction = Vector2.Normalize(enemyManager._player.RealPos - Position);

            Animation anim = new Animation(
                Globals.Content.Load<Texture2D>("Sprites/Bullets/CannonBullet"),
                1,
                4,
                0.1f,
                16,
                16,
                1,
                Color.Red,
                1f);

            Bullet bullet = new Bullet(anim, Position, direction, 200f, 1f, 0, 0f, null, 1f);
            enemyBullets.Add(bullet);
        }
    }
}