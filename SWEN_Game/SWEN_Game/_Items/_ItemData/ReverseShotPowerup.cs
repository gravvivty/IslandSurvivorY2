using SWEN_Game._Entities;
using SWEN_Game._PlayerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_Game._Items._ItemData
{
    public class ReverseShotPowerup : Powerup
    {
        // ID 2

        public ReverseShotPowerup(int level, IPlayerStats playerStats) : base(playerStats)
        {
            this.Level = level;
            this.ApplyPowerupLevel();
            this.itemID = 2; // Unique ID
            SetLevelValues();
        }

        private void ApplyPowerupLevel()
        {
            PlayerGameData.Instance.ReverseShotLevel = this.Level;
        }

        private void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.attackSpeedFlat += 0.03f;
                    break;
                case 2:
                    this.attackSpeedFlat += 0.06f;
                    break;
                case 3:
                    this.attackSpeedFlat += 0.08f;
                    break;
                default:
                    break;
            }
        }
    }
}
