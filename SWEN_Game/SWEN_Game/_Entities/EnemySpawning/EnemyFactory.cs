using Microsoft.Xna.Framework;
using SWEN_Game._Entities.Enemies;

namespace SWEN_Game._Entities.EnemySpawning
{
    public class EnemyFactory : IEnemyFactory
    {
        public Enemy CreateEnemy(string type, Vector2 pos)
        {
            return type switch
            {
                "Mummy" => new Mummy(pos),
                "Shark" => new Shark(pos),
                "Shroom" => new Shroom(pos),
                "Baumbart" => new Baumbart(pos),
                "Witch" => new Witch(pos),
                "Ekek" => new Ekek(pos),
                "Demon" => new Demon(pos),
                "Siren" => new Siren(pos),
                "Slime" => new Slime(pos),
                "SlimeBoss" => new SlimeBoss(pos),
                "Reaper" => new Reaper(pos),
                _ => throw new ArgumentException($"Unknown type: {type}")
            };
        }
    }
}
