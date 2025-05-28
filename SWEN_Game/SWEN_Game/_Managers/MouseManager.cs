using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpFont;
using SWEN_Game._Entities;
using SWEN_Game._Shooting;
using SWEN_Game._Utils;

namespace SWEN_Game._Managers
{
    public static class MouseManager
    {
        /// <summary>
        /// Updates the Mouse Position each frame.
        /// </summary>
        /// <param name="player">Player reference.</param>
        /// <param name="playerWeapon">PlayerWeapon reference.</param>
        /// <param name="mouseState">Current MouseState.</param>
        public static void UpdateMouse(Player player, PlayerWeapon playerWeapon, MouseState mouseState)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 mouseScreenPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                // Calculate the translation matrix directly (centered camera)
                Vector2 screenCenter = new Vector2(Globals.WindowSize.X / 2f, Globals.WindowSize.Y / 2f);
                Matrix cameraTransform = Matrix.CreateTranslation(new Vector3(-player.Position + screenCenter, 0));

                // Invert the camera transform to go from screen-space to world-space
                Matrix inverseTransform = Matrix.Invert(cameraTransform);

                // Convert mouse position from screen to world space
                Vector2 mouseWorldPos = Vector2.Transform(mouseScreenPos, inverseTransform);

                // Calculate and normalize the shooting direction
                Vector2 direction = mouseWorldPos - player.Position;
                direction.Normalize();

                // Shoot
                playerWeapon.Shoot(direction, player.Position);
            }
        }
    }
}
