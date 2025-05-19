using System.Collections.Generic;
using LDtk;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Input;
using MLEM.Maths;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Style;

namespace SWEN_Game
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameStateManager _gameStateManager;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.HardwareModeSwitch = false;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create necessary classes and set Global Values
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Setze globale Werte
            Globals.SpriteBatch = _spriteBatch;
            Globals.Content = Content;
            Globals.File = LDtkFile.FromFile("World", Content);
            Globals.World = Globals.File.LoadWorld(Worlds.World.Iid);
            Globals.Collisions = new List<Rectangle>();
            Globals.Graphics = _graphics;
            Globals.WindowSize = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            _gameStateManager = new GameStateManager(this, _graphics, Content, _spriteBatch);
        }


        protected override void Update(GameTime gameTime)
        {
            _gameStateManager.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _gameStateManager.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
