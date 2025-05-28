using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game._Anims
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

        /// <summary>
        /// Add an Animation to the Collection of your entity.
        /// </summary>
        /// <param name="key">Object under which your Animation should be known.</param>
        /// <param name="animation">Animation itself.</param>
        public void AddAnimation(object key, Animation animation)
        {
            _animations.Add(key, animation);
            if (_currentKey == null)
            {
                _currentKey = key;
            }
        }

        /// <summary>
        /// Starts and Updates the Animation if the Key exists - stops and resets if it doesn't.
        /// </summary>
        /// <param name="key">Object under which the Animation is known for lookup.</param>
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

        /// <summary>
        /// Draws the Animation depending on the key.
        /// </summary>
        /// <param name="position">Where the Sprite should be drawn.</param>
        /// <param name="tintColor">Optional: Color for the Sprite.</param>
        public void Draw(Vector2 position, Color? tintColor = null)
        {
            _animations[_currentKey].Draw(position, tintColor);
        }
    }
}