using Microsoft.Xna.Framework;

namespace SWEN_Game._Shooting
{
    /// <summary>
    /// Represents a weapon modifier that can alter the behavior of a weapon when it shoots.
    /// </summary>
    public interface IWeaponModifier
    {
        /// <summary>
        /// Called when a weapon fires a shot. Implement this method to modify shooting behavior.
        /// </summary>
        /// <param name="direction">The direction the weapon is shooting in.</param>
        /// <param name="playerPos">The world position of the player.</param>
        /// <param name="weapon">The player's weapon instance that is firing.</param>
        void OnShoot(Vector2 direction, Vector2 playerPos, IPlayerWeapon weapon);
    }
}
