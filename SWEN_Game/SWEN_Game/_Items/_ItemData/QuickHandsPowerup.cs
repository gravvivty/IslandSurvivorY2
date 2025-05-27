using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game
{
    public class QuickHandsPowerup : Powerup
    {
        // ID 8

        public QuickHandsPowerup(int level)
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
