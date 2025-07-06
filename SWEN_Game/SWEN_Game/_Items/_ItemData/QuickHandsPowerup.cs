using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class QuickHandsPowerup : Powerup
    {
        // ID 8

        public QuickHandsPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 8;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.reloadSpeedBonus -= 0.2f;
                    break;
                case 2:
                    this.reloadSpeedBonus -= 0.4f;
                    break;
                case 3:
                    this.reloadSpeedBonus -= 0.6f;
                    break;
                default:
                    break;
            }
        }
    }
}
