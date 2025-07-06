using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SWEN_Game._Entities;
using SWEN_Game._Items;
using SWEN_Game._Sound;
using SWEN_Game._Utils;

namespace SWEN_Game._Managers
{
    public static class InputManager
    {
        public static float Speed { get; set; } = 130f;

        /// <summary>
        /// Called every frame to check Key Input.
        /// </summary>
        /// <param name="player">Player class reference.</param>
        /// <param name="keyboardState">KeyboardState reference.</param>
        public static void Update(Player player, KeyboardState keyboardState)
        {
            Speed = PlayerGameData.Instance.PlayerSpeed;

            Vector2 moveDirection = Vector2.Zero;

            // How long was the button held
            float delta = Globals.Time;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                moveDirection.Y = -1;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                moveDirection.Y = 1;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                moveDirection.X = -1;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                moveDirection.X = 1;
            }

            if (keyboardState.IsKeyDown(Keys.R) && !PlayerGameData.Instance.CurrentWeapon.IsReloading)
            {
                PlayerGameData.Instance.CurrentWeapon.IsReloading = true;
                PlayerGameData.Instance.CurrentWeapon.ReloadTimer = 0f;
            }

            // Normalize Vector
            if (moveDirection != Vector2.Zero)
            {
                moveDirection.Normalize();
            }

            // X - Move Player if not colliding otherwise do not update Pos
            CheckXMovement(player, moveDirection, delta);

            // Same thing but for Y
            CheckYMovement(player, moveDirection, delta);

            // Normalize for Animation Use - how I got rid of having sqrt(2) for diagonals
            moveDirection = new Vector2(Math.Sign(moveDirection.X), Math.Sign(moveDirection.Y));
            player.SetDirection(moveDirection);
        }

        /// <summary>
        /// Checks movement in Y direction.
        /// </summary>
        /// <param name="player">Player reference.</param>
        /// <param name="moveDirection">Cardinal or intercardinal direction.</param>
        /// <param name="delta">DeltaTime between frames.</param>
        private static void CheckYMovement(Player player, Vector2 moveDirection, float delta)
        {
            Vector2 tentativePosition = player.Position;
            tentativePosition.Y += moveDirection.Y * Speed * delta;

            Rectangle yCollision = new Rectangle((int)tentativePosition.X + 4, (int)tentativePosition.Y + 6, 8, 8);

            if (!Globals.IsColliding(yCollision))
            {
                player.SetPosition(tentativePosition);
            }
        }

        /// <summary>
        /// Checks movement in X direction.
        /// </summary>
        /// <param name="player">Player reference.</param>
        /// <param name="moveDirection">Cardinal or intercardinal direction.</param>
        /// <param name="delta">DeltaTime between frames.</param>
        private static void CheckXMovement(Player player, Vector2 moveDirection, float delta)
        {
            Vector2 tentativePosition = player.Position;
            tentativePosition.X += moveDirection.X * Speed * delta;

            Rectangle xCollision = new Rectangle((int)tentativePosition.X + 4, (int)tentativePosition.Y + 6, 8, 8);

            if (!Globals.IsColliding(xCollision))
            {
                player.SetPosition(tentativePosition);
            }
        }
    }
}
