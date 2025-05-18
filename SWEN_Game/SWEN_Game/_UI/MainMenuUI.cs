using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Maths;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;

namespace SWEN_Game;

public enum MenuState
{
    MainMenu,
    Options
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
        rootPanel = new Panel(Anchor.Center, new Vector2(0.8F, 0.2F), Vector2.Zero);
        //rootPanel.Texture = null;
        uiSystem.Add("MainMenu", rootPanel);

        ClearAndSwitch(MenuState.MainMenu);
    }

    public void ClearAndSwitch(MenuState newMenuState)
    {
        currentMenuState = newMenuState;
        rootPanel.RemoveChildren();
        switch (currentMenuState)
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
        // START Button: Switch game state to Playing
        var playButton = new Button(Anchor.AutoInline, new Vector2(0.3F, 0.6F), "Play");
        playButton.PositionOffset = new Vector2(30,0);

        playButton.OnPressed += _ =>
        {
            // System.Diagnostics.Debug.WriteLine("Start Clicked");
            GameStateManager.ChangeGameState(GameState.Playing);
        };
        rootPanel.AddChild(playButton);

        var optionsButton = new Button(Anchor.AutoInline, new Vector2(0.3F,0.6F), "Options");
        optionsButton.PositionOffset = new Vector2(30, 0);
        optionsButton.OnPressed += _ => ClearAndSwitch(MenuState.Options);
        {
            System.Diagnostics.Debug.WriteLine("Options Clicked");
        };
        rootPanel.AddChild(optionsButton);

        var exitButton = new Button(Anchor.AutoInline, new Vector2(0.3F, 0.6F), "Exit");
        exitButton.PositionOffset = new Vector2(30, 0);
        exitButton.OnPressed += _ => 
        {
            System.Diagnostics.Debug.WriteLine("Exit Clicked");
            ui.Game.Exit();
        };
        rootPanel.AddChild(exitButton);
    }

    private void ShowOptionsMenu()
    {

        var dropdown = new Dropdown(Anchor.AutoLeft, new Vector2(0.5F, 0.6F), "Window Size");

        dropdown.AddElement("1280x720", element =>
        {
            System.Diagnostics.Debug.WriteLine("1280x720 Clicked");
            Globals.WindowSize = new Point(1280, 720);
            Globals.Graphics.PreferredBackBufferWidth = 1280;
            Globals.Graphics.PreferredBackBufferHeight = 720;
            Globals.Graphics.ApplyChanges();
            dropdown.IsOpen = false;
        }, 0);

        dropdown.AddElement("1600x900", element =>
        {
            System.Diagnostics.Debug.WriteLine("1600x900 Clicked");
            Globals.WindowSize = new Point(1600, 900);
            Globals.Graphics.PreferredBackBufferWidth = 1600;
            Globals.Graphics.PreferredBackBufferHeight = 900;
            Globals.Graphics.ApplyChanges();
            dropdown.IsOpen = false;
        }, 0);

        dropdown.AddElement("1920x1080", element =>
        {
            System.Diagnostics.Debug.WriteLine("1920x1080 Clicked");
            Globals.WindowSize = new Point(1920, 1080);
            Globals.Graphics.PreferredBackBufferWidth = 1920;
            Globals.Graphics.PreferredBackBufferHeight = 1080;
            Globals.Graphics.ApplyChanges();
            dropdown.IsOpen = false;
        }, 0);

        rootPanel.AddChild(dropdown);

        // Add other options UI elements here
        rootPanel.AddChild(new Button(Anchor.AutoLeft, new Vector2(0.5F, 0.6F), "Fullscreen")
        {
            CanBePressed = false,
            CanBeSelected = false,
        });


        var fullscreenCheck = new Checkbox(Anchor.AutoInline, new Vector2(0.5F, 0.6F), "")
        {
            Checked = Globals.Fullscreen,
        };
        fullscreenCheck.OnCheckStateChange += (element, isChecked) => {
            System.Diagnostics.Debug.WriteLine("Fullscreen Clicked");
            Globals.Fullscreen = isChecked;
            Globals.Graphics.IsFullScreen = isChecked;
            Globals.Graphics.ApplyChanges();
        };

        rootPanel.AddChild(fullscreenCheck);

        // BACK Button: Switch game state to MainMenu
        var backButton = new Button(Anchor.AutoLeft, new Vector2(0.3F, 0.6F), "Save");
        backButton.PositionOffset = new Vector2(0, 50);
        backButton.OnPressed += _ =>
        {
            System.Diagnostics.Debug.WriteLine("Back Clicked");
            ClearAndSwitch(MenuState.MainMenu);
        };
        rootPanel.AddChild(backButton);
    }



    public void Show() => rootPanel.IsHidden = false;
    public void Hide() => rootPanel.IsHidden = true;
}
