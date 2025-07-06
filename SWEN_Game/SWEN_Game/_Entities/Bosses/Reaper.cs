using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Anims;
using SWEN_Game._Shooting;
using SWEN_Game._Sound;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities.Enemies
{
    public class Reaper : Enemy, IBulletShooter
    {
        private int burstShotsFired = 0;
        private int maxBurstCount = 5;

        private float burstCooldown = 0.1f; // Time between each shot in a burst
        private float burstTimer = 0f;

        private float burstPauseDuration = 2f; // Pause between bursts
        private float burstPauseTimer = 0f;

        private bool inBurstMode = true;

        private List<Bullet> enemyBullets = new();

        public Reaper(Vector2 startPosition)
        {
            Position = startPosition;
            XPReward = 7000;
            CurrentHealth = 15000f;
            EnemyDamage = 2;
            EnemySpeed = 60f;
            FrameWidth = 80;
            FrameHeight = 80;
            Texture = Globals.Content.Load<Texture2D>("Sprites/Entities/Bosses/reaper");

            Animation walkLeft = new Animation(Texture, 1, 8, 0.1f, FrameWidth, FrameHeight, 1);
            Animation walkRight = new Animation(Texture, 1, 8, 0.1f, FrameWidth, FrameHeight, 2);

            this.AnimationManager = new AnimationManager();
            this.AnimationManager.AddAnimation("WalkLeft", walkLeft);
            this.AnimationManager.AddAnimation("WalkRight", walkRight);
        }

        public override void UpdateCustomBehavior(IEnemyContext enemyManager)
        {
            float delta = Globals.Time;

            if (inBurstMode)
            {
                burstTimer += delta;

                if (burstTimer >= burstCooldown && burstShotsFired < maxBurstCount)
                {
                    ShootAtPlayer(enemyManager);
                    SFXManager.Instance.Play("enemyShoot");
                    burstShotsFired++;
                    burstTimer = 0f;
                }

                if (burstShotsFired >= maxBurstCount)
                {
                    inBurstMode = false;
                    burstPauseTimer = 0f;
                }
            }
            else
            {
                burstPauseTimer += delta;

                if (burstPauseTimer >= burstPauseDuration)
                {
                    inBurstMode = true;
                    burstShotsFired = 0;
                    burstTimer = 0f;
                }
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

        protected override void UpdateHitbox()
        {
            int biggerWidth = FrameWidth / 4;
            float biggerHeight = FrameHeight / 2.5f;

            Hitbox = new Rectangle(
                (int)(Position.X + FrameWidth / 2.5f),
                (int)(Position.Y + FrameHeight / 2.5f),
                biggerWidth,
                (int)biggerHeight);
        }

        private void ShootAtPlayer(IEnemyContext enemyManager)
        {
            Vector2 reaperCenter = Position + new Vector2(FrameWidth / 2f, FrameHeight / 2f);
            Vector2 direction = Vector2.Normalize(enemyManager.PlayerPos - reaperCenter);

            Animation anim = new Animation(
                Globals.Content.Load<Texture2D>("Sprites/Bullets/ReaperBullet"),
                1,
                4,
                0.1f,
                16,
                16,
                1,
                Color.Orange,
                1f);

            Bullet bullet = new Bullet(anim, reaperCenter, direction, 300f, 1.5f, 0, 0f, 0f, null, 1f);
            enemyBullets.Add(bullet);
        }
    }
}
