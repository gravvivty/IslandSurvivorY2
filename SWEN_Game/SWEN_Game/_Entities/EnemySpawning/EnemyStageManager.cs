using Microsoft.Xna.Framework;
using SharpFont.TrueType;
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
        private bool _spawnedThirdBoss = false;

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
                _spawner.SetSpawnInterval(stage.SpawnInterval);

                if (stage.StageIndex == 4 && !_spawnedFirstBoss)
                {
                    _spawnedFirstBoss = true;
                    Vector2 pos = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("SlimeBoss", pos);
                }
                else if (stage.StageIndex == 6 && !_spawnedSecondBoss)
                {
                    _spawnedSecondBoss = true;
                    Vector2 pos = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("SlimeBoss", pos);
                    Vector2 pos1 = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("SlimeBoss", pos1);
                    Vector2 pos2 = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("SlimeBoss", pos2);
                }
                else if (stage.StageIndex == 8 && !_spawnedThirdBoss)
                {
                    _spawnedThirdBoss = true;
                    Vector2 pos = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("SlimeBoss", pos);
                    Vector2 pos1 = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("SlimeBoss", pos1);
                    Vector2 pos2 = _spawner.RandomizeSpawnPosition(_player.RealPos);
                    _spawner.SpawnEnemy("Reaper", pos2);
                }
                else if (stage.StageIndex == 0) // DEBUG SPAWN - REMOVE FOR FINAL BUILD
                {
                    Vector2 pos = _spawner.RandomizeSpawnPosition(_player.RealPos);
                }
            }
        }

        private void InitStages()
        {
            _stages = new List<EnemySpawnStage>
            {
                new() { StageIndex = 0, StartTime = 0f, EndTime = 60f, SpawnInterval = 2f, SpawnWeights = new() { { "Mummy", 0.5f }, { "Shroom", 0.5f } } },
                new() { StageIndex = 1, StartTime = 60f, EndTime = 120f, SpawnInterval = 1.8f, SpawnWeights = new() { { "Mummy", 0.5f }, { "Shark", 0.2f }, { "Shroom", 0.3f } } },
                new() { StageIndex = 2, StartTime = 120f, EndTime = 240f, SpawnInterval = 1.5f, SpawnWeights = new() { { "Mummy", 0.4f }, { "Shark", 0.3f }, { "Shroom", 0.2f }, { "Baumbart", 0.1f } } },
                new() { StageIndex = 3, StartTime = 240f, EndTime = 300f, SpawnInterval = 1.2f, SpawnWeights = new() { { "Mummy", 0.3f }, { "Shark", 0.4f }, { "Shroom", 0.1f }, { "Baumbart", 0.2f }, { "Witch", 0.1f } } },
                new() { StageIndex = 4, StartTime = 300f, EndTime = 360f, SpawnInterval = 0.8f, SpawnWeights = new() { { "Mummy", 0.2f }, { "Shark", 0.4f }, { "Shroom", 0.1f }, { "Baumbart", 0.3f }, { "Witch", 0.2f } } },
                new() { StageIndex = 5, StartTime = 360f, EndTime = 420f, SpawnInterval = 0.6f, SpawnWeights = new() { { "Mummy", 0.2f }, { "Shark", 0.5f }, { "Shroom", 0.1f }, { "Baumbart", 0.3f }, { "Witch", 0.4f }, { "Ekek", 0.3f } } },
                new() { StageIndex = 6, StartTime = 420f, EndTime = 480f, SpawnInterval = 0.5f, SpawnWeights = new() { { "Shark", 0.4f }, { "Shroom", 0.1f }, { "Baumbart", 0.3f }, { "Witch", 0.6f }, { "Ekek", 0.3f }, { "Demon", 0.3f } } },
                new() { StageIndex = 7, StartTime = 480f, EndTime = 540f, SpawnInterval = 0.5f, SpawnWeights = new() { { "Shark", 0.3f }, { "Shroom", 0.05f }, { "Witch", 0.7f }, { "Ekek", 0.4f }, { "Demon", 0.5f }, { "Siren", 0.3f } } },
                new() { StageIndex = 8, StartTime = 540f, EndTime = 660f, SpawnInterval = 0.5f, SpawnWeights = new() { { "Shroom", 0.2f }, { "Witch", 0.5f }, { "Demon", 0.3f }, { "Siren", 0.6f } } },
            };
        }
    }
}