using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Formatting;
using MLEM.Maths;
using MLEM.Ui;
using MLEM.Ui.Elements;
using SWEN_Game._Items;
using SWEN_Game._Managers;
using SWEN_Game._Utils;

namespace SWEN_Game._UI
{
    public class InGameUI
    {
        private readonly UiSystem uiSystem;
        private Panel healthPanel;
        private Paragraph healthParagraph;

        private Panel ammoPanel;
        private Paragraph ammoParagraph;
        private Image weaponIconImage;

        private Panel timePanel;
        private Paragraph timeParagraph;

        private Panel xpPanel;
        private Paragraph xpText;
        private Texture2D pixel;

        private Paragraph currentLevel;

        public InGameUI(UiSystem uiSystem)
        {
            pixel = Globals.Content.Load<Texture2D>("debug_rect");
            this.uiSystem = uiSystem;

            // Health panel top-left
            healthPanel = new Panel(Anchor.TopLeft, new Vector2(300, 100), new Vector2(20, 20));
            healthPanel.DrawColor = Color.Black * 0.5f;
            uiSystem.Add("HealthPanel", healthPanel);
            CreateHealthDisplay();

            // Ammo panel bottom-right
            ammoPanel = new Panel(Anchor.BottomRight, new Vector2(300, 100), new Vector2(20, 20));
            ammoPanel.DrawColor = Color.Black * 0.5f;
            uiSystem.Add("AmmoPanel", ammoPanel);
            CreateAmmoDisplay();

            timePanel = new Panel(Anchor.TopRight, new Vector2(300, 100), new Vector2(20, 20));
            timePanel.DrawColor = Color.Black * 0.5f;
            uiSystem.Add("TimePanel", timePanel);
            CreateTimeDisplay();

            xpPanel = new Panel(Anchor.BottomCenter, new Vector2(500, 40), new Vector2(0, 20));
            xpPanel.DrawColor = Color.Black * 0.6f;
            xpPanel.Padding = new Padding(0);
            uiSystem.Add("XpPanel", xpPanel);
            CreateXPDisplay();

            Hide();
        }

        public void Update(GameTime gameTime, GameState currentState)
        {
            if (currentState == GameState.Playing)
            {
                Show();
                UpdateHealthText();
                UpdateAmmoText();
                UpdateTimeText();
                UpdateXpBar();
            }
            else
            {
                Hide();
            }
        }

        public void Show()
        {
            healthPanel.IsHidden = false;
            ammoPanel.IsHidden = false;
            timePanel.IsHidden = false;
            xpPanel.IsHidden = false;
        }

        public void Hide()
        {
            healthPanel.IsHidden = true;
            ammoPanel.IsHidden = true;
            timePanel.IsHidden = true;
            xpPanel.IsHidden = true;
        }

        private void CreateHealthDisplay()
        {
            healthParagraph = new Paragraph(Anchor.Center, 1, "Health: -- / --")
            {
                TextColor = Color.White,
            };
            healthPanel.AddChild(healthParagraph);
        }

        private void CreateAmmoDisplay()
        {
            ammoParagraph = new Paragraph(Anchor.Center, 1, "Ammo: -- / --")
            {
                TextColor = Color.Yellow,
            };
            ammoPanel.AddChild(ammoParagraph);

            weaponIconImage = new Image(Anchor.BottomCenter, new Vector2(64, 64), (MLEM.Textures.TextureRegion)null)
            {
                PositionOffset = new Vector2(120, 15),
            };
            ammoPanel.AddChild(weaponIconImage);
        }

        private void CreateTimeDisplay()
        {
            timeParagraph = new Paragraph(Anchor.Center, 1, "Time: 0.0s")
            {
                TextColor = Color.Cyan,
            };
            timePanel.AddChild(timeParagraph);
        }

        private void CreateXPDisplay()
        {
            // Level label on the left
            currentLevel = new Paragraph(Anchor.CenterLeft, 0.5F, "Lv 1")
            {
                PositionOffset = new Vector2(5, 0), // some padding from the left
                TextColor = Color.Gold,
                Alignment = TextAlignment.Left,
            };
            xpPanel.AddChild(currentLevel);

            // XP text just to the right of the level label
            xpText = new Paragraph(Anchor.CenterLeft, 1F, "XP: 0 / 0")
            {
                PositionOffset = new Vector2(150, 0),  // move it right to avoid overlapping the level
                TextColor = Color.White,
                Alignment = TextAlignment.Left,
            };
            xpPanel.AddChild(xpText);
        }

        private void UpdateHealthText()
        {
            if (PlayerGameData.Instance != null)
            {
                var playerData = PlayerGameData.Instance;
                healthParagraph.Text = $"Health: {playerData.GetCurrentHealth()} / {playerData.GetMaxHealth()}";
            }
            else
            {
                healthParagraph.Text = "Health: -- / --";
            }
        }

        private void UpdateAmmoText()
        {
            if (PlayerGameData.Instance != null && PlayerGameData.Instance.CurrentWeapon != null)
            {
                weaponIconImage.Texture = new MLEM.Textures.TextureRegion(PlayerGameData.Instance.BaseWeapon.IconSprite);
                ammoParagraph.Text = $"Ammo: {PlayerGameData.Instance.CurrentWeapon.CurrentAmmo} / {PlayerGameData.Instance.CurrentWeapon.MagazineSize}";
            }
            else
            {
                weaponIconImage.Texture = null;
                ammoParagraph.Text = "Ammo: -- / --";
            }
        }

        private void UpdateTimeText()
        {
            int totalSeconds = (int)Globals.TotalGameTime;
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timeParagraph.Text = $"Time: {minutes:D2}:{seconds:D2}";
        }

        private void UpdateXpBar()
        {
            var data = PlayerGameData.Instance;
            float currentXp = data.GetXP();
            float requiredXp = data.GetRequiredXPForLevel();

            float fillRatio = MathHelper.Clamp(currentXp / requiredXp, 0, 1);
            float panelWidth = xpPanel.Size.X;

            currentLevel.Text = $"Lvl {data.GetLevel()}";
            xpText.Text = $"XP: {MathF.Round(currentXp)} / {MathF.Round(requiredXp)}";
        }
    }
}