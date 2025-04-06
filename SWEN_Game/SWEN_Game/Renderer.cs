using System;
using System.Collections.Generic;
using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SWEN_Game
{
    public class Renderer
    {
        private readonly float DepthRadius = 120f;
        private Player _player;
        private SpriteManager _spriteManager;
        private SpriteCalculator _spriteCalculator;

        public Renderer(Player player, SpriteManager spriteManager, SpriteCalculator spriteCalculator)
        {
            _player = player;
            _spriteManager = spriteManager;
            _spriteCalculator = spriteCalculator;
        }

        public Matrix CalcTranslation()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 screenCenter = new Vector2(
                Globals.Graphics.PreferredBackBufferWidth / 2f,
                Globals.Graphics.PreferredBackBufferHeight / 2f);

            // Raw mouse offset from the screen center -> cuz character is center of screen
            Vector2 rawMouseOffset = new Vector2(mouseState.X, mouseState.Y) - screenCenter;

            float maxMouseRange = Globals.WindowSize.X; // Mouse can affect camera within this range
            float maxCameraOffset = 30f; // Camera shifts within this range

            // Scales the Offset down - 0->maxMouseRange gets scaled to 0->maxCameraOffset
            // Ensures Camera smoothness
            Vector2 mouseOffset = rawMouseOffset * (maxCameraOffset / maxMouseRange);

            // Ensure the final offset never exceeds maxCameraOffset
            if (mouseOffset.Length() > maxCameraOffset)
            {
                mouseOffset.Normalize(); // Keep direction
                mouseOffset = mouseOffset * maxCameraOffset; // Clamp to maxCameraOffset
            }

            return Matrix.CreateTranslation(
                -_player.RealPos.X - mouseOffset.X,
                -_player.RealPos.Y - mouseOffset.Y,
                0) *
                Matrix.CreateScale(Globals.Zoom, Globals.Zoom, 1f) *
                Matrix.CreateTranslation(screenCenter.X, screenCenter.Y, 0);
        }

    }
}
