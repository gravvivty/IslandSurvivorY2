using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public class Animation
    {
        public float _scale;
        public int frameSize = 16;

        private readonly Texture2D _texture;

        // Used to determine WHERE in the Spritesheet to get the Frames from
        private readonly List<Rectangle> _srcRect = new();
        private readonly int _totalFrames;
        private readonly float _frameTime;
        private int _currentFrame;
        private float _frameTimeLeft;
        private bool isActive = true;

        private Color _tintColor;

        public Animation(Texture2D texture, int framesX, int framesY, float frameTime, int frameSize, int column = 1, Color? tintColor = null, float? scale = null)
        {
            // Spritesheet
            _texture = texture;

            // How long frame should last
            _frameTime = frameTime;

            // How much time a frame has left before changing
            _frameTimeLeft = _frameTime;

            // Total Frames for anim
            _totalFrames = framesY;
            int frameWidth = frameSize;
            int frameHeight = frameWidth;

            // Add the sprite frames from the sheet
            for (int i = 0; i < _totalFrames; i++)
            {
                _srcRect.Add(new Rectangle((column - 1) * frameWidth, i * frameHeight, frameWidth, frameHeight));
            }

            _tintColor = tintColor ?? Color.White;
            _scale = scale ?? 1f;
        }

        /// <summary>
        /// Starts the Animation.
        /// </summary>
        public void Start()
        {
            isActive = true;
        }

        /// <summary>
        /// Stops the Animation.
        /// </summary>
        public void Stop()
        {
            isActive = false;
        }

        /// <summary>
        /// Resets the current Animation to first Frame.
        /// </summary>
        public void Reset()
        {
            _currentFrame = 0;
            _frameTimeLeft = _frameTime;
        }

        /// <summary>
        /// Handles the Frame Timer and cycles through all Frames of an Animation.
        /// </summary>
        public void Update()
        {
            if (!isActive)
            {
                return;
            }

            // Subtract time from the current frame
            _frameTimeLeft -= Globals.Time;

            // If no time --> next frame + add time
            if (_frameTimeLeft <= 0)
            {
                _frameTimeLeft += _frameTime;
                _currentFrame = (_currentFrame + 1) % _totalFrames;
            }
        }

        /// <summary>
        /// Draws a single Frame of an Animation with depth calculation.
        /// </summary>
        /// <param name="position">Where the Sprite should be drawn.</param>
        /// <param name="tintColor">Optional: Color for the Sprite.</param>
        public void Draw(Vector2 position, Color? tintColor = null)
        {
            Color color = tintColor ?? _tintColor;
            float depth = Globals.SpriteManager.GetDepth(position, frameSize);
            Globals.SpriteBatch.Draw(
                _texture,
                position,
                _srcRect[_currentFrame],
                color,
                0,
                Vector2.Zero,
                _scale,
                SpriteEffects.None,
                depth);
        }
    }
}