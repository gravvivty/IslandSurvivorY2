using SWEN_Game._PlayerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game._Items._ItemData
{
    public class RocketspeedPowerup : Powerup
    {
        // ID 5

        public RocketspeedPowerup(int level, IPlayerStats playerStats) : base(playerStats)
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
                    break;
                case 2:
                    this.shotSpeedFlat += 200;
                    break;
                case 3:
                    this.shotSpeedFlat += 350;
                    break;
                default:
                    break;
            }
        }
    }
}
