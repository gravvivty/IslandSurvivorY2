using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Anims;
using SWEN_Game._Items._Modifiers;
using SWEN_Game._Shooting;
using SWEN_Game._Sound;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities
{
    public abstract class Enemy
    {
        public float XPReward { get; protected set; }
        public Vector2 Position { get; protected set; }
        public Rectangle Hitbox { get; protected set; }
        public virtual bool IsAlive { get; protected set; } = true;
        public float CurrentHealth { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public int EnemyDamage { get; protected set; }
        public float EnemySpeed { get; protected set; }
        public int FrameWidth { get; protected set; }
        public int FrameHeight { get; protected set; }
        public AnimationManager AnimationManager { get; protected set; }
        public int DamageFlashFrames { get; protected set; } = 5;
        public int DamageFlashTimer { get; protected set; }
        private List<DamageNumber> _damageNumbers = new List<DamageNumber>();
        private SpriteFont _damageFont;
        private float _slowDuration;
        private float _slowTimer;
        private float _originalSpeed;
        private bool _isSlowed;

        public Enemy()
        {
            _damageFont = Globals.Content.Load<SpriteFont>("Menu/GameFont");
        }

        public virtual void UpdateCustomBehavior(IEnemyContext ienemyManager)
        {
        }

        public virtual void Update(List<Bullet> bulletList, Vector2 playerPosition, IEnemyContext ienemyManager)
        {
            UpdateSlow((float)Globals.Time);
            UpdateMovement(playerPosition);
            UpdateAnimation(playerPosition);
            UpdateHitbox();

            if (GotHitByBullet(bulletList) && CurrentHealth <= 0)
            {
                HandleDeath();
            }

            UpdateDamageFlashTimer();

            UpdateCustomBehavior(ienemyManager);

            for (int i = _damageNumbers.Count - 1; i >= 0; i--)
            {
                _damageNumbers[i].Update(Globals.Time);
                if (_damageNumbers[i].IsExpired)
                {
                    _damageNumbers.RemoveAt(i);
                }
            }
        }

        public virtual void Draw()
        {
            // Draw enemy
            Color drawColor = DamageFlashTimer > 0 ? Color.Red : (_isSlowed ? new Color(100, 170, 230) : Color.White);
            AnimationManager.Draw(Position, drawColor);

            // Draw damage numbers
            foreach (var dmgNum in _damageNumbers)
            {
                dmgNum.Draw(Globals.SpriteBatch);
            }
        }

        public virtual void TakeDamage(float amount, bool isCrit = false)
        {
            CurrentHealth -= amount;
            DamageFlashTimer = DamageFlashFrames;

            if (CurrentHealth <= 0)
            {
                IsAlive = false;
            }

            Random rand = new Random();
            float offsetX = (float)(rand.NextDouble() * FrameWidth - FrameWidth / 2);
            float offsetY = (float)(rand.NextDouble() * FrameHeight - FrameHeight / 2);

            Vector2 dmgPos = Position + new Vector2(offsetX, offsetY);

            Color dmgColor = isCrit ? Color.Red : Color.Black;

            if (isCrit)
            {
                SFXManager.Instance.Play("enemyHitCrit");
            }
            else
            {
                SFXManager.Instance.Play("enemyHit");
            }

            _damageNumbers.Add(new DamageNumber(((int)amount).ToString(), dmgPos, _damageFont, dmgColor));
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

        public void ApplySlow(float duration)
        {
            if (!_isSlowed)
            {
                _originalSpeed = EnemySpeed;
            }

            EnemySpeed = _originalSpeed * 0.5f; // Half speed
            _slowTimer = 0f;
            _slowDuration = duration;
            _isSlowed = true;
            SFXManager.Instance.Play("slow");
        }

        protected virtual void UpdateHitbox()
        {
            int biggerWidth = FrameWidth / 3;
            int biggerHeight = FrameHeight / 3;

            Hitbox = new Rectangle(
                (int)(Position.X + FrameWidth / 3f),
                (int)(Position.Y + FrameHeight / 1.5f),
                biggerWidth,
                biggerHeight);
        }

        private void UpdateSlow(float deltaTime)
        {
            if (_isSlowed)
            {
                _slowTimer += deltaTime;
                if (_slowTimer >= _slowDuration)
                {
                    EnemySpeed = _originalSpeed;
                    _isSlowed = false;
                }
            }
        }

        private void ProcessBulletHit(Bullet bullet)
        {
            bullet.RegisterHit(this);
            bullet.HasProcessedThisFrame = true;

            HandleBulletModifiers(bullet);
            HandleBulletPiercing(bullet);
            ApplyBulletDamage(bullet);
            ApplyBulletSlow(bullet);
        }

        private void ApplyBulletSlow(Bullet bullet)
        {
            if (!_isSlowed && bullet.SlowChance > 0f && new Random().NextDouble() < bullet.SlowChance)
            {
                ApplySlow(2f); // Slow for 2 seconds
            }
        }

        private void HandleBulletModifiers(Bullet bullet)
        {
            if (bullet.IsShadowBullet)
            {
                return;
            }

            foreach (var mod in bullet.Weapon.GetModifiers())
            {
                if (mod is ShadowBulletsModifier demonMod)
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
            bool isCrit = false;
            if (bullet.CritChance > 0f && new Random().NextDouble() <= bullet.CritChance)
            {
                damage *= 2f;
                isCrit = true;
                System.Diagnostics.Debug.WriteLine($"CRIT! Bullet dealt {damage} damage at position {Position}");
            }

            TakeDamage(damage, isCrit);
        }

        private void UpdateMovement(Vector2 playerPosition)
        {
            Vector2 enemyCenter = Position + new Vector2(FrameWidth / 2f, FrameHeight / 2f);
            Vector2 direction = playerPosition - enemyCenter;
            Position += Vector2.Normalize(direction) * EnemySpeed * (float)Globals.Time;
        }

        private void UpdateAnimation(Vector2 playerPosition)
        {
            Vector2 direction = playerPosition - Position;
            string animationKey = direction.X >= 0 ? "WalkRight" : "WalkLeft";
            AnimationManager.Update(animationKey);
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
            SFXManager.Instance.Play("enemyDeath");
            IsAlive = false;
        }
    }
}
