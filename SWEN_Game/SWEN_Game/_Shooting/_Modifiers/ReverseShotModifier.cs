using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SWEN_Game._Shooting._Modifiers
{
    /// <summary>
    /// A weapon modifier that fires additional bullets in reverse or perpendicular directions
    /// depending on its level.
    /// </summary>
    public class ReverseShotModifier : IWeaponModifier
    {
        private int level;

        public ReverseShotModifier()
        {
            this.level = 1;
        }

        /// <summary>
        /// Sets the modifier level which determines how many and in which directions bullets are shot.
        /// </summary>
        /// <param name="level">The new modifier level.</param>
        public void SetReverseShotModifier(int level)
        {
            this.level = level;
        }

        /// <summary>
        /// Executes the reverse shot behavior when the player fires a weapon.
        /// Based on the current level, additional bullets are fired in reverse or perpendicular directions.
        /// </summary>
        /// <param name="direction">The original shooting direction.</param>
        /// <param name="playerPos">The position of the player when shooting.</param>
        /// <param name="weapon">The weapon being used to shoot.</param>
        public void OnShoot(Vector2 direction, Vector2 playerPos, PlayerWeapon weapon)
        {
            switch (level)
            {
                case 1:
                    weapon.ShootInDirection(-direction, playerPos); // Behind
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
