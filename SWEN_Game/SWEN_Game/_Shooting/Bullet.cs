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
        private Vector2 _shotSpeed;
        private float _bulletSize;
        private bool _isVisible = true;
        private float _timer = 0f;
        private float _visibilityTime = 1f;
        private int _piercingCount = 0;
        private Animation _animation;
        private Rectangle bullet;

        public Bullet(Animation animation, Vector2 startposition, Vector2 direction, float shotSpeed, float bulletSize, int piercingCount)
        {
            _animation = animation;
            _animation.Reset();
            _animation.Start();
            Vector2 origin = new Vector2(_animation.frameSize / 2f, _animation.frameSize / 2f);
            _position = startposition - origin * (_animation._scale - 1f);
            _shotSpeed = Vector2.Normalize(direction) * shotSpeed;
            _bulletSize = bulletSize;
            _piercingCount = piercingCount;
        }

        public void Update()
        {
            _position += _shotSpeed * (float)Globals.Time;
            bullet = new Rectangle((int)_position.X, (int)_position.Y, (int)(_bulletSize + 4f), (int)(_bulletSize + 4f));
            _timer += (float)Globals.Time;
            _animation.Update();
            System.Diagnostics.Debug.WriteLine("Trying to update Bullet location" + DateTime.Now);

            if (_timer >= _visibilityTime)
            {
                _isVisible = false;
                _timer = 0f;
            }

            if (Globals.IsCollidingHitbox(bullet))
            {
                if (_piercingCount > 0)
                {
                    _piercingCount--; // move this logic with pierce later to the enemies, they will know the bulletList
                }
                else
                {
                    _isVisible = false;
                    _timer = 0f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                _animation.Draw(_position);
                System.Diagnostics.Debug.WriteLine("Trying to draw the Bullet" + DateTime.Now);
            }
        }

        public bool IsVisible => _isVisible;
        public Vector2 Position => _position;
    }
}
