using Microsoft.Xna.Framework;
using MLEM.Ui;
using MLEM.Ui.Elements;
using SWEN_Game._Managers;
using SWEN_Game._Interfaces;
using SWEN_Game._Sound;

namespace SWEN_Game._UI
{
    public class WinUI
    {
        private readonly UiSystem _uiSystem;
        private readonly IGameStateManager _gameStateManager;

        private Panel _winPanel;

        private Paragraph _winParagraph;
        private Button _restartBtn;
        private Button _mainMenuBtn;

        public WinUI(UiSystem uiSystem, IGameStateManager gameStateManager)
        {
            _uiSystem = uiSystem;
            _gameStateManager = gameStateManager;

            _winPanel = new Panel(Anchor.Center, new Vector2(400, 300))
            {
                DrawColor = Color.Black * 0.7f,
                IsHidden = true,
            };
            _uiSystem.Add("WinPanel", _winPanel);

            _winParagraph = new Paragraph(Anchor.TopCenter, 0.9F, "Congratulations!\nYou did it!", true)
            {
                TextColor = Color.Yellow,
                PositionOffset = new Vector2(0, 20),
            };
            _winPanel.AddChild(_winParagraph);

            _restartBtn = new Button(Anchor.Center, new Vector2(200, 50), "Restart")
            {
                PositionOffset = new Vector2(0, 0),
            };

            _restartBtn.OnPressed += (ele) =>
            {
                _gameStateManager.ResetGame();
                Hide();
            };
            _winPanel.AddChild(_restartBtn);

            _mainMenuBtn = new Button(Anchor.Center, new Vector2(200, 50), "Main Menu")
            {
                PositionOffset = new Vector2(0, 60),
            };

            _mainMenuBtn.OnPressed += (ele) =>
            {
                _gameStateManager.ChangeGameState(GameState.MainMenu); // Go to main menu
                SongManager.Instance.Stop();
                SongManager.Instance.Play("Main");
                Hide();
            };

            _winPanel.AddChild(_mainMenuBtn);
        }

        public void Show()
        {
            _winPanel.IsHidden = false;
            SongManager.Instance.Play("Portal");
        }

        public void Hide()
        {
            _winPanel.IsHidden = true;
        }
    }
}
