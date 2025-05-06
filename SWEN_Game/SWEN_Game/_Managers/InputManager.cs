using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpFont;

namespace SWEN_Game
{
    public static class InputManager
    {
        public static void Update(Player player, WeaponManager weaponManager, KeyboardState keyboardState, MouseState mouseState)
        {
            Vector2 moveDirection = Vector2.Zero;

            // How long was the button held
            float delta = Globals.Time;
            if (keyboardState.IsKeyDown(Keys.W)) { moveDirection.Y = -1; }
            if (keyboardState.IsKeyDown(Keys.S)) { moveDirection.Y = 1; }
            if (keyboardState.IsKeyDown(Keys.A)) { moveDirection.X = -1; }
            if (keyboardState.IsKeyDown(Keys.D)) { moveDirection.X = 1; }
            
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
                weaponManager.Shoot(direction, player.Position);
            }


            


            // Normalize Vector
            if (moveDirection != Vector2.Zero) { moveDirection.Normalize(); }

            // X - Move Player if not colliding otherwise do not update Pos
            Vector2 tentativePosition = player.Position;
            Vector2 tentativePositionReal = player.RealPos;
            tentativePosition.X += moveDirection.X * player.Speed * delta;
            tentativePositionReal.X += moveDirection.X * player.Speed * delta;
            if (!Globals.IsColliding(tentativePosition, player.Texture))
            {
                player.SetPosition(tentativePosition, tentativePositionReal);
            }

            // Same thing but for Y
            tentativePosition = player.Position;
            tentativePositionReal = player.RealPos;
            tentativePosition.Y += moveDirection.Y * player.Speed * delta;
            tentativePositionReal.Y += moveDirection.Y * player.Speed * delta;
            if (!Globals.IsColliding(tentativePosition, player.Texture))
            {
                player.SetPosition(tentativePosition, tentativePositionReal);
            }

            // Normalize for Animation Use
            moveDirection = new Vector2(Math.Sign(moveDirection.X), Math.Sign(moveDirection.Y));
            player.SetDirection(moveDirection);
        }
    }
}
