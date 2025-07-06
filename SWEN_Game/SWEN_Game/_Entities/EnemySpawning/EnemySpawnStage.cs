namespace SWEN_Game._Entities.EnemySpawning
{
    public class EnemySpawnStage
    {
        public int StageIndex { get; set; }
        public float StartTime { get; set; }
        public float EndTime { get; set; }
        public float SpawnInterval { get; set; } = 2f;
        public Dictionary<string, float> SpawnWeights { get; set; }
        public List<string> SpawnableEnemies => SpawnWeights.Keys.ToList();
    }
}
