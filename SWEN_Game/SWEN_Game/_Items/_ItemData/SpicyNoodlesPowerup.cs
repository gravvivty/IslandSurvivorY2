using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class SpicyNoodlesPowerup : Powerup
    {
        // ID 9

        public SpicyNoodlesPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 9;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.speedBonus += 10;
                    this.critChance += 0.05f;
                    break;
                case 2:
                    this.speedBonus += 30;
                    this.critChance += 0.10f;
                    break;
                case 3:
                    this.speedBonus += 60;
                    this.critChance += 0.20f;
                    break;
                default:
                    break;
            }
        }
    }
}
