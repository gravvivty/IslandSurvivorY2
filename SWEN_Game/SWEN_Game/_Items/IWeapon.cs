using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Shooting;

namespace SWEN_Game._Items
{
    public interface IWeapon
    {
        string Name { get; set; }
        float AttackSpeed { get; set; }
        float BulletDamage { get; set; }
        float BulletSize { get; set; }
        int BulletsPerShot { get; set; }
        float BulletSpread { get; set; }
        int CurrentAmmo { get; set; }
        bool IsReloading { get; set; }
        int MagazineSize { get; set; }
        int Pierce { get; set; }
        float ReloadTime { get; set; }
        float ReloadTimer { get; set; }
        float ShotSpeed { get; set; }
        float TimeSinceLastShot { get; set; }
        Texture2D BulletTexture { get; set; }
        Texture2D IngameSprite { get; set; }
        Texture2D IconSprite { get; set; }

        Weapon Clone();
    }
}