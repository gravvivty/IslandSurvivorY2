using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SWEN_Game._Shooting;
using SWEN_Game._Utils;
using SWEN_Game._Enemies.Enemies;

namespace SWEN_Game._Entities
{
    public class EnemyManager
    {
        private float _enemySpawnInterval = 1f;
        private int _maxEnemiesAllowed = 100;
        private float _unlockCheckCooldown = 5f;
        private float _timeSinceLastUnlockCheck = 0f;
        private float TimeSinceLastSpawn { get; set; }
        private List<IEnemy> _allEnemies = new List<IEnemy>();
        private Player _player;
        // List of enemy types that can be spawned
        private List<string> spawnableEnemyTypes = new List<string>
        {
            "Mummy",
            "Shroom",
        };

        private Dictionary<string, float> currentSpawnWeights = new Dictionary<string, float>()
        {
           { "Mummy", 0.8f },
           { "Shroom", 0.2f },
        };

        private Random _random = new Random();

        public EnemyManager(Player player)
        {
            _player = player;
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

        /// <summary>
        /// Spawns a random enemy which has been unlocked.
        /// </summary>
        /// <param name="spawnPosition">Where the enemy should be spawned.</param>
        public void SpawnEnemy(Vector2 spawnPosition)
        {
            string randomizedEnemyType = GetRandomEnemyType();
            RandomizeSpawnedEnemy(randomizedEnemyType, spawnPosition);
            System.Diagnostics.Debug.WriteLine("Spawned an enemy" + DateTime.Now);
        }

        public void Update(List<Bullet> bulletList, Vector2 playerPosition)
        {
            float gametime = Globals.Time;
            TimeSinceLastSpawn += (float)gametime;
            _timeSinceLastUnlockCheck += gametime;

            CheckEnemyUnlock();
            CheckCanSpawnEnemy();

            System.Diagnostics.Debug.WriteLine("Trying to spawn an enemy" + DateTime.Now);

            foreach (var enemy in _allEnemies)
            {
                enemy.Update(bulletList, playerPosition);
            }

            CheckEnemyPlayerCollision();

            // Remove the dead
            _allEnemies.RemoveAll(e => !e.IsAlive);
        }

        /// <summary>
        /// Compares the Player Hitbox with all Enemy Hitboxes to handle taking damage.
        /// </summary>
        private void CheckEnemyPlayerCollision()
        {
            foreach (var enemy in _allEnemies)
            {
                // Check collision with player only if player is not invincible
                if (!_player.GetIsInvincible() && enemy.Hitbox.Intersects(_player.Hitbox))
                {
                    _player.TakeDamage(enemy.EnemyDamage);  // reduce health by enemy damage
                    System.Diagnostics.Debug.WriteLine("Player hit by enemy: " + DateTime.Now);
                }
            }
        }

        /// <summary>
        /// Check if an enemy can be spawned depending on SpawnInterval and MaxEnemiesAllowed.
        /// </summary>
        private void CheckCanSpawnEnemy()
        {
            if (TimeSinceLastSpawn >= _enemySpawnInterval && _allEnemies.Count < _maxEnemiesAllowed)
            {
                SpawnEnemy(RandomizeEnemySpawnPosition(_player.RealPos));
                TimeSinceLastSpawn = 0f;
            }
        }

        /// <summary>
        /// Check and unlock enemies depending on UnlockCheckCooldown Interval.
        /// </summary>
        private void CheckEnemyUnlock()
        {
            if (_timeSinceLastUnlockCheck >= _unlockCheckCooldown)
            {
                UpdateEnemyUnlocks();
                _timeSinceLastUnlockCheck = 0f;
            }
        }

        /// <summary>
        /// Create an enemy depending on parameters.
        /// </summary>
        /// <param name="enemyType">Name of the enemy.</param>
        /// <param name="spawnPosition">Spawn Position of the enemy.</param>
        /// <exception cref="ArgumentException">Triggered when there is an Unknown Enemy Type.</exception>
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

        /// <summary>
        /// Chooses a random spawn position around the player.
        /// </summary>
        /// <param name="playerPosition">Player Position.</param>
        /// <returns>Random Vector close to the Player.</returns>
        private Vector2 RandomizeEnemySpawnPosition(Vector2 playerPosition)
        {
            float minDistance = 400f; // Minimum spawn radius
            float maxDistance = 600f; // Maximum spawn radius

            double angle = _random.NextDouble() * MathHelper.TwoPi;
            float distance = minDistance + (float)_random.NextDouble() * (maxDistance - minDistance);

            float x = playerPosition.X + (float)Math.Cos(angle) * distance;
            float y = playerPosition.Y + (float)Math.Sin(angle) * distance;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Adds and removes Enemy Types from the spawnableEnemyTypes List depending on TotalGameTime.
        /// </summary>
        private void UpdateEnemyUnlocks()
        {
            float gameTime = Globals.TotalGameTime;

            if (gameTime < 15f)
            {
                currentSpawnWeights = new Dictionary<string, float>()
                {
                   { "Mummy", 0.8f },
                   { "Shroom", 0.2f },
                };
                spawnableEnemyTypes = new List<string> { "Mummy", "Shroom" };
            }
            else if (gameTime >= 15f && gameTime < 180f)
            {
                currentSpawnWeights = new Dictionary<string, float>()
                {
                   { "Mummy", 0.5f },
                   { "Shark", 0.3f },
                   { "Shroom", 0.2f },
                };
                spawnableEnemyTypes = new List<string> { "Mummy", "Shark", "Shroom" };
            }
            else if (gameTime >= 30f)
            {
                currentSpawnWeights = new Dictionary<string, float>()
                {
                    { "Mummy", 0.4f },
                    { "Shark", 0.3f },
                    { "Shroom", 0.2f },
                    { "Baumbart", 0.1f },
                };
                spawnableEnemyTypes = new List<string> { "Mummy", "Shark", "Shroom", "Baumbart" };
            }
        }

        /// <summary>
        /// Chooses a random Enemy depepnding on their weights.
        /// </summary>
        /// <returns>Name of the enemy.</returns>
        private string GetRandomEnemyType()
        {
            // e.g. 0.4+0.9=1.3
            float totalWeight = currentSpawnWeights.Values.Sum();

            // e.g. 0.35 * 1.3
            float randomValue = (float)_random.NextDouble() * totalWeight;

            float cumulative = 0f;
            foreach (var weight in currentSpawnWeights)
            {
                cumulative += weight.Value;
                if (randomValue <= cumulative)
                {
                    return weight.Key;
                }
            }

            // fallback
            return currentSpawnWeights.Keys.First();
        }
    }
}
