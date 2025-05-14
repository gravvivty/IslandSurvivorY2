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
        uiSystem.Add("MainMenu", rootPanel);

        // START Button: Switch game state to Playing
        var startButton = new Button(Anchor.TopCenter, new Vector2(200, 40));
        startButton.OnPressed += _ => {
            GameStateManager.ChangeGameState(GameState.Playing);
        };
        rootPanel.AddChild(startButton);

        // OPTIONS Button: Placeholder for now (can be expanded later)
        var optionsButton = new Button(Anchor.AutoCenter, new Vector2(200, 40));
        optionsButton.OnPressed += _ => {
            // TODO: Open options menu here
            System.Console.WriteLine("Options clicked");
        };
        rootPanel.AddChild(optionsButton);

        // EXIT Button: Exits the game
        var exitButton = new Button(Anchor.BottomCenter, new Vector2(200, 40));
        exitButton.OnPressed += _ => {
            uiSystem.Game.Exit(); // Close the game
        };
        rootPanel.AddChild(exitButton);
    }

    // Call this to show the main menu
    public void Show()
    {
        if (rootPanel != null)
            rootPanel.IsHidden = false;
    }

    // Call this to hide the main menu
    public void Hide()
    {
        if (rootPanel != null)
            rootPanel.IsHidden = true;
    }
}
