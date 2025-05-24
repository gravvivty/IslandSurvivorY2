using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class Player
    {
        private SpriteManager _spriteManager;
        private AnimationManager _anims = new();

        // For general enemies/entities we can copy most of these variables
        public Texture2D PlayerTexture { get; private set; }

        // Used for drawing the Sprite
        public Vector2 Position { get; private set; }

        // Used to check if something is near the player
        public Vector2 RealPos { get; private set; }

        // Used to check if something entered the player's hitbox
        public Vector2 HitboxPos { get; private set; }
        public Rectangle Hitbox { get; private set; }

        public float Speed { get; private set; }
        public Vector2 Direction { get; private set; }

        // Invincibility Handling Stuff
        private bool _isInvincible = false;
        private float _invincibilityTime = 1.0f; // seconds
        private float _invincibilityTimer = 0f;

        private float _flickerRate = 0.075f; // seconds
        private float _flickerTimer = 0f;
        private bool _isVisible = true;

        public Player()
        {
            Speed = PlayerGameData.Speed;

            // SpritePos and Spawn Position
            Position = new Vector2(750, 750);

            // RealPos - used for actually comparing positions
            RealPos = new Vector2(Position.X + 4, Position.Y + 8);

            // HitboxPos of the Player
            HitboxPos = new Vector2(Position.X + 4, Position.Y + 8);
        }

        public void AddSpriteManager(SpriteManager spriteManager)
        {
            PlayerTexture = Globals.Content.Load<Texture2D>("player");
            _spriteManager = spriteManager;

            _anims.AddAnimation(new Vector2(0, -1), new(PlayerTexture, 1, 3, 0.1f, 1)); // Up
            _anims.AddAnimation(new Vector2(1, -1), new(PlayerTexture, 1, 3, 0.1f, 2)); // UpRight
            _anims.AddAnimation(new Vector2(1, 0), new(PlayerTexture, 1, 3, 0.1f, 3)); // Right
            _anims.AddAnimation(new Vector2(1, 1), new(PlayerTexture, 1, 3, 0.1f, 4)); // DownRight
            _anims.AddAnimation(new Vector2(0, 1), new(PlayerTexture, 1, 3, 0.1f, 5)); // Down
            _anims.AddAnimation(new Vector2(-1, 1), new(PlayerTexture, 1, 3, 0.1f, 6)); // DownLeft
            _anims.AddAnimation(new Vector2(-1, 0), new(PlayerTexture, 1, 3, 0.1f, 7)); // Left
            _anims.AddAnimation(new Vector2(-1, -1), new(PlayerTexture, 1, 3, 0.1f, 8)); // UpLeft
        }

        public void SetPosition(Vector2 newPos)
        {
            Position = newPos;
            RealPos = new Vector2(newPos.X + 4, newPos.Y + 8);
            HitboxPos = new Vector2(newPos.X + 4, newPos.Y + 4);
            Hitbox = new Rectangle((int)HitboxPos.X, (int)HitboxPos.Y, 8, 12);
        }

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

        public void SetDirection(Vector2 direction)
        {
            this.Direction = direction;
        }

        public void Draw()
        {
            if (_isVisible)
            {
                _anims.Draw(Position);
            }
        }

        // Use this to trigger invincibilty Flicker/Frames
        public void TriggerInvincibility()
        {
            _isInvincible = true;
            _invincibilityTimer = _invincibilityTime;
            _flickerTimer = _flickerRate;
            _isVisible = true;
        }

        public bool GetIsInvincible()
        {
            return _isInvincible;
        }
    }
}