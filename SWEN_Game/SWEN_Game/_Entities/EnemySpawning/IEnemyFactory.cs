using Microsoft.Xna.Framework;

namespace SWEN_Game._Entities.EnemySpawning
{
    public interface IEnemyFactory
    {
        Enemy CreateEnemy(string type, Vector2 pos);
    }
}
