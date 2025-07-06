using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Maths;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using SWEN_Game._Managers;
using SWEN_Game._Utils;
using SWEN_Game._Interfaces;
using SWEN_Game._Sound;

namespace SWEN_Game._UI
{
    public enum MenuState
    {
        MainMenu,
        Options,
        Paused,
    }

    public class MainMenuUI
    {
        private readonly UiSystem ui;
        private readonly IGameStateManager gameStateManager; // Add this field to store the GameStateManager instance
        private readonly Game game; // Add this field to store the Game instance
        private Panel rootPanel;
        private MenuState currentMenuState;

        public MainMenuUI(UiSystem uiSystem, IGameStateManager gameStateManager, Game game)
        {
            this.ui = uiSystem;
            this.gameStateManager = gameStateManager;
            this.game = game;

            // Create the root panel that contains all menu elements
            rootPanel = new Panel(Anchor.Center, new Vector2(700, 500), Vector2.Zero);
            rootPanel.DrawColor = Color.White;
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
                    ShowOptionsMenu("OPTIONS", "Save Settings", () => ClearAndSwitch(MenuState.MainMenu));
                    break;
                case MenuState.Paused:
                    ShowOptionsMenu("PAUSED", "Resume Game", () =>
                    {
                        gameStateManager.ChangeGameState(GameState.Playing); // Use the instance to call the method
                        Hide();
                    });
                    break;
            }
        }

        public void Show() => rootPanel.IsHidden = false;
        public void Hide() => rootPanel.IsHidden = true;

        private void ShowMainMenu()
        {
            var titleMenu = new Paragraph(Anchor.TopCenter, 1, "Island Survivor", true)
            {
                TextColor = Color.Black,
            };
            rootPanel.AddChild(titleMenu);
            rootPanel.AddChild(new VerticalSpace(20));

            var playButton = new Button(Anchor.AutoCenter, new Vector2(0.5F, 0.2F), "Play");

            playButton.PositionOffset = new Vector2(3, 0);

            playButton.OnMouseEnter += (e) => SFXManager.Instance.Play("uiSelect");
            playButton.OnPressed += _ =>
            {
                gameStateManager.ResetGame(); // Use the instance to call the method
                SFXManager.Instance.SetVolume(Globals.SoundVolume);
                SongManager.Instance.SetVolume(Globals.MusicVolume);
                Hide();
                SFXManager.Instance.Play("uiConfirm");
            };

            rootPanel.AddChild(playButton);
            rootPanel.AddChild(new VerticalSpace(20));

            var optionsButton = new Button(Anchor.AutoCenter, new Vector2(0.5F, 0.2F), "Options");
            optionsButton.PositionOffset = new Vector2(3, 0);
            optionsButton.OnMouseEnter += (e) => SFXManager.Instance.Play("uiSelect");
            optionsButton.OnPressed += _ =>
            {
                ClearAndSwitch(MenuState.Options);
                SFXManager.Instance.Play("uiConfirm");
            };

            rootPanel.AddChild(optionsButton);

            rootPanel.AddChild(new VerticalSpace(20));

            var exitButton = new Button(Anchor.AutoCenter, new Vector2(0.5F, 0.2F), "Exit");
            exitButton.PositionOffset = new Vector2(3, 0);
            exitButton.OnMouseEnter += (e) => SFXManager.Instance.Play("uiSelect");
            exitButton.OnPressed += _ =>
            {
                SFXManager.Instance.Play("uiConfirm");
                ui.Game.Exit();
            };

            rootPanel.AddChild(exitButton);
        }

