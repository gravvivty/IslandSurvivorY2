using Microsoft.Xna.Framework;
using SWEN_Game._Shooting;

/// <summary>
/// A weapon modifier that fires additional bullets in reverse or perpendicular directions
/// depending on its level.
/// </summary>

namespace SWEN_Game._Items._Modifiers
{
    /// <summary>
    /// A weapon modifier that fires additional bullets at angular offsets.
    /// </summary>
    public class MultiShotModifier : IWeaponModifier
    {
        private int level;

        public MultiShotModifier()
        {
            level = 1;
        }

        /// <summary>
        /// Sets the modifier level which determines how many and at what angles bullets are shot.
        /// </summary>
        /// <param name="level">The new modifier level.</param>
        public void SetMultiShotModifier(int level)
        {
            this.level = level;
        }

        /// <summary>
        /// Fires bullets at angular offsets based on the modifier level.
        /// </summary>
        /// <param name="direction">Direction of where to shoot.</param>
        /// <param name="playerPos">Position of the Player.</param>
        /// <param name="weapon">Current PlayerWeapon.</param>
        public void OnShoot(Vector2 direction, Vector2 playerPos, IPlayerWeapon weapon)
        {
            direction.Normalize(); // Ensure direction is normalized

            switch (level)
            {
                case 1:
                    // Shoot at +180°
                    weapon.ShootInDirection(RotateVector(direction, 180), playerPos);
                    break;

                case 2:
                    // Shoot at +10°, +180°
                    weapon.ShootInDirection(RotateVector(direction, 180), playerPos);
                    weapon.ShootInDirection(RotateVector(direction, 10), playerPos);
                    break;

                case 3:
                    // Shoot at +10°, -10°, +180°
                    weapon.ShootInDirection(RotateVector(direction, 180), playerPos);
                    weapon.ShootInDirection(RotateVector(direction, 10), playerPos);
                    weapon.ShootInDirection(RotateVector(direction, -10), playerPos);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Rotates a vector by a specified angle in degrees.
        /// </summary>
        private Vector2 RotateVector(Vector2 vector, float degrees)
        {
            float radians = MathHelper.ToRadians(degrees);
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            return new Vector2(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos);
        }
    }
}