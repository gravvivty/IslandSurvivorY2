using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Items;

namespace SWEN_Game._Interfaces
{
    public interface IPlayerStats
    {
        IWeapon GetWeapon();
        IWeapon GetBaseWeapon();
        void AddAttackSpeedMult(float value);
        void AddShotSpeedMult(float value);
        void AddBulletSizeMult(float value);
        void AddBulletSpreadMult(float value);
        void AddBulletsPerShotMult(int value);
        void AddBulletDamageMult(float value);

        void AddAttackSpeedFlat(float value);
        void AddShotSpeedFlat(float value);
        void AddBulletSizeFlat(float value);
        void AddBulletSpreadFlat(float value);
        void AddBulletsPerShotFlat(int value);
        void AddBulletDamageFlat(float value);

        void AddBulletPierceBonus(int value);
        void AddMagazineSizeBonus(int value);
        void AddReloadSpeedBonus(float value);
        void AddReloadSpeedMultBonus(float value);
        void AddSpeedBonus(float value);
        void AddCritChance(float value);
        void AddSlowChance(float value);
        int GetCurrentHealth();
        void SetCurrentHealth(int value);
        Texture2D SetBulletTexture(Texture2D value);
        float GetXP();
        void AddXP(float amount);
        int GetLevel();
    }
}