using LDtk;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Interfaces;
using SWEN_Game._Sound;
using SWEN_Game._UI;
using SWEN_Game._Utils;

namespace SWEN_Game._Managers
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        GameWon,
        LevelUp,
    }

    public class GameStateManager : IGameStateManager
    {
        public GameState CurrentGameState { get; set; } = GameState.MainMenu;
        private UIManager _uiManager;
        private GameManager _gameManager;
        public UIManager GetUIManager() => _uiManager;

        public GameStateManager(Game game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, SpriteBatch spriteBatch)
        {
            Globals.SpriteBatch = spriteBatch;
            Globals.GameStateManager = this;
            Globals.Content = content;
            Globals.File = LDtkFile.FromFile("World", content);
            Globals.World = Globals.File.LoadWorld(Worlds.World.Iid);
            Globals.Collisions = new List<Rectangle>();
            Globals.Graphics = graphicsDeviceManager;
            Globals.WindowSize = new Point(graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight);
            _uiManager = new UIManager(this, game, content, graphicsDeviceManager, spriteBatch);
        }

        public void ChangeGameState(GameState newGameState)
        {
            CurrentGameState = newGameState;
            _uiManager.OnGameStateChanged(newGameState);
        }

        public void Update(GameTime gameTime)
        {
            Globals.LastGameTime = gameTime;

            if (CurrentGameState == GameState.Playing)
            {
                Globals.UpdateTime(gameTime);
                _gameManager?.Update();

                if (Globals.TotalGameTime >= 601)
                {
                    CaptureLastFrame();
                    ChangeGameState(GameState.GameWon);
                }
            }

            _uiManager.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (CurrentGameState == GameState.Playing)
            {
                _gameManager?.Draw();
            }

            _uiManager.Draw(gameTime, spriteBatch);
        }

        public void ResetGame()
        {
            Globals.TotalGameTime = 0;
            SFXManager.Instance.ResetCooldowns();
            Globals.WinRenderTarget?.Dispose();
            Globals.WinRenderTarget = null;
            _gameManager = new GameManager(this); // Re-create everything
            ChangeGameState(GameState.Playing);
        }

        public void CaptureLastFrame()
        {
            var device = Globals.Graphics.GraphicsDevice;

            var renderTarget = new RenderTarget2D(device, Globals.WindowSize.X, Globals.WindowSize.Y);
            device.SetRenderTarget(renderTarget);
            device.Clear(Color.White);

            Draw(Globals.LastGameTime, Globals.SpriteBatch); // Draw current game frame

            device.SetRenderTarget(null);
            Globals.WinRenderTarget = renderTarget; // Store it globally
        }
    }
}
