using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game._Anims;
using SWEN_Game._Managers;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities
{
    public class Player
    {
        private SpriteManager _spriteManager;
        private AnimationManager _anims = new();

        public Texture2D PlayerTexture { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 RealPos { get; private set; }
        public Vector2 HitboxPos { get; private set; }
        public Rectangle Hitbox { get; private set; }
        public Vector2 Direction { get; private set; }

        public float Speed { get; private set;}

        // Invincibility Handling Stuff
        private bool _isInvincible = false;
        private float _invincibilityTime = 1.0f; // In seconds
        private float _invincibilityTimer = 0f;

        private float _flickerRate = 0.075f; // In seconds
        private float _flickerTimer = 0f;
        private bool _isVisible = true;

        public Player()
        {
            // SpritePos and Spawn Position
            Position = new Vector2(750, 750);

            // RealPos - used for actually comparing positions
            RealPos = new Vector2(Position.X + 4, Position.Y + 8);

            // HitboxPos of the Player
            HitboxPos = new Vector2(Position.X + 4, Position.Y + 8);
        }

        /// <summary>
        /// Add SpriteManager to Player and register all input direction keys with Animations.
        /// </summary>
        /// <param name="spriteManager">SpriteManager reference.</param>
        public void AddSpriteManager(SpriteManager spriteManager)
        {
            PlayerTexture = Globals.Content.Load<Texture2D>("player");
            _spriteManager = spriteManager;

            _anims.AddAnimation(new Vector2(0, -1), new(PlayerTexture, 1, 3, 0.1f, 16, 1)); // Up
            _anims.AddAnimation(new Vector2(1, -1), new(PlayerTexture, 1, 3, 0.1f, 16, 2)); // UpRight
            _anims.AddAnimation(new Vector2(1, 0), new(PlayerTexture, 1, 3, 0.1f, 16, 3)); // Right
            _anims.AddAnimation(new Vector2(1, 1), new(PlayerTexture, 1, 3, 0.1f, 16, 4)); // DownRight
            _anims.AddAnimation(new Vector2(0, 1), new(PlayerTexture, 1, 3, 0.1f, 16, 5)); // Down
            _anims.AddAnimation(new Vector2(-1, 1), new(PlayerTexture, 1, 3, 0.1f, 16, 6)); // DownLeft
            _anims.AddAnimation(new Vector2(-1, 0), new(PlayerTexture, 1, 3, 0.1f, 16, 7)); // Left
            _anims.AddAnimation(new Vector2(-1, -1), new(PlayerTexture, 1, 3, 0.1f, 16, 8)); // UpLeft
        }

        /// <summary>
        /// Sets the position of the Player and his relative positions like Hitbox/RealPosition. Updates Hitbox as well.
        /// </summary>
        /// <param name="newPos">New Position of the Player.</param>
        public void SetPosition(Vector2 newPos)
        {
            Position = newPos;
            RealPos = new Vector2(newPos.X + 4, newPos.Y + 8);
            HitboxPos = new Vector2(newPos.X + 4, newPos.Y + 4);
            Hitbox = new Rectangle((int)HitboxPos.X, (int)HitboxPos.Y, 8, 12);
        }

        /// <summary>
        /// Update Animation depending on movment input and handles invincibility when getting hit.
        /// </summary>
        public void Update()
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
        public void SetDirection(Vector2 direction)
        {
            this.Direction = direction;
        }

        /// <summary>
        /// Calls the Draw() in Animation for the Player if he is supposed to be visible.
        /// </summary>
        /// <remarks>
        /// Player will toggle visible and invisible fast when he gets hit by something.
        /// </remarks>
        public void Draw()
        {
            if (_isVisible)
            {
                _anims.Draw(Position);
            }
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
                PlayerGameData.CurrentHealth -= amount;
                TriggerInvincibility();

                if (PlayerGameData.CurrentHealth <= 0)
                {
                    // Handle player death here
                    PlayerGameData.CurrentHealth = 0;
                    System.Diagnostics.Debug.WriteLine("Player died");
                }
            }
        }
    }
}