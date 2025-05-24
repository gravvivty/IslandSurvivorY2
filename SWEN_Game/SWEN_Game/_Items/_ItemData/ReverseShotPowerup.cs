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
        }

        private void ApplyPowerupLevel()
        {
            PlayerGameData.ReverseShotLevel = this.Level;
        }
    }
}
