using Microsoft.Xna.Framework;
using SWEN_Game._Entities.EnemySpawning;
using SWEN_Game._Items;
using SWEN_Game._Shooting;

namespace SWEN_Game._Entities
{
    public class EnemyManager : IEnemyContext
    {
        public Player _player { get; set; }
        public Vector2 PlayerPos => _player.RealPos;
        private List<Enemy> _allEnemies = new List<Enemy>();
        private List<Enemy> _enemiesToAdd = new List<Enemy>();

        private EnemySpawner _enemySpawner;
        private EnemyStageManager _stageManager;
        private EnemyCollisionHandler _collisionHandler;
        private EnemyFactory _enemyFactory;

        public EnemyManager(Player player)
        {
            _player = player;
            _enemyFactory = new EnemyFactory();
            _enemySpawner = new EnemySpawner(_player, this, _enemyFactory);
            _stageManager = new EnemyStageManager(_player, _enemySpawner);
            _collisionHandler = new EnemyCollisionHandler(_player);
        }

        /// <summary>
        /// Draws all enemies using each enemies Draw() function.
        /// </summary>
        public void Draw()
        {
            foreach (var enemy in _allEnemies)
            {
                enemy.Draw();
            }
        }

        public void QueueEnemy(Enemy e)
        {
            _enemiesToAdd.Add(e);
        }

        public void QueueEnemy(string type, Vector2 pos)
        {
            Enemy newEnemy = _enemyFactory.CreateEnemy(type, pos);
            _enemiesToAdd.Add(newEnemy);
        }

        public void Update(List<Bullet> bulletList, Vector2 playerPosition)
        {
            _stageManager.Update();
            _enemySpawner.TrySpawnEnemy(_allEnemies.Count);

            foreach (var enemy in _allEnemies)
            {
                enemy.Update(bulletList, playerPosition, this);
                if (!enemy.IsAlive)
                {
                    PlayerGameData.Instance.AddXP(enemy.XPReward);
                }
            }

            _collisionHandler.CheckCollisions(_allEnemies);

            _allEnemies.RemoveAll(e => !e.IsAlive);
            _allEnemies.AddRange(_enemiesToAdd);
            _enemiesToAdd.Clear();
        }

        public List<Enemy> GetAllEnemies()
        {
            return _allEnemies;
        }
    }
}
