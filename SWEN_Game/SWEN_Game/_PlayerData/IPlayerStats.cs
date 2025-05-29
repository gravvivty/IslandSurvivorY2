namespace SWEN_Game._PlayerData
{
    public interface IPlayerStats
    {
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
        void AddSpeedBonus(float value);
        int GetCurrentHealth();
        void SetCurrentHealth(int value);
    }
}