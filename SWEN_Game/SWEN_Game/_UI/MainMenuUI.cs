using Microsoft.Xna.Framework;
using MLEM.Ui;
using MLEM.Ui.Elements;

namespace SWEN_Game;

public class MainMenuUI
{
    private readonly UiSystem ui;
    private Panel rootPanel;

    public MainMenuUI(UiSystem uiSystem)
    {
        this.ui = uiSystem;

        // Create the root panel that contains all menu elements
        rootPanel = new Panel(Anchor.Center, new Vector2(300, 300), Vector2.Zero, false, true);
        rootPanel.Texture = null;
        uiSystem.Add("MainMenu", rootPanel);

        // START Button: Switch game state to Playing
        var startButton = new Button(Anchor.AutoCenter, new Vector2(500, 100), "Start");
        startButton.OnPressed += _ =>
        {
            System.Diagnostics.Debug.WriteLine("Start Clicked");
            GameStateManager.ChangeGameState(GameState.Playing);
        };
        rootPanel.AddChild(startButton);

        var optionsButton = new Button(Anchor.AutoCenter, new Vector2(500, 100), "Options");
        startButton.OnPressed += _ =>
        {
            System.Diagnostics.Debug.WriteLine("Options Clicked");
        };
        rootPanel.AddChild(optionsButton);

        var exitButton = new Button(Anchor.AutoCenter, new Vector2(500, 100), "Exit");
        exitButton.OnPressed += _ =>
        {
            System.Diagnostics.Debug.WriteLine("Exit Clicked");
            uiSystem.Game.Exit();
        };
        rootPanel.AddChild(exitButton);
    }

    public void Show() => rootPanel.IsHidden = false;
    public void Hide() => rootPanel.IsHidden = true;
}
