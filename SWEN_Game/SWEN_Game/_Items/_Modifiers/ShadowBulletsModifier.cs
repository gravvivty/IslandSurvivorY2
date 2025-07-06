using Microsoft.Xna.Framework;
using SWEN_Game._Shooting;

namespace SWEN_Game._Items._Modifiers
{
    /// <summary>
    /// Modifier that causes bullets to spawn additional demon bullets upon collision.
    /// The number of spawned bullets is based on the modifier's level.
    /// </summary>
    public class ShadowBulletsModifier : IWeaponModifier
    {
        private static readonly Random Rand = new Random();
        private int level;

        public ShadowBulletsModifier()
        {
            level = 1;
        }

        /// <summary>
        /// Sets the level of the DemonBulletsModifier, determining how many extra bullets will spawn upon collision.
        /// </summary>
        /// <param name="level">The new level of the modifier.</param>
        public void SetShadowBulletsModifier(int level)
        {
            this.level = level;
        }

        /// <summary>
        /// Called when the weapon shoots. This modifier does not alter the shooting behavior.
        /// </summary>
        /// <param name="direction">The direction of the shot.</param>
        /// <param name="playerPos">The position of the player.</param>
        /// <param name="weapon">The player's weapon instance.</param>
        public void OnShoot(Vector2 direction, Vector2 playerPos, IPlayerWeapon weapon)
        {
            // This modifier doesnt affect shooting, only collisions
        }

        /// <summary>
        /// Called when a bullet collides with something. Spawns additional demon bullets in random directions.
        /// </summary>
        /// <param name="position">The position where the bullet collided.</param>
        /// <param name="weapon">The player's weapon instance used to spawn new bullets.</param>
        /// <param name="isShadowBullet">Flag indicating if the original bullet is already a demon bullet (to avoid infinite recursion).</param>
        public void OnBulletCollision(Vector2 position, IPlayerWeapon weapon, bool isShadowBullet)
        {
            int shadowBulletsCount = level; // Number of child bullets per bounce

            for (int i = 0; i < shadowBulletsCount; i++)
            {
                float angle = (float)(Rand.NextDouble() * MathHelper.TwoPi); // 0 to 2π
                Vector2 randomDir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                weapon.ShootInDirection(randomDir, position, isShadowBullet);
            }
        }
    }
}
