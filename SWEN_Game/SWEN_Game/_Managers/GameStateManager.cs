using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace SWEN_Game
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused
    }

    public class GameStateManager
    {
        private readonly MainGame _game;
        public GameState CurrentGameState { get; private set; } = GameState.MainMenu;
        private UIManager _uiManager;
        private GameManager _gameManager;

        public GameStateManager(Game game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, SpriteBatch spriteBatch)
        {
            _uiManager = new UIManager(this, game, content, graphicsDeviceManager, spriteBatch);
        }

        public void ChangeGameState(GameState newGameState)
        {
            CurrentGameState = newGameState;
            if (newGameState == GameState.Playing && _gameManager == null)
            {
                _gameManager = new GameManager();

            }
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentGameState == GameState.MainMenu || CurrentGameState == GameState.Paused)
            {
                _uiManager.Update(gameTime);
            }

            if (CurrentGameState == GameState.Playing)
            {
                _uiManager?.Update(gameTime);
                _gameManager?.Update();
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            if (CurrentGameState == GameState.MainMenu || CurrentGameState == GameState.Paused)
            {
                _uiManager.Draw(gameTime, _spriteBatch);
            }

            if (CurrentGameState == GameState.Playing)
            {
                _gameManager?.Draw();
            }
        }
    }
}
