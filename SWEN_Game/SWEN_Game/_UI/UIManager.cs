using MLEM.Font;
using MLEM.Input;
using MLEM.Maths;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Managers;
using SWEN_Game._Utils;

namespace SWEN_Game._UI
{
    public class UIManager
    {
        private readonly GameStateManager _gameStateManager;
        private readonly UiSystem _uiSystem;
        private readonly MainMenuUI _mainMenuUI;
        private Texture2D _backgroundTexture;
        private InputHandler _inputHandler;

        private RenderTarget2D _gameRenderTarget;
        private bool wasEscPressed = false;

        public UIManager(GameStateManager gameStateManager, Game game, ContentManager content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            _gameStateManager = gameStateManager;
            _backgroundTexture = content.Load<Texture2D>("Menu/Background");
            _inputHandler = new InputHandler(game);

            var style = new UntexturedStyle(spriteBatch)
            {
                Font = new GenericSpriteFont(content.Load<SpriteFont>("Menu/GameFont")),
                TextColor = Color.Gray,
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
            _mainMenuUI.Show();
        }

        public void Update(GameTime gameTime)
        {
            _inputHandler.Update();
            _uiSystem.Update(gameTime);

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape) && !wasEscPressed)
            {
                wasEscPressed = true;
                if (_gameStateManager.CurrentGameState == GameState.Playing)
                {
                    _gameRenderTarget = new RenderTarget2D(Globals.Graphics.GraphicsDevice, Globals.WindowSize.X, Globals.WindowSize.Y);
                    Globals.Graphics.GraphicsDevice.SetRenderTarget(_gameRenderTarget);
                    Globals.Graphics.GraphicsDevice.Clear(Color.White);
                    if (_gameRenderTarget != null)
                    {
                        // Globals.SpriteBatch.Begin();
                        _gameStateManager.Draw(gameTime, Globals.SpriteBatch);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Game render target is null, cannot draw the game state to the UI.");
                    }

                    _gameStateManager.Update(gameTime);
                    Globals.Graphics.GraphicsDevice.SetRenderTarget(null);

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
            if  (_gameStateManager.CurrentGameState == GameState.Paused)
            {
                if(_gameRenderTarget != null)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(_gameRenderTarget, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.White * 0.3f);
                    spriteBatch.End();
                }
            }
            else if (_gameStateManager.CurrentGameState == GameState.MainMenu)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, Globals.WindowSize.X, Globals.WindowSize.Y), Color.White);
                spriteBatch.End();
            }

            _uiSystem.Draw(gameTime, spriteBatch);
        }

        // Add the following definition for SliderStyle if it is missing in your project
        public class SliderStyle
        {
            public NinePatch Background { get; set; }
            public TextureRegion Grabber { get; set; }
        }
    }
}
