using SWEN_Game._Interfaces;

namespace SWEN_Game._Items._ItemData
{
    public class ShadowBulletsPowerup : Powerup
    {
        // ID 7

        public ShadowBulletsPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            this.itemID = 7;
        }
    }
}
