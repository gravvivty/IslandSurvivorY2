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
        private readonly Debug _debug;

        public GameManager()
        {
            _spriteManager = new SpriteManager();
            _spriteManager.MapTileToTexture();
            _player = new Player();
            _player.AddSpriteManager(_spriteManager);
            _spriteCalculator = new SpriteCalculator(_spriteManager, _player);
            _renderer = new Renderer(_player, _spriteManager, _spriteCalculator);

            _debug = new Debug(_player, _renderer);

            // Calculates ALL collisions in the level
            Globals.CalculateAllCollisions();
        }

        public void Update()
        {
           // System.Diagnostics.Debug.WriteLine("GameManager Update running" + DateTime.Now);

            // Every Frame check input
            KeyboardState keyboard = Keyboard.GetState();
            InputManager.Update(_player, keyboard);
            _player.Update();
        }

        public void Draw()
        {
            // Begin the sprite batch with depth sorting (FrontToBack) and apply the camera transformation.
            Globals.SpriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                transformMatrix: _renderer.CalcTranslation(),
                samplerState: SamplerState.PointClamp);
            _renderer.DrawWorld();
            Globals.SpriteBatch.End();

            _debug.DrawWorldDebug();
            Cursor.DrawCursor();

           // System.Diagnostics.Debug.WriteLine("GameManager Draw running" + DateTime.Now);
        }
    }
}
