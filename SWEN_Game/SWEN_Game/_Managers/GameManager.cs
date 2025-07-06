using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SWEN_Game._Entities;
using SWEN_Game._Graphics;
using SWEN_Game._Interfaces;
using SWEN_Game._Items;
using SWEN_Game._Shooting;
using SWEN_Game._Sound;
using SWEN_Game._Utils;

namespace SWEN_Game._Managers
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
        private readonly PlayerGameData _playerGameData;
        private readonly IGameStateManager _gameStateManager;

        public GameManager(IGameStateManager manager)
        {
            _gameStateManager = manager;
            _playerGameData = new PlayerGameData(_gameStateManager);
            _spriteManager = new SpriteManager();
            _spriteManager.MapTileToTexture();
            _player = new Player(PlayerGameData.Instance, _gameStateManager);
            _player.InitPlayer(_spriteManager);
            _spriteCalculator = new SpriteCalculator(_spriteManager, _player);
            _renderer = new Renderer(_player, _spriteManager, _spriteCalculator);

            _weaponManager = new WeaponManager();
            _weaponManager.InitWeapons();
            _playerWeapon = new PlayerWeapon(_weaponManager);

            _enemyManager = new EnemyManager(_player);

            _powerupManager = new PowerupManager(_playerWeapon, PlayerGameData.Instance);
            Globals.GameStateManager.GetUIManager().GetLevelUpUI().SetPowerupManager(_powerupManager);
            Globals.GameStateManager.GetUIManager().SetWeaponManager(_weaponManager);

            _debug = new Debug(_player, _renderer, _powerupManager, _enemyManager, _playerWeapon);

            // Calculates all collisions in the level
            Globals.CalculateAllCollisions();

            // Calculate all world hitboxes
            Globals.CalculateAllHitboxes();

            // Set Global Classes
            Globals.SpriteManager = _spriteManager;

            SongManager.Instance.Stop();
            SongManager.Instance.Play("World");
            SongManager.Instance.SetVolume(Globals.SoundVolume);
        }

        /// <summary>
        /// Calls all Update functions in all other classes.
        /// </summary>
        public void Update()
        {
            // Every Frame check input
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            InputManager.Update(_player, keyboard);
            MouseManager.UpdateMouse(_player, _playerWeapon, mouse);
            _player.Update();
            _playerWeapon.Update();

            // - 6 on the x axis so that the enemies try to hit the "middle"
            _enemyManager.Update(_playerWeapon.GetBullets(), _player.RealPos);

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

            _debug.DrawWorldDebug();
            Cursor.DrawCursor();
        }
    }
}
