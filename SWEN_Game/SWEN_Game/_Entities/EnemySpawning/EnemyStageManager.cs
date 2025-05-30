using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SWEN_Game._Entities.Enemies;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities.EnemySpawning
{
    public class EnemyStageManager
    {
        private List<EnemySpawnStage> _stages;
        private int _lastStageIndex = -1;
        private EnemySpawner _spawner;
        private Player _player;
        private bool _spawnedFirstBoss = false;
        private bool _spawnedSecondBoss = false;

        public EnemyStageManager(Player player, EnemySpawner spawner)
        {
            _player = player;
            _spawner = spawner;
            InitStages();
        }

        public void Update()
        {
            float time = Globals.TotalGameTime;
            EnemySpawnStage stage = _stages.FirstOrDefault(s => time >= s.StartTime && time < s.EndTime);

            if (stage != null && stage.StageIndex != _lastStageIndex)
            {
                _lastStageIndex = stage.StageIndex;
                _spawner.SetSpawnWeights(stage.SpawnWeights);

                if (stage.StageIndex == 3 && !_spawnedFirstBoss)
                {
                    _spawnedFirstBoss = true;
                    Vector2 pos = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("SlimeBoss", pos);
                }
                else if (stage.StageIndex == 4 && !_spawnedSecondBoss)
                {
                    _spawnedSecondBoss = true;
                    Vector2 pos = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("SlimeBoss", pos);
                }
                else if (stage.StageIndex == 0) // DEBUG SPAWN - REMOVE FOR FINAL BUILD
                {
                    _spawnedSecondBoss = true;
                    Vector2 pos = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("Witch", pos);
                    _spawner.SpawnEnemy("Witch", pos);
                    _spawner.SpawnEnemy("Witch", pos);
                    _spawner.SpawnEnemy("Witch", pos);
                }
            }
        }

        private void InitStages()
        {
            _stages = new List<EnemySpawnStage>
            {
                new() { StageIndex = 0, StartTime = 0f, EndTime = 60f, SpawnWeights = new() { { "Mummy", 0.5f }, { "Shroom", 0.5f } } },
                new() { StageIndex = 1, StartTime = 60f, EndTime = 120f, SpawnWeights = new() { { "Mummy", 0.5f }, { "Shark", 0.2f }, { "Shroom", 0.3f } } },
                new() { StageIndex = 2, StartTime = 120f, EndTime = 240f, SpawnWeights = new() { { "Mummy", 0.4f }, { "Shark", 0.3f }, { "Shroom", 0.2f }, { "Baumbart", 0.1f } } },
                new() { StageIndex = 3, StartTime = 240f, EndTime = 300f, SpawnWeights = new() { { "Mummy", 0.3f }, { "Shark", 0.4f }, { "Shroom", 0.1f }, { "Baumbart", 0.2f } } },
                new() { StageIndex = 4, StartTime = 300f, EndTime = 420f, SpawnWeights = new() { { "Mummy", 0.1f }, { "Shark", 0.4f }, { "Baumbart", 0.3f }, { "Witch", 0.2f } } },
            };
        }
    }
}