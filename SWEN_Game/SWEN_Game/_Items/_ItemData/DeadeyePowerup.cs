using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class DeadeyePowerup : Powerup
    {
        // ID 10

        public DeadeyePowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 10;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.critChance += 0.1f;
                    break;
                case 2:
                    this.critChance += 0.25f;
                    break;
                case 3:
                    this.critChance += 0.50f;
                    break;
                default:
                    break;
            }
        }
    }
}
