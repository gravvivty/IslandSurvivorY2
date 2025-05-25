using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game
{
    public class PiercerPowerup : Powerup
    {
        // ID 3

        public PiercerPowerup(int level)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 3;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.bulletPierce += 1;
                    this.bulletDamageMult += 1.03f;
                    break;
                case 2:
                    this.bulletPierce += 1;
                    this.bulletDamageMult += 1.05f;
                    break;
                case 3:
                    this.bulletPierce += 1;
                    this.bulletDamageMult += 1.10f;
                    break;
                default:
                    break;
            }
        }
    }
}
