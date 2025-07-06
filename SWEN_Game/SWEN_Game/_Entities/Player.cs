using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game._Anims;
using SWEN_Game._Graphics;
using SWEN_Game._Interfaces;
using SWEN_Game._Managers;
using SWEN_Game._Sound;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities
{
    public class Player : IPlayer
    {
        private SpriteManager _spriteManager;
        private AnimationManager _anims = new();
        private IPlayerStats _playerStats;
        private IGameStateManager _gameStateManager;
        public virtual Texture2D PlayerTexture { get; private set; }
        public Vector2 Position { get; private set; }
        public virtual Vector2 RealPos { get; private set; }
        public virtual Vector2 HitboxPos { get; private set; }
        public virtual Rectangle Hitbox { get; private set; }
        public virtual Vector2 Direction { get; private set; }

        public float Speed { get; set; } = 130f;
        private Texture2D pixel;
        private Texture2D _weaponIcon;

        // Invincibility Handling Stuff
        private bool _isInvincible = false;
        private float _invincibilityTime = 1.0f; // In seconds
        private float _invincibilityTimer = 0f;

        private float _flickerRate = 0.075f; // In seconds
        private float _flickerTimer = 0f;
        private bool _isVisible = true;

        public Player(IPlayerStats playerStats, IGameStateManager gameStateManager)
        {
            _playerStats = playerStats;
            // SpritePos and Spawn Position
            Position = new Vector2(750, 750);

            // RealPos - used for actually comparing positions
            RealPos = new Vector2(Position.X + 4, Position.Y + 8);

            // HitboxPos of the Player
            HitboxPos = new Vector2(Position.X + 4, Position.Y + 8);
            _gameStateManager = gameStateManager;
        }

        /// <summary>
        /// Add SpriteManager to Player and register all input direction keys with Animations.
        /// </summary>
        /// <param name="spriteManager">SpriteManager reference.</param>
        public virtual void InitPlayer(SpriteManager spriteManager)
        {
            PlayerTexture = Globals.Content.Load<Texture2D>("player");
            _spriteManager = spriteManager;
            _weaponIcon = Globals.Content.Load<Texture2D>("Sprites/Guns/Guns_Ingame/pistol");
            pixel = Globals.Content.Load<Texture2D>("debug_rect"); // A white pixel texture

            _anims.AddAnimation(new Vector2(0, -1), new(PlayerTexture, 1, 3, 0.1f, 16, 16, 1)); // Up
            _anims.AddAnimation(new Vector2(1, -1), new(PlayerTexture, 1, 3, 0.1f, 16, 16, 2)); // UpRight
            _anims.AddAnimation(new Vector2(1, 0), new(PlayerTexture, 1, 3, 0.1f, 16, 16, 3)); // Right
            _anims.AddAnimation(new Vector2(1, 1), new(PlayerTexture, 1, 3, 0.1f, 16, 16, 4)); // DownRight
            _anims.AddAnimation(new Vector2(0, 1), new(PlayerTexture, 1, 3, 0.1f, 16, 16, 5)); // Down
            _anims.AddAnimation(new Vector2(-1, 1), new(PlayerTexture, 1, 3, 0.1f, 16, 16, 6)); // DownLeft
            _anims.AddAnimation(new Vector2(-1, 0), new(PlayerTexture, 1, 3, 0.1f, 16, 16, 7)); // Left
            _anims.AddAnimation(new Vector2(-1, -1), new(PlayerTexture, 1, 3, 0.1f, 16, 16, 8)); // UpLeft
        }

        /// <summary>
        /// Sets the position of the Player and his relative positions like Hitbox/RealPosition. Updates Hitbox as well.
        /// </summary>
        /// <param name="newPos">New Position of the Player.</param>
        public virtual void SetPosition(Vector2 newPos)
        {
            Position = newPos;
            RealPos = new Vector2(newPos.X + 4, newPos.Y + 8);
            HitboxPos = new Vector2(newPos.X + 4, newPos.Y + 4);
            Hitbox = new Rectangle((int)HitboxPos.X, (int)HitboxPos.Y, 7, 9);
        }

        /// <summary>
        /// Update Animation depending on movment input and handles invincibility when getting hit.
        /// </summary>
        public virtual void Update()
        {
            _anims.Update(this.Direction);

            // Handle invincibility timer
            if (_isInvincible)
            {
                _invincibilityTimer -= Globals.Time;
                _flickerTimer -= Globals.Time;

                if (_flickerTimer <= 0)
                {
                    _isVisible = !_isVisible; // Toggle flicker
                    _flickerTimer = _flickerRate; // Reset flicker timer
                }

                if (_invincibilityTimer <= 0)
                {
                    _isInvincible = false;
                    _isVisible = true;
                }
            }
        }

        /// <summary>
        /// Sets the new directional vector of the player.
        /// </summary>
        /// <param name="direction">Direction (8 possibilities).</param>
        public virtual void SetDirection(Vector2 direction)
        {
            this.Direction = direction;
        }

        /// <summary>
        /// Calls the Draw() in Animation for the Player if he is supposed to be visible.
        /// </summary>
        /// <remarks>
        /// Player will toggle visible and invisible fast when he gets hit by something.
        /// </remarks>
        public virtual void Draw()
        {
            if (_isVisible)
            {
                _anims.Draw(Position);
            }

            DrawReloadBar();

            DrawWeaponIcon();
        }

        /// <summary>
        /// Triggers Invincibility.
        /// </summary>
        public void TriggerInvincibility()
        {
            _isInvincible = true;
            _invincibilityTimer = _invincibilityTime;
            _flickerTimer = _flickerRate;
            _isVisible = true;
        }

        /// <summary>
        /// Get current invincibility State.
        /// </summary>
        /// <returns>_isInvincible.</returns>
        public bool GetIsInvincible()
        {
            return _isInvincible;
        }

        /// <summary>
        /// Reduces the player health by amount if he is not invincible.
        /// </summary>
        /// <param name="amount">How much Player HP is reduced.</param>
        public void TakeDamage(int amount)
        {
            if (!_isInvincible)
            {
                _playerStats.SetCurrentHealth(_playerStats.GetCurrentHealth() - amount);
                SFXManager.Instance.Play("playerHit");
                TriggerInvincibility();

                if (_playerStats.GetCurrentHealth() <= 0)
                {
                    // Handle player death here
                    _playerStats.SetCurrentHealth(0);
                    System.Diagnostics.Debug.WriteLine("Player died");
                    _gameStateManager.ChangeGameState(GameState.GameOver);
                }
            }
        }

        private void DrawReloadBar()
        {
            var weapon = _playerStats.GetWeapon();
            if (weapon.IsReloading)
            {
                float progress = weapon.ReloadTimer / weapon.ReloadTime;

                int barWidth = 16;
                int barHeight = 4;
                Vector2 barPos = new Vector2(Position.X, Position.Y - 5);

                Rectangle bg = new Rectangle((int)barPos.X, (int)barPos.Y, barWidth, barHeight);
                Rectangle fg = new Rectangle((int)barPos.X, (int)barPos.Y, (int)(barWidth * progress), barHeight);

                Globals.SpriteBatch.Draw(pixel, bg, null, Color.Gray, 0f, Vector2.Zero, SpriteEffects.None, 0.999f);
                Globals.SpriteBatch.Draw(pixel, fg, null, Color.Blue, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            }
        }

        private void DrawWeaponIcon()
        {
            _weaponIcon = _playerStats.GetBaseWeapon().IngameSprite;
            if (_weaponIcon == null)
            {
                return;
            }

            // Get mouse position in world coordinates (same as MouseManager)
            Vector2 mouseScreenPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            Vector2 screenCenter = new Vector2(Globals.WindowSize.X / 2f, Globals.WindowSize.Y / 2f);
            Matrix cameraTransform = Matrix.CreateTranslation(new Vector3(-Position + screenCenter, 0));
            Matrix inverseTransform = Matrix.Invert(cameraTransform);
            Vector2 mouseWorldPos = Vector2.Transform(mouseScreenPos, inverseTransform);

            // Direction and rotation
            Vector2 direction = mouseWorldPos - RealPos;
            float rotation = (float)Math.Atan2(direction.Y, direction.X) - MathHelper.PiOver4;

            // Draw position and centered origin
            Vector2 iconPosition = RealPos + new Vector2(3, 0); // position above the player
            Vector2 iconOrigin = new Vector2(_weaponIcon.Width / 2f, _weaponIcon.Height / 2f); // center of sprite

            float depth = Globals.SpriteManager.GetDepth(Position, 18);

            Globals.SpriteBatch.Draw(
                _weaponIcon,
                iconPosition,
                null,
                Color.White,
                rotation,
                iconOrigin,
                1f,
                SpriteEffects.None,
                depth);
        }
    }
}