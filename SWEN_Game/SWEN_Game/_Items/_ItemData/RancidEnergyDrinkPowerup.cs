using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class RancidEnergyDrinkPowerup : Powerup
    {
        // ID 6

        public RancidEnergyDrinkPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 6;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.attackSpeedMult -= 0.1f;
                    break;
                case 2:
                    this.attackSpeedMult -= 0.2f;
                    break;
                case 3:
                    this.attackSpeedMult -= 0.5f;
                    break;
                default:
                    break;
            }
        }
    }
}
