using Microsoft.Xna.Framework;

namespace SWEN_Game._Shooting
{
    public interface IPlayerWeapon
    {
        List<IWeaponModifier> GetModifiers();
        void ShootInDirection(Vector2 direction, Vector2 player_position, bool? isDemonBullet = null);
    }
}