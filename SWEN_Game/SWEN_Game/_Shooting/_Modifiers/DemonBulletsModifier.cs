using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SWEN_Game
{
    public class DemonBulletsModifier : IWeaponModifier
    {
        private int level;

        public DemonBulletsModifier()
        {
            this.level = 1;
        }

        public void SetDemonBulletsModifier(int level)
        {
            this.level = level;
        }

        public void OnShoot(Vector2 direction, Vector2 playerPos, PlayerWeapon weapon)
        {
            // This modifier doesnt affect shooting, only collisions
        }

        public void OnBulletCollision(Vector2 position, PlayerWeapon weapon, bool isDemonBullet)
        {
            Random rand = new Random();
            int demonBulletsCount = level; // Number of child bullets per bounce

            for (int i = 0; i < demonBulletsCount; i++)
            {
                float angle = (float)(rand.NextDouble() * Math.PI * 2); // 0 to 2π
                Vector2 randomDir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                weapon.ShootInDirection(randomDir, position, isDemonBullet);
            }
        }
    }
}
