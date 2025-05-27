using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;

namespace SWEN_Game
{
    public class GameManager
    {
        private readonly Player _player;
        private readonly Renderer _renderer;
        private readonly SpriteManager _spriteManager;
        private readonly SpriteCalculator _spriteCalculator;
        private readonly WeaponManager _weaponManager;
        private readonly PlayerWeapon _playerWeapon;
        private readonly PowerupManager _powerupManager;
        private readonly Debug _debug;
        private readonly EnemyManager _enemyManager;

        public GameManager()
        {
            _spriteManager = new SpriteManager();
            _spriteManager.MapTileToTexture();
            _player = new Player();
            _player.AddSpriteManager(_spriteManager);
            _spriteCalculator = new SpriteCalculator(_spriteManager, _player);
            _renderer = new Renderer(_player, _spriteManager, _spriteCalculator);

            _weaponManager = new WeaponManager();
            _weaponManager.InitWeapons();
            _playerWeapon = new PlayerWeapon(_weaponManager);

            _enemyManager = new EnemyManager(_player);

            _powerupManager = new PowerupManager(_playerWeapon);

            _debug = new Debug(_player, _renderer, _powerupManager);

            // Calculates all collisions in the level
            Globals.CalculateAllCollisions();

            // Calculate all world hitboxes
            Globals.CalculateAllHitboxes();

            // Set Global Classes
            Globals.SpriteManager = _spriteManager;
        }

        /// <summary>
        /// Calls all Update functions in all other classes.
        /// </summary>
        public void Update()
        {
           // System.Diagnostics.Debug.WriteLine("GameManager Update running" + DateTime.Now);

            // Every Frame check input
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            InputManager.Update(_player, keyboard);
            MouseManager.UpdateMouse(_player, _playerWeapon, mouse);
            _player.Update();
            _playerWeapon.Update();

            // - 6 on the x axis so that the enemies try to hit the "middle"
            _enemyManager.Update(_playerWeapon.GetBullets(), _player.Position - new Vector2(6, 0));

            // Debug
            _debug.DebugUpdate();
        }

        /// <summary>
        /// Calls all Draw functions in all other classes.
        /// </summary>
        public void Draw()
        {
            // Begin the sprite batch with depth sorting (FrontToBack) and apply the camera transformation.
            Globals.SpriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: _renderer.CalcTranslation(),
                samplerState: SamplerState.PointClamp);

            _renderer.DrawWorld();
            _playerWeapon.DrawBullets();
            _enemyManager.Draw();
            _player.Draw();

            Globals.SpriteBatch.End();

            // Custom draw functions that differ from basic game draw logic

            // _debug.DrawWorldDebug();
            Cursor.DrawCursor();

            System.Diagnostics.Debug.WriteLine("GameManager Draw running" + DateTime.Now);
        }
    }
}
