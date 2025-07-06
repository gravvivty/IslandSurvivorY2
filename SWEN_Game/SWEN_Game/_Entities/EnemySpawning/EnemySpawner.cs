using Microsoft.Xna.Framework;
using SWEN_Game._Entities.Enemies;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities.EnemySpawning
{
    public class EnemySpawner
    {
        private readonly IEnemyConsumer _enemyManagerConsumer;
        private IEnemyFactory _enemyFactory;
        private float _enemySpawnInterval = 1.5f;
        private float _timeSinceLastSpawn = 0f;
        private int _maxAllowedEnemies = 200;
        private Dictionary<string, float> _currentSpawnWeights;
        private Random _random = new Random();
        private Player _player;

        public EnemySpawner(Player player, IEnemyConsumer consumer, IEnemyFactory enemyFactory)
        {
            _player = player;
            _currentSpawnWeights = new Dictionary<string, float> { { "Mummy", 0.5f }, { "Shroom", 0.5f } };
            _enemyManagerConsumer = consumer;
            _enemyFactory = enemyFactory;
        }

        public void SetSpawnWeights(Dictionary<string, float> weights)
        {
            _currentSpawnWeights = weights;
        }

        public void TrySpawnEnemy(int currentEnemyCount)
        {
            _timeSinceLastSpawn += Globals.Time;
            if (_timeSinceLastSpawn >= _enemySpawnInterval && currentEnemyCount < _maxAllowedEnemies)
            {
                string type = GetRandomEnemyType();
                Vector2 pos = RandomizeSpawnPosition(_player.RealPos);
                SpawnEnemy(type, pos);
                _timeSinceLastSpawn = 0f;
            }
        }

        public void SpawnEnemy(string type, Vector2 pos)
        {
            Enemy enemy = _enemyFactory.CreateEnemy(type, pos);
            _enemyManagerConsumer.QueueEnemy(enemy);
        }

        public Vector2 RandomizeSpawnPosition(Vector2 playerPos)
        {
            float min = 400f, max = 600f;
            double angle = _random.NextDouble() * MathHelper.TwoPi;
            float dist = min + (float)_random.NextDouble() * (max - min);
            return playerPos + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * dist;
        }

        public void SetSpawnInterval(float interval)
        {
            _enemySpawnInterval = interval;
        }

        private string GetRandomEnemyType()
        {
            float total = _currentSpawnWeights.Values.Sum();
            float roll = (float)_random.NextDouble() * total;
            float sum = 0f;
            foreach (var weight in _currentSpawnWeights)
            {
                sum += weight.Value;
                if (roll <= sum)
                {
                    return weight.Key;
                }
            }

            return _currentSpawnWeights.Keys.First();
        }
    }
}
