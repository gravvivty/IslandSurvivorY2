using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game._Items._ItemData
{
    public class GunpowderPowerup : Powerup
    {
        // ID 1

        public GunpowderPowerup(int level)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 1;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.bulletDamageFlat += 5;
                    break;
                case 2:
                    this.bulletDamageFlat += 10;
                    break;
                case 3:
                    this.bulletDamageFlat += 20;
                    break;
                default:
                    break;
            }
        }
    }
}
