using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Managers;
using SWEN_Game._Sound;

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
            Window.Title = "Island Survivor";
            IsMouseVisible = false;

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
            var songs = new Dictionary<string, string>
            {
                { "World", "Music/deltaruneWorldRevolving" },
                { "Portal", "Music/portalRadio" },
                { "Main", "Music/main" },
            };

            var sounds = new Dictionary<string, string>
            {
                { "enemyDeath", "SFX/enemyDeath" },
                { "enemyHit", "SFX/enemyHit" },
                { "levelUp", "SFX/levelUp" },
                { "playerHit", "SFX/playerHit" },
                { "enemyShoot", "SFX/enemyShoot" },
                { "reload", "SFX/reload" },
                { "slow", "SFX/slow" },
                { "uiConfirm", "SFX/uiConfirm" },
                { "uiSelect", "SFX/uiSelect" },
                { "enemyHitCrit", "SFX/enemyHitCrit" },
                { "playerDeath", "SFX/playerDeath" },
                { "splat", "SFX/splat" },
                { "akShoot", "SFX/akShoot" },
                { "blunderbussShoot", "SFX/blunderbussShoot" },
                { "pistolShoot", "SFX/pistolShoot" },
                { "precisionShoot", "SFX/precisionShoot" },
                { "revolverShoot", "SFX/revolverShoot" },
            };
            // Create necessary classes and set Global Values
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SFXManager.Instance.LoadSounds(Content, sounds);
            SongManager.Instance.LoadSongs(Content, songs);
            _gameStateManager = new GameStateManager(this, _graphics, Content, _spriteBatch);
            SongManager.Instance.Play("Main");
        }

        protected override void Update(GameTime gameTime)
        {
            IsMouseVisible = _gameStateManager.CurrentGameState != GameState.Playing;
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
