using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;
using SWEN_Game._Entities;
using SWEN_Game._Graphics;
using SWEN_Game._Items;

namespace SWEN_Game._Utils
{
    /// <summary>
    /// Provides debug functionality such as input-based toggles, power-up injection,
    /// and hitbox/collision rendering for development and testing.
    /// </summary>
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

        /// <summary>
        /// Checks debug input and executes relevant debug actions, such as toggling invincibility or applying powerups.
        /// </summary>
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
            if (_currentKeyboardState.IsKeyDown(Keys.F1) && !_previousKeyboardState.IsKeyDown(Keys.F1))
            {
                // _powerupManager.AddItem(1); // itemID 1 = GunpowderPowerup
                 _powerupManager.AddItem(2); // itemID 2 = ReverseShotPowerup
                 _powerupManager.AddItem(3); // itemID 3 = PiercerPowerup
                // _powerupManager.AddItem(4); // itemID 4 = AdrenalinePowerup
                // _powerupManager.AddItem(5); // itemID 5 = RocketspeedPowerup
                // _powerupManager.AddItem(6); // itemID 6 = RancidEnergyDrinkPowerup
                 _powerupManager.AddItem(7); // itemID 7 = DemonBulletsPowerup
                // _powerupManager.AddItem(8); // itemID 8 = QuickHandsPowerup
                // _powerupManager.AddItem(9); // itemID 9 = SpicyNoodlesPowerup
                 _powerupManager.AddItem(10); // itemID 10 = DeadeyePowerup

                PlayerGameData.Instance.UpdateWeaponGameData();
            }
        }

        /// <summary>
        /// Draws visual debug overlays, including player hitboxes, real position rectangles, and collision boxes.
        /// </summary>
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