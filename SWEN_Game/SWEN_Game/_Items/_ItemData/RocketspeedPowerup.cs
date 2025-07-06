using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class RocketspeedPowerup : Powerup
    {
        // ID 5

        public RocketspeedPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 5;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.shotSpeedFlat += 100;
                    this.speedBonus += 10;
                    break;
                case 2:
                    this.shotSpeedFlat += 200;
                    this.speedBonus += 20;
                    break;
                case 3:
                    this.shotSpeedFlat += 500;
                    this.speedBonus += 30;
                    break;
                default:
                    break;
            }
        }
    }
}
