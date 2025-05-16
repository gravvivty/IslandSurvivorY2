using Microsoft.Xna.Framework;
using MLEM.Maths;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;

namespace SWEN_Game;

public enum MenuState
{
    MainMenu,
    Options,
    Playing
}

public class MainMenuUI
{
    private readonly UiSystem ui;
    private Panel rootPanel;
    private MenuState currentMenuState;

    public MainMenuUI(UiSystem uiSystem)
    {
        this.ui = uiSystem;

        // Create the root panel that contains all menu elements
        rootPanel = new Panel(Anchor.Center, new Vector2(0.9F, 0.2F), Vector2.Zero);
        rootPanel.Texture = null;
        uiSystem.Add("MainMenu", rootPanel);

        ClearAndSwitchMenu(MenuState.MainMenu);
    }

    private void ClearAndSwitchMenu(MenuState menuState)
    {
        currentMenuState = menuState;
        rootPanel.RemoveChildren();

        switch (menuState)
        {
            case MenuState.MainMenu:
                ShowMainMenu();
                break;
            case MenuState.Options:
                ShowOptionsMenu();
                break;
        }
    }

    private void ShowMainMenu()
    {
        var buttonPanel = new Panel(Anchor.Center, new Vector2(1f, 1f), Vector2.Zero);
        rootPanel.AddChild(buttonPanel);
        // START Button: Switch game state to Playing
        var playButton = new Button(Anchor.AutoInline, new Vector2(0.3F, 0.8F), "Play")
        {
            PositionOffset = new Vector2(50, 0),
            OnPressed = _ => {
                Hide();
                GameStateManager.ChangeGameState(GameState.Playing);
            }
        };

        buttonPanel.AddChild(playButton);


        var optionsButton = new Button(Anchor.AutoInline, new Vector2(0.3F, 0.8F), "Options")
        {
            PositionOffset = new Vector2(50, 0),
            OnPressed = _ =>
            {
                System.Diagnostics.Debug.WriteLine("Options Clicked");
                ClearAndSwitchMenu(MenuState.Options);
            }
        };
        buttonPanel.AddChild(optionsButton);

        var exitButton = new Button(Anchor.AutoInline, new Vector2(0.3F, 0.8F), "Exit")
        {
            PositionOffset = new Vector2(50, 0),
            OnPressed = _ =>
            {
                System.Diagnostics.Debug.WriteLine("Exit Clicked");
                ui.Game.Exit();
            }
        };
        buttonPanel.AddChild(exitButton);
    }
     

    private void ShowOptionsMenu()
    {
        // Create a back button
        var backButton = new Button(Anchor.AutoInline, new Vector2(0.3F, 0.8F), "Back");
        backButton.PositionOffset = new Vector2(50, 0);
        backButton.OnPressed += _ =>
        {
            System.Diagnostics.Debug.WriteLine("Back Clicked");
            ClearAndSwitchMenu(MenuState.MainMenu);
        };
        rootPanel.AddChild(backButton);
    }

    public void Show() => rootPanel.IsHidden = false;
    public void Hide() => rootPanel.IsHidden = true;
}
