using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SWEN_Game._Shooting;
using SWEN_Game._Utils;
using SWEN_Game._Entities.Enemies;
using SWEN_Game._Entities.EnemySpawning;

namespace SWEN_Game._Entities
{
    public class EnemyManager : IEnemyConsumer
    {
        public Player _player { get; set; }
        private List<Enemy> _allEnemies = new List<Enemy>();
        private List<Enemy> _enemiesToAdd = new List<Enemy>();

        private EnemySpawner _enemySpawner;
        private EnemyStageManager _stageManager;
        private EnemyCollisionHandler _collisionHandler;

        public EnemyManager(Player player)
        {
            _player = player;
            _enemySpawner = new EnemySpawner(_player, this);
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

        public void Update(List<Bullet> bulletList, Vector2 playerPosition)
        {
            _stageManager.Update();
            _enemySpawner.TrySpawnEnemy(_allEnemies.Count);

            foreach (var enemy in _allEnemies)
            {
                enemy.Update(bulletList, playerPosition, this);
            }

            _collisionHandler.CheckCollisions(_allEnemies);

            _allEnemies.RemoveAll(e => !e.IsAlive);
            _allEnemies.AddRange(_enemiesToAdd);
            _enemiesToAdd.Clear();
        }
    }
}
