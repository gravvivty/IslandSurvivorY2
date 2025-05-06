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
        private readonly Debug _debug;

        public GameManager()
        {
            _spriteManager = new SpriteManager();
            _spriteManager.MapTileToTexture();
            _player = new Player();
            _player.AddSpriteManager(_spriteManager);
            _spriteCalculator = new SpriteCalculator(_spriteManager, _player);
            _renderer = new Renderer(_player, _spriteManager, _spriteCalculator);

            _weaponManager = new WeaponManager(new Pistol(Globals.Content.Load<Texture2D>("pistol_bullet"), new Vector2(100, 100)));

            _debug = new Debug(_player, _renderer);

            // Calculates ALL collisions in the level
            Globals.CalculateAllCollisions();
        }

        public void Update()
        {
            System.Diagnostics.Debug.WriteLine("GameManager Update running" + DateTime.Now);

            // Every Frame check input
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            InputManager.Update(_player, _weaponManager, keyboard, mouse);
            _player.Update();
            _weaponManager.Update();
        }

        public void Draw()
        {
            // Begin the sprite batch with depth sorting (FrontToBack) and apply the camera transformation.
            Globals.SpriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: _renderer.CalcTranslation(),
                samplerState: SamplerState.PointClamp);
            _renderer.DrawWorld();
            foreach (var bullet in _weaponManager.GetWeapon().GetBullets())
            {
                bullet.Draw(Globals.SpriteBatch);
            }

            Globals.SpriteBatch.End();

            _debug.DrawWorldDebug();
            Cursor.DrawCursor();

            System.Diagnostics.Debug.WriteLine("GameManager Draw running" + DateTime.Now);
        }
    }
}
