using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class HeavyMagsPowerup : Powerup
    {
        // ID 11

        public HeavyMagsPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 11;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.magazineSizeBonus += 3;
                    break;
                case 2:
                    this.magazineSizeBonus += 6;
                    break;
                case 3:
                    this.magazineSizeBonus += 13;
                    break;
                default:
                    break;
            }
        }
    }
}
