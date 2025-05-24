using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;

namespace SWEN_Game
{
    public class Debug
    {
        private readonly Player _player;
        private readonly Renderer _renderer;
        private readonly PowerupManager _powerupManager;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public Debug(Player player, Renderer renderer, PowerupManager powerupmanager)
        {
            _player = player;
            _renderer = renderer;
            _powerupManager = powerupmanager;
        }

        public void DebugUpdate()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            // DEBUG PLAYER INVINCIBILITY
            if (_currentKeyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space) && !_player.GetIsInvincible())
            {
                _player.TriggerInvincibility();
            }

            // DEBUG REVERSE SHOT
            if (_currentKeyboardState.IsKeyDown(Keys.R) && !_previousKeyboardState.IsKeyDown(Keys.R))
            {
                // _powerupManager.AddItem(1); // itemID 1 = GunpowderPowerup
                _powerupManager.AddItem(2); // itemID 2 = ReverseShotPowerup
                // _powerupManager.AddItem(3); // itemID 3 = PiercerPowerup
                PlayerGameData.UpdateWeaponGameData();
            }
        }

        public void DrawWorldDebug()
        {
            Globals.SpriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: _renderer.CalcTranslation(),
                samplerState: SamplerState.PointClamp);

            // Draw the player sprite using its calculated depth.
            // _spriteManager.DrawPlayer(Globals.SpriteBatch, _player.texture, _player.position);

            // Draw the player's collision box for debugging, using a pink overlay.
            Rectangle playerCollision = new Rectangle((int)_player.Position.X + 5, (int)_player.Position.Y + 10, 8, 8);
            Globals.SpriteBatch.Draw(
                Globals.Content.Load<Texture2D>("debug_rect"),
                playerCollision,
                null,
                Color.Pink,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.992f);

            // Draw any collision areas in red.
            foreach (var collision in Globals.Collisions)
            {
                Globals.SpriteBatch.Draw(
                    Globals.Content.Load<Texture2D>("debug_rect"),
                    collision,
                    null,
                    Color.Red,
                    0f,
                    new Vector2(0, 0),
                    SpriteEffects.None,
                    1f);
            }

            // Draw Player Position/Rectangle
            Rectangle realPositionRect = new Rectangle(
                (int)_player.RealPos.X,
                (int)_player.RealPos.Y,
                _player.PlayerTexture.Width / 20,
                _player.PlayerTexture.Height / 10);
            Globals.SpriteBatch.Draw(
                Globals.Content.Load<Texture2D>("debug_rect"),
                realPositionRect,
                null,
                Color.Blue,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.993f);

            // Draw the player's hitbox for debugging, using a purple overlay.
            Rectangle playerHitbox = _player.Hitbox;
            Globals.SpriteBatch.Draw(
                Globals.Content.Load<Texture2D>("debug_rect"),
                playerHitbox,
                null,
                Color.Purple,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.991f);
            Globals.SpriteBatch.End();
        }
    }
}