using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Maths;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;
using MLEM.Textures;

namespace SWEN_Game;

public enum MenuState
{
    MainMenu,
    Options,
    Paused,
}

public class MainMenuUI
{
    private readonly UiSystem ui;
    private readonly GameStateManager gameStateManager; // Add this field to store the GameStateManager instance
    private readonly Game game; // Add this field to store the Game instance
    private Panel rootPanel;
    private MenuState currentMenuState;

    public MainMenuUI(UiSystem uiSystem, GameStateManager gameStateManager, Game game)
    {
        this.ui = uiSystem;
        this.gameStateManager = gameStateManager;
        this.game = game;

        // Create the root panel that contains all menu elements
        rootPanel = new Panel(Anchor.Center, new Vector2(0.4F, 0.3F), Vector2.Zero);
        rootPanel.DrawColor = Color.White * 0.7f;
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

    public void ShowOptionsOnly()
    {
        Show();
        ClearAndSwitch(MenuState.Options);
    }

    public void Show() => rootPanel.IsHidden = false;
    public void Hide() => rootPanel.IsHidden = true;

    private void ShowMainMenu()
    {
        var titleMenu = new Paragraph(Anchor.TopCenter, 1, "MAIN MENU", true);
        rootPanel.AddChild(titleMenu);
        rootPanel.AddChild(new VerticalSpace(10));

        var playButton = new Button(Anchor.AutoLeft, new Vector2(0.5F, 0.2F), "Play");
        playButton.PositionOffset = new Vector2(3, 0);

        playButton.OnPressed += _ =>
        {
            gameStateManager.ChangeGameState(GameState.Playing); // Use the instance to call the method
            Hide();
        };

        rootPanel.AddChild(playButton);
        rootPanel.AddChild(new VerticalSpace(10));

        var optionsButton = new Button(Anchor.AutoLeft, new Vector2(0.5F, 0.2F), "Options");
        optionsButton.PositionOffset = new Vector2(3, 0);
        optionsButton.OnPressed += _ => ClearAndSwitch(MenuState.Options);

        rootPanel.AddChild(optionsButton);

        rootPanel.AddChild(new VerticalSpace(10));

        var exitButton = new Button(Anchor.AutoLeft, new Vector2(0.5F, 0.2F), "Exit");
        exitButton.PositionOffset = new Vector2(3, 0);
        exitButton.OnPressed += _ =>
        {
            System.Diagnostics.Debug.WriteLine("Exit Clicked");
            ui.Game.Exit();
        };

        rootPanel.AddChild(exitButton);
    }

    private void ShowOptionsMenu()
    {
        rootPanel.RemoveChildren();

        var titleMenu = new Paragraph(Anchor.TopCenter, 1, "OPTIONS", true);
        rootPanel.AddChild(titleMenu);
        rootPanel.AddChild(new VerticalSpace(10));

        AddCheckboxButton(rootPanel, "Fullscreen", Globals.Fullscreen, isChecked =>
        {
            Globals.Fullscreen = isChecked;
            Globals.Graphics.IsFullScreen = isChecked;
            Globals.Graphics.ApplyChanges();
        });
        rootPanel.AddChild(new VerticalSpace(10));

        ShowResolutionSelector(rootPanel);
        rootPanel.AddChild(new VerticalSpace(10));

        /*var dropdown = new Dropdown(Anchor.AutoLeft, new Vector2(0.5F, 0.6F), "Window Size");
        rootPanel.AddChild(dropdown);*/
        AddVolumeSlider(rootPanel, "SFX Volume", Globals.SoundVolume, newValue =>
         {
             Globals.SoundVolume = newValue;
             // Update SFX volume in the game
         });

        var saveButton = new Button(Anchor.AutoLeft, new Vector2(0.5F, 0.2F), "Save  Settings");
        saveButton.PositionOffset = new Vector2(3, 0);
        saveButton.OnPressed += _ =>
        {
            ClearAndSwitch(MenuState.MainMenu);
        };

        rootPanel.AddChild(saveButton);
    }

    private void AddCheckboxButton(Panel parentPanel, string labelText, bool isCheckedInitial, Action<bool> onToggle)
    {
        var button = new Button(Anchor.AutoLeft, new Vector2(0.5F, 0.2F), "");
        button.PositionOffset = new Vector2(3, 0);
        button.AddChild(new Paragraph(Anchor.Center, 1, labelText)
        {
            PositionOffset = new Vector2(10, 0),
        });

        var checkbox = new Checkbox(Anchor.CenterRight, new Vector2(0.2F, 0.8F), " ")
        {
            Checked = isCheckedInitial,
            CanBeSelected = false,
        };

        checkbox.OnCheckStateChange += (elem, newState) =>
        {
            onToggle?.Invoke(newState);
        };

        button.AddChild(checkbox);
        parentPanel.AddChild(button);
    }

    private void ShowResolutionSelector(Panel parent)
    {
        var dropdown = new Dropdown(Anchor.AutoLeft, new Vector2(0.5F, 0.2F), "Window Size")
        {
            PositionOffset = new Vector2(3, 0),
            IsOpen = false,
        };

        var resolutions = new[]
        {
            new { Label = "1280x720", Width = 1280, Height = 720 },
            new { Label = "1600x900", Width = 1600, Height = 900 },
            new { Label = "1920x1080", Width = 1920, Height = 1080 },
        };

        foreach (var res in resolutions)
        {
            dropdown.AddElement(
                res.Label,
                element =>
                {
                    Globals.WindowSize = new Point(res.Width, res.Height);
                    Globals.Graphics.PreferredBackBufferWidth = res.Width;
                    Globals.Graphics.PreferredBackBufferHeight = res.Height;
                    Globals.Graphics.ApplyChanges();
                    Globals.UiSystem.Viewport = new Rectangle(0, 0, res.Width, res.Height);
                    dropdown.IsOpen = false;
                }, 0);
        }

        parent.AddChild(dropdown);
    }

    private void AddVolumeSlider(Panel parentPanel, string labelText, float initialVolume, Action<float> onChanged)
    {
        var container = new Group(Anchor.AutoLeft, new Vector2(0.5F, 0.2F));
        container.AddChild(new Paragraph(Anchor.Center, 1, labelText)
        {
            PositionOffset = new Vector2(3, 0),
        });

        var slider = new Slider(Anchor.CenterRight, new Vector2(0.3F, 0.5F), 0, 1)
        {
            Background = new NinePatch(ui.Game.Content.Load<Texture2D>("Menu/slider_progress"), padding: 6),
            Grabber = new TextureRegion(ui.Game.Content.Load<Texture2D>("Menu/v_slidder_grabber"), new Rectangle(0, 0, 16, 16)),
            CurrentValue = initialVolume,
        };
        slider.OnValueChanged += (elem, newValue) =>
        {
            onChanged?.Invoke(newValue);
        };
        container.AddChild(slider);
        parentPanel.AddChild(container);
    }
}