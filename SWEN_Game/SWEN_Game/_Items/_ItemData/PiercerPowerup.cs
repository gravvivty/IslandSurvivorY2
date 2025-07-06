using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class PiercerPowerup : Powerup
    {
        // ID 3

        public PiercerPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 3;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.bulletPierce += 1;
                    break;
                case 2:
                    this.bulletPierce += 3;
                    break;
                case 3:
                    this.bulletPierce += 6;
                    break;
                default:
                    break;
            }
        }
    }
}
