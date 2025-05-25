using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SWEN_Game
{
    public class ReverseShotModifier : IWeaponModifier
    {
        private int level;

        public ReverseShotModifier()
        {
            this.level = 1;
        }

        public void SetReverseShotModifier(int level)
        {
            this.level = level;
        }

        public void OnShoot(Vector2 direction, Vector2 playerPos, PlayerWeapon weapon)
        {
            switch (level)
            {
                case 1:
                    weapon.ShootInDirection(-direction, playerPos);
                    break;
                case 2:
                    Vector2 reverseLvl2 = -direction;
                    Vector2 perpClockwiseLvl2 = new Vector2(direction.Y, -direction.X);   // 90° CW

                    weapon.ShootInDirection(reverseLvl2, playerPos);
                    weapon.ShootInDirection(perpClockwiseLvl2, playerPos);
                    break;
                case 3:
                    Vector2 reverseLvl3 = -direction;
                    Vector2 perpClockwiseLvl3 = new Vector2(direction.Y, -direction.X);   // 90° CW
                    Vector2 perpCounterCWLvl3 = new Vector2(-direction.Y, direction.X);  // 90° CCW

                    weapon.ShootInDirection(reverseLvl3, playerPos);
                    weapon.ShootInDirection(perpClockwiseLvl3, playerPos);
                    weapon.ShootInDirection(perpCounterCWLvl3, playerPos);
                    break;

                /*
                Vector2 forward = direction;
                Vector2 backward = -direction;
                Vector2 left = new Vector2(-direction.Y, direction.X);
                Vector2 right = new Vector2(direction.Y, -direction.X);

                // Intercardinal directions are sums of adjacent directions
                Vector2 forwardLeft = Vector2.Normalize(forward + left);
                Vector2 forwardRight = Vector2.Normalize(forward + right);
                Vector2 backwardLeft = Vector2.Normalize(backward + left);
                Vector2 backwardRight = Vector2.Normalize(backward + right);

                weapon.ShootInDirection(forward, playerPos);
                weapon.ShootInDirection(backward, playerPos);
                weapon.ShootInDirection(left, playerPos);
                weapon.ShootInDirection(right, playerPos);
                weapon.ShootInDirection(forwardLeft, playerPos);
                weapon.ShootInDirection(forwardRight, playerPos);
                weapon.ShootInDirection(backwardLeft, playerPos);
                weapon.ShootInDirection(backwardRight, playerPos);
                break;
                */

                default:
                    break;
            }
        }
    }
}
