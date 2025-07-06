using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class AdrenalinePowerup : Powerup
    {
        // ID 4

        public AdrenalinePowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 4;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.attackSpeedFlat -= 0.05f;
                    break;
                case 2:
                    this.attackSpeedFlat -= 0.1f;
                    break;
                case 3:
                    this.attackSpeedFlat -= 0.15f;
                    break;
                default:
                    break;
            }
        }
    }
}
