using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game
{
    public class ReverseShotPowerup : Powerup
    {
        public ReverseShotPowerup(int level)
        {
            this.Level = level;
            this.ApplyPowerupLevel();
            this.itemID = 2; // Unique ID
            SetLevelValues();
        }

        private void ApplyPowerupLevel()
        {
            PlayerGameData.ReverseShotLevel = this.Level;
        }

        private void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.attackSpeedFlat += 0.1f;
                    break;
                case 2:
                    this.attackSpeedFlat += 0.15f;
                    break;
                case 3:
                    this.attackSpeedFlat += 0.2f;
                    break;
                default:
                    break;
            }
        }
    }
}
