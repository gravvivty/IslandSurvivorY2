using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Interfaces;
using SWEN_Game._Utils;

namespace SWEN_Game._Items._ItemData
{
    public class FrozenTearsPowerup : Powerup
    {
        // ID 13

        public FrozenTearsPowerup(int level, IPlayerStats playerStats)
            : base(playerStats)
        {
            this.Level = level;
            SetLevelValues();
            this.itemID = 13;
        }

        public void SetLevelValues()
        {
            switch (this.Level)
            {
                case 1:
                    this.slowChance += 0.15f;
                    this.bulletPierce += 1;
                    this.bulletDamageMult += 0.02f;
                    this.bulletTexture = Globals.Content.Load<Texture2D>("Sprites/Bullets/VanillaBulletBlue");
                    break;
                case 2:
                    this.slowChance += 0.30f;
                    this.bulletPierce += 1;
                    this.bulletDamageMult += 0.04f;
                    break;
                case 3:
                    this.slowChance += 0.50f;
                    this.bulletPierce += 1;
                    this.bulletDamageMult += 0.1f;
                    break;
                default:
                    break;
            }
        }
    }
}
