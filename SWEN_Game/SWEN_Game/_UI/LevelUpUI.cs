using Microsoft.Xna.Framework;
using MLEM.Maths;
using MLEM.Ui;
using MLEM.Ui.Elements;
using SWEN_Game._Items;
using SWEN_Game._Managers;
using SWEN_Game._Shooting;
using SWEN_Game._Interfaces;
using SWEN_Game._Sound;

namespace SWEN_Game._UI
{
    public class LevelUpUI
    {
        private readonly UiSystem _uiSystem;
        private readonly IGameStateManager _gameStateManager;
        private PowerupManager _powerupManager;
        private WeaponManager _weaponManager;

        private Panel _levelUpPanel;
        private List<Button> _optionButtons = new();
        private Dictionary<Button, int> _buttonIdMap = new();

        private Panel _statsPanel;
        private Paragraph _statsText;

        private Button _weaponOptionButton;
        private string _randomWeaponKey;

        public LevelUpUI(UiSystem uiSystem, IGameStateManager gameStateManager)
        {
            _uiSystem = uiSystem;
            _gameStateManager = gameStateManager;

            _levelUpPanel = new Panel(Anchor.Center, new Vector2(1200, 420))
            {
                DrawColor = Color.Black * 0.5f,
                IsHidden = true,
            };
            _uiSystem.Add("LevelUpPanel", _levelUpPanel);

            var header = new Paragraph(Anchor.TopCenter, 0.9F, "Level Up! Choose a Powerup:", true)
            {
                PositionOffset = new Vector2(0, 15),
                TextColor = Color.Gold,
            };
            _levelUpPanel.AddChild(header);

            for (int i = 0; i < 3; i++)
            {
                var btn = new Button(Anchor.TopCenter, new Vector2(0.95F, 60), string.Empty)
                {
                    PositionOffset = new Vector2(0, 80 + i * 70),
                };

                int index = i;
                btn.OnMouseEnter += (e) => SFXManager.Instance.Play("uiSelect");
                btn.OnPressed += (e) =>
                {
                    SFXManager.Instance.Play("uiConfirm");
                    OnPowerupSelected(index);
                };
                _optionButtons.Add(btn);
                _levelUpPanel.AddChild(btn);
            }

            // Weapon Option
            _weaponOptionButton = new Button(Anchor.BottomCenter, new Vector2(0.95F, 60), string.Empty)
            {
                PositionOffset = new Vector2(0, 20),
            };
            _weaponOptionButton.OnMouseEnter += (e) => SFXManager.Instance.Play("uiSelect");
            _weaponOptionButton.OnPressed += (e) =>
            {
                OnWeaponSelected();
                SFXManager.Instance.Play("uiConfirm");
            };
            _levelUpPanel.AddChild(_weaponOptionButton);

            // Add stats panel to the left
            _statsPanel = new Panel(Anchor.CenterLeft, new Vector2(350, 500), new Vector2(0, 20))
            {
                DrawColor = Color.Black * 0.1f,
                Padding = new Padding(10),
                IsHidden = true,  // start hidden
            };
            _statsText = new Paragraph(Anchor.CenterLeft, 1, string.Empty)
            {
                TextColor = Color.White,
            };
            _statsPanel.AddChild(_statsText);
            _uiSystem.Add("StatsPanel", _statsPanel);
        }

        public void Hide()
        {
            _levelUpPanel.IsHidden = true;
            _statsPanel.IsHidden = true;
        }

