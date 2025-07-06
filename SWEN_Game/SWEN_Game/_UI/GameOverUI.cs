using Microsoft.Xna.Framework;
using MLEM.Ui;
using MLEM.Ui.Elements;
using SWEN_Game._Managers;
using SWEN_Game._Interfaces;
using SWEN_Game._Sound;
using SWEN_Game._Utils;
namespace SWEN_Game._UI
{
    public class GameOverUI
    {
        private readonly UiSystem _uiSystem;
        private readonly IGameStateManager _gameStateManager;

        private Panel _gameOverPanel;

        private Paragraph _gameOverParagraph;
        private Button _restartBtn;
        private Button _mainMenuBtn;

        public GameOverUI(UiSystem uiSystem, IGameStateManager gameStateManager)
        {
            _uiSystem = uiSystem;
            _gameStateManager = gameStateManager;

            _gameOverPanel = new Panel(Anchor.Center, new Vector2(400, 300))
            {
                DrawColor = Color.Black * 0.7f,
                IsHidden = true,
            };
            _uiSystem.Add("GameOverPanel", _gameOverPanel);

            _gameOverParagraph = new Paragraph(Anchor.TopCenter, 1, "Game Over!", true)
            {
                TextColor = Color.Red,
                PositionOffset = new Vector2(0, 20),
            };
            _gameOverPanel.AddChild(_gameOverParagraph);

            _restartBtn = new Button(Anchor.Center, new Vector2(200, 50), "Restart")
            {
                PositionOffset = new Vector2(0, -20),
            };

            _restartBtn.OnMouseEnter += (e) => SFXManager.Instance.Play("uiSelect");
            _restartBtn.OnPressed += (ele) =>
            {
                _gameStateManager.ResetGame();
                SFXManager.Instance.SetVolume(Globals.SoundVolume);
                SongManager.Instance.SetVolume(Globals.MusicVolume);
                SFXManager.Instance.Play("uiConfirm");
                Hide();
            };
            _gameOverPanel.AddChild(_restartBtn);

            _mainMenuBtn = new Button(Anchor.Center, new Vector2(200, 50), "Main Menu")
            {
                PositionOffset = new Vector2(0, 50),
            };

            _mainMenuBtn.OnMouseEnter += (e) => SFXManager.Instance.Play("uiSelect");
            _mainMenuBtn.OnPressed += (ele) =>
            {
                _gameStateManager.ChangeGameState(GameState.MainMenu); // Go to main menu
                SFXManager.Instance.Play("uiConfirm");
                SongManager.Instance.Play("Main");
                Hide();
            };

            _gameOverPanel.AddChild(_mainMenuBtn);
        }

        public void Show()
        {
            _gameOverPanel.IsHidden = false;
            SFXManager.Instance.Play("playerDeath");
            SongManager.Instance.Stop();
        }

        public void Hide()
        {
            _gameOverPanel.IsHidden = true;
        }
    }
}
