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

        private Vector2 _direction;

        public Player()
        {
            Speed = 130f;

            // Spawn Pos
            Position = new Vector2(750, 750);

            // Offset Pos - used for actually comparing positions
            RealPos = new Vector2(Position.X + 4, Position.Y + 8);
            HitboxPos = new Vector2(Position.X + 4, Position.Y + 8);

            // Hitbox of the Player
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
            _anims.Update(GetDirection());
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public Vector2 GetDirection()
        {
            return _direction;
        }

        public void Draw()
        {
            _anims.Draw(Position);
        }
    }
}