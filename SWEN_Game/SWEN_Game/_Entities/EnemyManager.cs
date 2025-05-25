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
    public class EnemyManager
    {
        private float _enemySpawnInterval = 1f;
        private int _maxEnemies = 100;
        private float _unlockCheckCooldown = 5f;
        private float _timeSinceLastUnlockCheck = 0f;
        private float TimeSinceLastSpawn { get; set; }
        private List<IEnemy> _allEnemies = new List<IEnemy>();
        private Player _player;
        // List of enemy types that can be spawned
        private List<string> spawnableEnemyTypes = new List<string>
        {
            "Mummy",
        };
        private Random _random = new Random();

        public EnemyManager(Player player)
        {
            _player = player;
        }

        public void SpawnEnemy(Vector2 spawnPosition)
        {
            string randomizedEnemyType = spawnableEnemyTypes[_random.Next(spawnableEnemyTypes.Count)];
            RandomizeSpawnedEnemy(randomizedEnemyType, spawnPosition);
            System.Diagnostics.Debug.WriteLine("Spawned an enemy" + DateTime.Now);
        }

        public void Update(List<Bullet> bulletList, Vector2 playerPosition)
        {
            float gametime = Globals.Time;
            TimeSinceLastSpawn += (float)gametime;

            if (_timeSinceLastUnlockCheck >= _unlockCheckCooldown)
            {
                UpdateEnemyUnlocks();
                _timeSinceLastUnlockCheck = 0f;
            }

            if (TimeSinceLastSpawn >= _enemySpawnInterval && _allEnemies.Count < _maxEnemies)
            {
                SpawnEnemy(RandomizeEnemySpawnPosition(_player.RealPos));
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
        private Vector2 RandomizeEnemySpawnPosition(Vector2 playerPosition)
        {
            // Off screen padding to push enemies beyond view
            int padding = 200;

            // Half screen size to offset from center
            int halfScreenWidth = Globals.WindowSize.X / 2;
            int halfScreenHeight = Globals.WindowSize.Y / 2;

            // Randomly pick a side
            int side = _random.Next(4);

            float x = 0, y = 0;

            switch (side)
            {
                case 0: // Top
                    x = playerPosition.X + _random.Next(-halfScreenWidth, halfScreenWidth);
                    y = playerPosition.Y - halfScreenHeight - padding;
                    break;
                case 1: // Bottom
                    x = playerPosition.X + _random.Next(-halfScreenWidth, halfScreenWidth);
                    y = playerPosition.Y + halfScreenHeight + padding;
                    break;
                case 2: // Left
                    x = playerPosition.X - halfScreenWidth - padding;
                    y = playerPosition.Y + _random.Next(-halfScreenHeight, halfScreenHeight);
                    break;
                case 3: // Right
                    x = playerPosition.X + halfScreenWidth + padding;
                    y = playerPosition.Y + _random.Next(-halfScreenHeight, halfScreenHeight);
                    break;
            }

            return new Vector2(x, y);
        }

        private void UpdateEnemyUnlocks()
        {
            float gameTime = Globals.TotalGameTime;

            if (gameTime >= 0 && !spawnableEnemyTypes.Contains("Mummy"))
            {
                spawnableEnemyTypes.Add("Mummy");
            }

            if (gameTime >= 60 && !spawnableEnemyTypes.Contains("Shroom"))
            {
                spawnableEnemyTypes.Add("Shroom");
            }

            if (gameTime >= 120 && !spawnableEnemyTypes.Contains("Shark"))
            {
                spawnableEnemyTypes.Add("Shark");
            }

            if (gameTime >= 180 && !spawnableEnemyTypes.Contains("Baumbart"))
            {
                spawnableEnemyTypes.Add("Baumbart");
            }
        }
    }
}
