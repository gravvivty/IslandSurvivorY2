using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public class AnimationManager
    {
        // Key for this Map can be anything - i think
        private readonly Dictionary<object, Animation> _animations = new Dictionary<object, Animation>();
        private object _currentKey;
        public AnimationManager()
        {
            // dummy
        }

        public void AddAnimation(object key, Animation animation)
        {
            _animations.Add(key, animation);
            if (_currentKey == null)
            {
                _currentKey = key;
            }
        }

        public void Update(object key)
        {
            // If Key exists - start the animation and update it
            if (_animations.ContainsKey(key))
            {
                _animations[key].Start();
                _animations[key].Update();
                _currentKey = key;
            }
            else if (_currentKey != null && _animations.ContainsKey(_currentKey)) // Should never occur anyway but exception handling
            {
                _animations[_currentKey].Stop();
                _animations[_currentKey].Reset();
            }
        }

        public void Draw(Vector2 position, Color? tintColor = null)
        {
            _animations[_currentKey].Draw(position, tintColor);
        }
    }
}