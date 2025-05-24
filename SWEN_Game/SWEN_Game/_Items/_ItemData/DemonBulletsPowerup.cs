using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game
{
    public class DemonBulletsPowerup : Powerup
    {
        // ID 7

        public DemonBulletsPowerup(int level)
        {
            this.Level = level;
            this.itemID = 7;
        }
    }
}
