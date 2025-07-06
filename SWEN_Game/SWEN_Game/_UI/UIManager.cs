using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Input;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Style;
using SWEN_Game._Managers;
using SWEN_Game._Shooting;
using SWEN_Game._Utils;
using SWEN_Game._Interfaces;

namespace SWEN_Game._UI
{
    public class UIManager
    {
        public LevelUpUI GetLevelUpUI() => _levelUpUI;
        private readonly IGameStateManager _gameStateManager;
        private readonly UiSystem _uiSystem;
        private readonly MainMenuUI _mainMenuUI;
        private readonly InGameUI _inGameUI;
        private readonly GameOverUI _gameOverUI;
        private readonly WinUI _winUI;
        private readonly LevelUpUI _levelUpUI;
        private Texture2D _backgroundTexture;
        private InputHandler _inputHandler;

        private bool wasEscPressed = false;

        public UIManager(IGameStateManager gameStateManager, Game game, ContentManager content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            _gameStateManager = gameStateManager;
            _backgroundTexture = content.Load<Texture2D>("Menu/Background");
            _inputHandler = new InputHandler(game);

            var style = new UntexturedStyle(spriteBatch)
            {
                Font = new GenericSpriteFont(content.Load<SpriteFont>("Menu/GameFont")),
                TextColor = Color.Black,
                TextScale = 1.5F,
                PanelTexture = new NinePatch(content.Load<Texture2D>("Menu/nine_path_panel"), padding: 6),
                ButtonTexture = new NinePatch(content.Load<Texture2D>("Menu/button_hover"), padding: 6),
                CheckboxTexture = new NinePatch(content.Load<Texture2D>("Menu/slider_progress_hover"), padding: 6),
                CheckboxCheckmark = new TextureRegion(content.Load<Texture2D>("Menu/checked_disabled"), new Rectangle(0, 0, 16, 16)),
            };

            _uiSystem = new UiSystem(game, style, _inputHandler)
            {
                AutoScaleWithScreen = true,
            };

            Globals.UiSystem = _uiSystem;
            _mainMenuUI = new MainMenuUI(_uiSystem, _gameStateManager, game);
            _inGameUI = new InGameUI(_uiSystem);
            _gameOverUI = new GameOverUI(_uiSystem, gameStateManager);
            _winUI = new WinUI(_uiSystem, gameStateManager);
            _levelUpUI = new LevelUpUI(_uiSystem, _gameStateManager);

            _mainMenuUI.Show();
        }

        public void Update(GameTime gameTime)
        {
            _inputHandler.Update();
            _uiSystem.Update(gameTime);

            _inGameUI.Update(gameTime, _gameStateManager.CurrentGameState); // Ensure InGameUI visibility is updated properly

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape) && !wasEscPressed)
            {
                wasEscPressed = true;
                if (_gameStateManager.CurrentGameState == GameState.Playing)
                {
                    _gameStateManager.CaptureLastFrame();

                    _gameStateManager.ChangeGameState(GameState.Paused);
                    _mainMenuUI.ClearAndSwitch(MenuState.Paused);
                    _mainMenuUI.Show();
                }
                else if (_gameStateManager.CurrentGameState == GameState.Paused)
                {
                    _gameStateManager.ChangeGameState(GameState.Playing);
                    _mainMenuUI.Hide();
                }
            }

            if (keyboardState.IsKeyUp(Keys.Escape))
            {
                wasEscPressed = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_gameStateManager.CurrentGameState == GameState.Paused)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Globals.WinRenderTarget, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.White * 0.3f);
                spriteBatch.End();
            }
            else if (_gameStateManager.CurrentGameState == GameState.MainMenu ||
                     _gameStateManager.CurrentGameState == GameState.GameOver)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.White);
                spriteBatch.End();
            }
            else if (_gameStateManager.CurrentGameState == GameState.GameWon)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Globals.WinRenderTarget, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.White);
                spriteBatch.End();
            }
            else if (_gameStateManager.CurrentGameState == GameState.LevelUp)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Globals.WinRenderTarget, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.White * 0.3f);
                spriteBatch.End();
            }

            _uiSystem.Draw(gameTime, spriteBatch);
        }

        public void OnGameStateChanged(GameState newState)
        {
            if (newState == GameState.Playing)
            {
                _mainMenuUI.Hide();
                _inGameUI.Show();
                _gameOverUI.Hide();
                _winUI.Hide();
            }
            else if (newState == GameState.MainMenu)
            {
                _mainMenuUI.ClearAndSwitch(MenuState.MainMenu);
                _mainMenuUI.Show();
                _inGameUI.Hide();
                _gameOverUI.Hide();
                _winUI.Hide();
            }
            else if (newState == GameState.Paused)
            {
                _mainMenuUI.ClearAndSwitch(MenuState.Paused);
                _mainMenuUI.Show();
                _inGameUI.Hide();
                _gameOverUI.Hide();
                _winUI.Hide();
            }
            else if (newState == GameState.GameOver)
            {
                _mainMenuUI.Hide();
                _inGameUI.Hide();
                _gameOverUI.Show();
                _winUI.Hide();
            }
            else if (newState == GameState.GameWon)
            {
                _mainMenuUI.Hide();
                _inGameUI.Hide();
                _gameOverUI.Hide();
                _winUI.Show();
            }
            else if (newState == GameState.LevelUp)
            {
                _mainMenuUI.Hide();
                _inGameUI.Show();
                _gameOverUI.Hide();
                _winUI.Hide();
                _levelUpUI.Show();
            }
        }

        // Add the following definition for SliderStyle if it is missing in your project
        public class SliderStyle
        {
            public NinePatch Background { get; set; }
            public TextureRegion Grabber { get; set; }
        }

        public void SetWeaponManager(WeaponManager weaponManager)
        {
            _levelUpUI.SetWeaponManager(weaponManager);
        }
    }
}
