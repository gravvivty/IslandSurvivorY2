using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game._Entities
{
    public class DamageNumber
    {
        public string Text { get; private set; }
        public Vector2 Position;
        public float Timer;
        private float _duration = 1f; // 1 second duration
        private Vector2 _velocity;
        private Color _color;
        private SpriteFont _font;

        public bool IsExpired => Timer >= _duration;

        public DamageNumber(string text, Vector2 position, SpriteFont font, Color color)
        {
            Text = text;
            Position = position;
            Timer = 0f;
            _font = font;
            _color = color;

            // Random velocity, e.g., slightly upwards with some horizontal spread
            var rand = new Random();
            float vx = (float)(rand.NextDouble() * 40 - 20); // -20 to 20 px/sec horizontally
            float vy = -40f; // move up at 40 px/sec
            _velocity = new Vector2(vx, vy);
        }

        public void Update(float gameTime)
        {
            Timer += gameTime;
            Position += _velocity * gameTime;

            // Fade out effect near end
            if (Timer > _duration * 0.7f)
            {
                float fadeAmount = 1f - (Timer - _duration * 0.7f) / (_duration * 0.3f);
                _color *= fadeAmount;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float scale = 0.5f;
            spriteBatch.DrawString(_font, Text, Position, _color, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
        }
    }
}