        /// <summary>
        /// Display 3 random, valid powerups and show the panel.
        /// </summary>
        public void Show()
        {
            // All possible powerup IDs
            var allIds = Enumerable.Range(1, 14);

            // Filter out maxed (level 3) powerups
            var availableIds = allIds
                .Where(id =>
                    !PlayerGameData.Instance.Powerups.TryGetValue(id, out var powerup) ||
                    powerup.Level < 3)
                .ToList();

            // If no powerups are available to choose from, just skip
            if (availableIds.Count == 0)
            {
                Hide();
                _gameStateManager.ChangeGameState(GameState.Playing);
                return;
            }

            var rand = new Random();
            var selectedIds = availableIds.OrderBy(x => rand.Next()).Take(3).ToArray();

            _buttonIdMap.Clear();

            for (int i = 0; i < 3; i++)
            {
                if (i >= selectedIds.Length)
                {
                    _optionButtons[i].IsHidden = true;
                    continue;
                }

                int id = selectedIds[i];
                var btn = _optionButtons[i];
                btn.IsHidden = false;

                ((Paragraph)_optionButtons[i].Children[0]).Text = GetPowerupName(id);
                _buttonIdMap[btn] = id;
            }

            _levelUpPanel.IsHidden = false;
            _statsPanel.IsHidden = false;
            UpdateStatsDisplay();

            // Pick a random weapon (excluding current weapon)
            var allWeaponKeys = _weaponManager.GetWeaponKeys().Where(k => k != PlayerGameData.Instance.BaseWeapon.Name).ToList();
            _randomWeaponKey = allWeaponKeys[rand.Next(allWeaponKeys.Count)];

            // Format the button text
            ((Paragraph)_weaponOptionButton.Children[0]).Text = $"Switch to: {_randomWeaponKey.Replace('_', ' ')}";
            _weaponOptionButton.IsHidden = false;
        }

        public void SetPowerupManager(PowerupManager manager)
        {
            _powerupManager = manager;
        }

        public void SetWeaponManager(WeaponManager weaponManager)
        {
            _weaponManager = weaponManager;
        }

        private void OnPowerupSelected(int buttonIndex)
        {
            var btn = _optionButtons[buttonIndex];
            if (_buttonIdMap.TryGetValue(btn, out int selectedId))
            {
                _powerupManager.AddItem(selectedId);
                PlayerGameData.Instance.UpdateWeaponGameData();
            }

            Hide();
            _gameStateManager.ChangeGameState(GameState.Playing);
        }

        private string GetPowerupName(int id)
        {
            int currentLevel = PlayerGameData.Instance.Powerups.TryGetValue(id, out var powerup) ? powerup.Level : 0;
            int newLevel = currentLevel + 1;

            string baseName = id switch
            {
                1 => "Gunpowder - Increases Bullet Damage (flat)",
                2 => "Multi Shot - Shoot From More Directions",
                3 => "Piercer - More Bullet Pierce",
                4 => "Adrenaline - Shoot Faster (flat)",
                5 => "Rocket Speed - Bullets Travel faster",
                6 => "Rancid Energy - Shoot Faster (mult)",
                7 => "Shadow Bullets - Spawn Smaller Bullets",
                8 => "Quick Hands - Increases Reload Speed (flat)",
                9 => "Spicy Noodles - Increases Speed",
                10 => "Deadeye - Increases Crit Chance",
                11 => "Heavy Mags - Increases Mag Size",
                12 => "Extreme Teapowder - Increases Bullet Damage (mult)",
                13 => "Frozen Tears - Increases Slow Chance",
                14 => "Speed Cola - Increases Reload Speed (mult)",
                _ => "Unknown"
            };

            return $"{baseName} (Lv. {currentLevel} -> Lv. {newLevel})";
        }

        private void UpdateStatsDisplay()
        {
            var weapon = PlayerGameData.Instance.CurrentWeapon;
            float speed = PlayerGameData.Instance.Speed + PlayerGameData.Instance.SpeedBonus;

            _statsText.Text = $"-- Stats --\n" +
                              $"Speed: {speed:F1}\n" +
                              $"Bullet Damage: {weapon.BulletDamage:F1}\n" +
                              $"Bullet Speed: {weapon.ShotSpeed:F1}\n" +
                              $"Attack Speed: {weapon.AttackSpeed:F2}\n" +
                              $"Reload Time: {weapon.ReloadTime:F2}\n" +
                              $"Magazine Size: {weapon.MagazineSize}\n" +
                              $"Pierce: {weapon.Pierce + PlayerGameData.Instance.BulletPierceBonus}\n" +
                              $"Crit: {PlayerGameData.Instance.CritChance * 100:F0}%\n" +
                              $"Slow: {PlayerGameData.Instance.SlowChance * 100:F0}%\n";
        }

        private void OnWeaponSelected()
        {
            var newWeapon = _weaponManager.GetWeapon(_randomWeaponKey);
            PlayerGameData.Instance.BaseWeapon = newWeapon;
            PlayerGameData.Instance.UpdateWeaponGameData();
            PlayerGameData.Instance.CurrentWeapon.CurrentAmmo = newWeapon.MagazineSize;

            Hide();
            _gameStateManager.ChangeGameState(GameState.Playing);
        }
    }
}