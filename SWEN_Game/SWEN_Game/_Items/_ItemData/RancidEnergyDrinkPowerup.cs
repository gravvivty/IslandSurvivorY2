using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game._Items._ItemData
{
    public class RancidEnergyDrinkPowerup : Powerup
    {
        // ID 6

        public RancidEnergyDrinkPowerup(int level, IPlayerStats playerStats) : base(playerStats)
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
                    this.attackSpeedMult += 0.9f;
                    break;
                case 2:
                    this.attackSpeedMult += 0.8f;
                    break;
                case 3:
                    this.attackSpeedMult += 0.7f;
                    break;
                default:
                    break;
            }
        }
    }
}
