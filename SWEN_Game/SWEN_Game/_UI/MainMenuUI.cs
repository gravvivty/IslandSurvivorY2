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
        rootPanel = new Panel(Anchor.Center, new Vector2(300, 200), Vector2.Zero);
        rootPanel.Texture = null;
        uiSystem.Add("MainMenu", rootPanel);

        // START Button: Switch game state to Playing
        var startButton = new Button(Anchor.TopCenter, new Vector2(300, 60));
        startButton.OnPressed += _ =>
        {
            System.Console.WriteLine("Start Clicked");
            GameStateManager.ChangeGameState(GameState.Playing);
        };
        rootPanel.AddChild(startButton);
    }

    public void Show() => rootPanel.IsHidden = false;
    public void Hide() => rootPanel.IsHidden = true;
}
