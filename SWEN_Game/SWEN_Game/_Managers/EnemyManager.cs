using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SWEN_Game;

namespace SWEN_Game
{
    public class EnemyManager()
    {
        private float _enemySpawnInterval = 2f;
        private float TimeSinceLastSpawn { get; set; }
        private List<InterfaceEnemy> _allEnemies = new List<InterfaceEnemy>();
        // List of enemy types that can be spawned
        private List<string> enemyTypes = new List<string>
        {
            "Mummy",
            "Shark",
            "Baumbart",
            "Shroom",
        };
        private Random _random = new Random();
        public void SpawnEnemy(Vector2 spawnPosition)
        {
            string randomizedEnemyType = enemyTypes[_random.Next(enemyTypes.Count)];
            RandomizeSpawnedEnemy(randomizedEnemyType, spawnPosition);
            System.Diagnostics.Debug.WriteLine("Spawned an enemy" + DateTime.Now);
        }

        public void Update(List<Bullet> bulletList, Vector2 playerPosition)
        {
            float gametime = Globals.Time;
            TimeSinceLastSpawn += (float)gametime;
            if (TimeSinceLastSpawn >= _enemySpawnInterval)
            {
                SpawnEnemy(RandomizeEnemySpawnPosition());
                TimeSinceLastSpawn = 0f;
            }

            System.Diagnostics.Debug.WriteLine("Trying to spawn an enemy" + DateTime.Now);

            foreach (var enemy in _allEnemies)
            {
                enemy.Update(bulletList, playerPosition);
            }

            // Remove the dead
            _allEnemies.RemoveAll(e => !e.IsAlive);
        }

        public void Draw()
        {
            foreach (var enemy in _allEnemies)
            {
                enemy.Draw();
            }
        }

        // Randomly spawn an enemy based on the type
        private void RandomizeSpawnedEnemy(string enemyType, Vector2 spawnPosition)
        {
            switch (enemyType)
            {
                case "Mummy":
                    _allEnemies.Add(new Mummy(spawnPosition));
                    break;
                case "Shark":
                    _allEnemies.Add(new Shark(spawnPosition));
                    break;
                case "Baumbart":
                    _allEnemies.Add(new Baumbart(spawnPosition));
                    break;
                case "Shroom":
                    _allEnemies.Add(new Shroom(spawnPosition));
                    break;
                default:
                    throw new ArgumentException($"Unknown enemy type: {enemyType}");
            }
        }

        // Randomize the spawn position of an enemy within the game world
        private Vector2 RandomizeEnemySpawnPosition()
        {
            int randomX = _random.Next(0, 5000);
            int randomY = _random.Next(0, 4000);
            return new Vector2(randomX, randomY);
        }
    }
}
