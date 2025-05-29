using SWEN_Game._PlayerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game._Items._ItemData
{
    public class DemonBulletsPowerup : Powerup
    {
        // ID 7

        public DemonBulletsPowerup(int level, IPlayerStats playerStats) : base(playerStats)
        {
            this.Level = level;
            this.itemID = 7;
        }
    }
}
