using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class SpeedColaPowerup : Powerup
    {
        // ID 14

        public SpeedColaPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 14;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.reloadSpeedMultBonus -= 0.1f;
                    break;
                case 2:
                    this.reloadSpeedMultBonus -= 0.2f;
                    break;
                case 3:
                    this.reloadSpeedMultBonus -= 0.4f;
                    break;
                default:
                    break;
            }
        }
    }
}