        private void ShowOptionsMenu(string menuName, string buttonName, Action onButtonPressed)
        {
            Checkbox fullscreenCheckbox = null;

            var titleMenu = new Paragraph(Anchor.TopCenter, 1, menuName, true);
            rootPanel.AddChild(titleMenu);
            rootPanel.AddChild(new VerticalSpace(60));

            var horizintalGroup = new Group(Anchor.TopCenter, new Vector2(660, 340));
            rootPanel.AddChild(horizintalGroup);

            var leftPanel = new Panel(Anchor.TopLeft, new Vector2(320, 330), Vector2.Zero)
            {
                Padding = Padding.Empty,
                PositionOffset = new Vector2(0, 40),
            };
            horizintalGroup.AddChild(leftPanel);

            fullscreenCheckbox = AddCheckboxButton(leftPanel, "Fullscreen", Globals.Fullscreen, isChecked =>
            {
                if (isChecked)
                {
                    Globals.Borderless = false;
                    game.Window.IsBorderless = false;
                }

                Globals.Fullscreen = isChecked;
                Globals.Graphics.IsFullScreen = isChecked;
                Globals.Graphics.ApplyChanges();
            });
            leftPanel.AddChild(new VerticalSpace(20));

            ShowResolutionSelector(leftPanel, fullscreenCheckbox);

            var saveButton = new Button(Anchor.BottomLeft, new Vector2(280, 80), buttonName);
            saveButton.PositionOffset = new Vector2(10, -100);
            saveButton.OnMouseEnter += (e) => SFXManager.Instance.Play("uiSelect");
            saveButton.OnPressed += _ =>
            {
                onButtonPressed?.Invoke();
                System.Diagnostics.Debug.WriteLine("Save Settings Clicked");
                System.Diagnostics.Debug.WriteLine($"Fullscreen: {Globals.Fullscreen}");
                System.Diagnostics.Debug.WriteLine($"Window Size: {Globals.WindowSize}");
                System.Diagnostics.Debug.WriteLine($"Sound Volume: {Globals.SoundVolume}");
                System.Diagnostics.Debug.WriteLine($"Music Volume: {Globals.MusicVolume}");
                SFXManager.Instance.Play("uiConfirm");
            };

            leftPanel.AddChild(saveButton);

            var rightPanel = new Panel(Anchor.TopRight, new Vector2(320, 330), Vector2.Zero)
            {
                Padding = Padding.Empty,
                PositionOffset = new Vector2(0, 40),
            };
            horizintalGroup.AddChild(rightPanel);

            rightPanel.AddChild(new VerticalSpace(10));

            AddVolumeSlider(rightPanel, "SFX Volume", Globals.SoundVolume, newValue =>
             {
                 Globals.SoundVolume = newValue;
                 SFXManager.Instance.SetVolume(newValue);
             });

            rightPanel.AddChild(new VerticalSpace(20));
            AddVolumeSlider(rightPanel, "Music Volume", Globals.MusicVolume, newValue =>
             {
                 Globals.MusicVolume = newValue;
                 SongManager.Instance.SetVolume(newValue);
             });
            // rightPanel.AddChild(new VerticalSpace(80));

            var exitButtonLabel = currentMenuState == MenuState.Paused ? "Return to Main Menu" : "Exit Game";

            var exitButton = new Button(Anchor.BottomRight, new Vector2(280, 80), exitButtonLabel)
            {
                PositionOffset = new Vector2(10, -100),
            };

            exitButton.OnMouseEnter += (e) => SFXManager.Instance.Play("uiSelect");
            exitButton.OnPressed += _ =>
            {
                if (currentMenuState == MenuState.Paused)
                {
                    gameStateManager.ChangeGameState(GameState.MainMenu);
                    SongManager.Instance.Stop();
                    SongManager.Instance.Play("Main");
                    ClearAndSwitch(MenuState.MainMenu);
                }
                else
                {
                    ui.Game.Exit();
                }

                SFXManager.Instance.Play("uiConfirm");
            };

            rightPanel.AddChild(exitButton);
        }

        private Checkbox AddCheckboxButton(Panel parentPanel, string labelText, bool isCheckedInitial, Action<bool> onToggle)
        {
            var button = new Button(Anchor.AutoLeft, new Vector2(280, 80), string.Empty);
            button.PositionOffset = new Vector2(10, 10);
            button.AddChild(new Paragraph(Anchor.Center, 1, labelText)
            {
                PositionOffset = new Vector2(10, 0),
            });

            button.OnMouseEnter += (e) =>
            {
                SFXManager.Instance.Play("uiSelect");
            };

            var checkbox = new Checkbox(Anchor.CenterRight, new Vector2(40, 40), " ")
            {
                Checked = isCheckedInitial,
                CanBeSelected = false,
                PositionOffset = new Vector2(10, 0),
            };

            checkbox.OnCheckStateChange += (elem, newState) =>
            {
                SFXManager.Instance.Play("uiConfirm");
                onToggle?.Invoke(newState);
            };

            button.AddChild(checkbox);
            parentPanel.AddChild(button);
            return checkbox;
        }

        private void ShowResolutionSelector(Panel parent, Checkbox fullscreenCheckbox)
        {
            var dropdown = new Dropdown(Anchor.AutoLeft, new Vector2(280, 80), "Resolution")
            {
                PositionOffset = new Vector2(10, 0),
                IsOpen = false,
            };

            dropdown.OnMouseEnter += elem =>
            {
                SFXManager.Instance.Play("uiSelect");
            };

            dropdown.OnPressed += elem =>
            {
                SFXManager.Instance.Play("uiConfirm");
            };

            var resolutions = new[]
            {
                new { Label = "1280x720", Width = 1280, Height = 720 },
                new { Label = "1600x900", Width = 1600, Height = 900 },
                new { Label = "1920x1080", Width = 1920, Height = 1080 },
                new { Label = "2560x1440", Width = 2560, Height = 1440 },
                new { Label = "2560x1080 (Ultrawide)", Width = 2560, Height = 1080 },
                new { Label = "3440x1440 (Ultrawide)", Width = 3440, Height = 1440 },
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
                        game.GraphicsDevice.Viewport = new Viewport(0, 0, res.Width, res.Height);
                        dropdown.IsOpen = false;

                        fullscreenCheckbox.IsDisabled = false;
                    }, 0);
            }

            var supportedFullscreenResolutions = new[]
            {
                new Point(1920, 1080),
                new Point(2560, 1440),
                new Point(2560, 1080),  // Ultrawide
                new Point(3440, 1440),  // Ultrawide
            };

            // Check if current size is a supported fullscreen size
            bool isSupportedFullscreen = supportedFullscreenResolutions.Any(p =>
                p.X == Globals.WindowSize.X && p.Y == Globals.WindowSize.Y);

            fullscreenCheckbox.IsDisabled = !isSupportedFullscreen;

            if (!isSupportedFullscreen)
            {
                fullscreenCheckbox.Checked = false;
                Globals.Fullscreen = false;
                Globals.Graphics.IsFullScreen = false;
                Globals.Graphics.ApplyChanges();
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
                Background = new NinePatch(ui.Game.Content.Load<Texture2D>("Menu/slider_progress_hover"), padding: 6),
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
            // parentPanel.AddChild(new VerticalSpace(20));
        }
    }
}