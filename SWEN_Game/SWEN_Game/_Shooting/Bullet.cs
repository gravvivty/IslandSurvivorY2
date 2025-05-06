using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class Bullet
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private float _speed = 500f;
        private bool _isVisible = true;
        private Texture2D _texture;

        public Bullet(Texture2D texture, Vector2 startposition, Vector2 direction)
        {
            this._texture = texture;
            _position = startposition;
            _velocity = Vector2.Normalize(direction) * _speed;
        }

        public void Update()
        {
            _position += _velocity * (float)Globals.Time;
            System.Diagnostics.Debug.WriteLine("Trying to update Bullet location" + DateTime.Now);

            if (_position.X < 0 || _position.X > Globals.Graphics.PreferredBackBufferWidth ||
               _position.Y < 0 || _position.Y > Globals.Graphics.PreferredBackBufferHeight)
            {
                _isVisible = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // normally isVisible not just true
            if (IsVisible)
            {
                Globals.SpriteBatch.Draw(_texture, _position, new Rectangle(0, 0, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1);
                System.Diagnostics.Debug.WriteLine("Trying to draw the Bullet" + DateTime.Now);
            }
        }

        public bool IsVisible => _isVisible;
        public Vector2 Position => _position;
    }
}
