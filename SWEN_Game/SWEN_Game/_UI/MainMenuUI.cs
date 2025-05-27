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
        rootPanel = new Panel(Anchor.Center, new Vector2(700, 400), Vector2.Zero);
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
                ShowOptionsMenu("OPTIONS", "Save Settings");
                break;
            case MenuState.Paused:
                ShowOptionsMenu("PAUSED", "Resume Game");
                break;

        }
    }

    public void ShowOptionsOnly()
    {
        Show();
        ClearAndSwitch(MenuState.Paused);
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

    private void ShowOptionsMenu(String MenuName, String ButtonName)
    {
        Checkbox fullscreenCheckbox;

        var titleMenu = new Paragraph(Anchor.TopCenter, 1, MenuName, true);
        rootPanel.AddChild(titleMenu);
        rootPanel.AddChild(new VerticalSpace(40));

        var horizintalGroup = new Group(Anchor.TopCenter, new Vector2(660, 340));
        rootPanel.AddChild(horizintalGroup);

        var leftPanel = new Panel(Anchor.TopLeft, new Vector2(320, 330), Vector2.Zero)
        {
            Padding = Padding.Empty,
            PositionOffset = new Vector2(0, 40)
        };
        horizintalGroup.AddChild(leftPanel);

        fullscreenCheckbox = AddCheckboxButton(leftPanel, "Fullscreen", Globals.Fullscreen, isChecked =>
        {
            Globals.Fullscreen = isChecked;
            Globals.Graphics.IsFullScreen = isChecked;
            Globals.Graphics.ApplyChanges();
        });
        leftPanel.AddChild(new VerticalSpace(20));

        ShowResolutionSelector(leftPanel, fullscreenCheckbox);
        leftPanel.AddChild(new VerticalSpace(20));
        var saveButton = new Button(Anchor.AutoLeft, new Vector2(280, 80), ButtonName);
        saveButton.PositionOffset = new Vector2(10, 10);
        saveButton.OnPressed += _ =>
        {
            System.Diagnostics.Debug.WriteLine("Save Settings Clicked");
            System.Diagnostics.Debug.WriteLine($"Fullscreen: {Globals.Fullscreen}");
            System.Diagnostics.Debug.WriteLine($"Window Size: {Globals.WindowSize}");
            System.Diagnostics.Debug.WriteLine($"Sound Volume: {Globals.SoundVolume}");
            System.Diagnostics.Debug.WriteLine($"Music Volume: {Globals.MusicVolume}");
            ClearAndSwitch(MenuState.MainMenu);
        };

        leftPanel.AddChild(saveButton);

        var rightPanel = new Panel(Anchor.TopRight, new Vector2(320, 330), Vector2.Zero)
        {
            Padding = Padding.Empty,
            PositionOffset = new Vector2(0, 40),
        };
        horizintalGroup.AddChild(rightPanel);

        AddVolumeSlider(rightPanel, "SFX Volume", Globals.SoundVolume, newValue =>
         {
             Globals.SoundVolume = newValue;
         });

        rightPanel.AddChild(new VerticalSpace(10));
        AddVolumeSlider(rightPanel, "Music Volume", Globals.MusicVolume, newValue =>
         {
             Globals.MusicVolume = newValue;

         });
    }

    private Checkbox AddCheckboxButton(Panel parentPanel, string labelText, bool isCheckedInitial, Action<bool> onToggle)
    {
        var button = new Button(Anchor.AutoLeft, new Vector2(280, 80), "");
        button.PositionOffset = new Vector2(10, 10);
        button.AddChild(new Paragraph(Anchor.Center, 1, labelText)
        {
            PositionOffset = new Vector2(10, 0),
        });

        var checkbox = new Checkbox(Anchor.CenterRight, new Vector2(60, 60), " ")
        {
            Checked = isCheckedInitial,
            CanBeSelected = false,
            PositionOffset = new Vector2(10, 0),
        };

        checkbox.OnCheckStateChange += (elem, newState) =>
        {
            onToggle?.Invoke(newState);
        };

        button.AddChild(checkbox);
        parentPanel.AddChild(button);
        return checkbox;
}

    private void ShowResolutionSelector(Panel parent, Checkbox fullscreenCheckbox)
    {
        var dropdown = new Dropdown(Anchor.AutoLeft, new Vector2(280, 80), "Window Size")
        {
            PositionOffset = new Vector2(10, 0),
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

                    if (res.Width == 1920 && res.Height == 1080)
                    {
                        fullscreenCheckbox.IsDisabled = false;

                    }
                    else
                    {
                        fullscreenCheckbox.IsDisabled = true;
                        fullscreenCheckbox.Checked = false;
                        Globals.Fullscreen = false;
                        Globals.Graphics.IsFullScreen = false;
                        Globals.Graphics.ApplyChanges();
                    }
                }, 0);
        }

        parent.AddChild(dropdown);
    }

    private void AddVolumeSlider(Panel parentPanel, string labelText, float initialVolume, Action<float> onChanged)
    {
       parentPanel.AddChild(new Paragraph(Anchor.AutoLeft, 1, labelText)
        {
            PositionOffset = new Vector2(10, 10),
        });
        parentPanel.AddChild(new VerticalSpace(10));

        var slider = new Slider(Anchor.AutoLeft, new Vector2(280, 40), 40, 1)
        {
            Background = new NinePatch(ui.Game.Content.Load<Texture2D>("Menu/slider_progress"), padding: 6),
            ScrollerTexture = new NinePatch(ui.Game.Content.Load<Texture2D>("Menu/v_slidder_grabber"), padding: 6),
            StepPerScroll = 0.1f,
            PositionOffset = new Vector2(10, 10),
            CurrentValue = initialVolume,
        };
        slider.OnValueChanged += (elem, newValue) =>
        {
            onChanged?.Invoke(newValue);
        };

        parentPanel.AddChild(slider);
        parentPanel.AddChild(new VerticalSpace(20));

    }
}