using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class ExtremeTeapowderPowerup : Powerup
    {
        // ID 12

        public ExtremeTeapowderPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 12;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.bulletDamageMult += 0.1f;
                    this.bulletDamageFlat += 2;
                    break;
                case 2:
                    this.bulletDamageMult += 0.2f;
                    this.bulletDamageFlat += 4;
                    break;
                case 3:
                    this.bulletDamageMult += 0.5f;
                    this.bulletDamageFlat += 8;
                    break;
                default:
                    break;
            }
        }
    }
}
