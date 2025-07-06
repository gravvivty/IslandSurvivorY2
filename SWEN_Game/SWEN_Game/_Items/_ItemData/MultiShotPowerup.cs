using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class MultiShotPowerup : Powerup
    {
        // ID 2

        public MultiShotPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            this.ApplyPowerupLevel();
            this.itemID = 2; // Unique ID
            SetLevelValues();
        }

        private void ApplyPowerupLevel()
        {
            PlayerGameData.Instance.ReverseShotLevel = this.Level;
        }

        private void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.attackSpeedFlat += 0.05f;
                    break;
                case 2:
                    this.attackSpeedFlat += 0.1f;
                    break;
                case 3:
                    this.attackSpeedFlat += 0.2f;
                    break;
                default:
                    break;
            }
        }
    }
}
