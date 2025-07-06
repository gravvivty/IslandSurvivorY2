using Microsoft.Xna.Framework;
using SWEN_Game._Entities.Enemies;

namespace SWEN_Game._Entities
{
    public interface IEnemyContext : IEnemyConsumer
    {
        // Currently, IEnemyConsumer already includes:
        // void QueueEnemy(Enemy e);
        Vector2 PlayerPos { get; }
        void QueueEnemy(string type, Vector2 pos);
    }
}
