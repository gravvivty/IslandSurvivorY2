using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game
{
    public class SpicyNoodlesPowerup : Powerup
    {
        // ID 9

        public SpicyNoodlesPowerup(int level)
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
                    break;
                case 2:
                    this.speedBonus += 30;
                    break;
                case 3:
                    this.speedBonus += 50;
                    break;
                default:
                    break;
            }
        }
    }
}
