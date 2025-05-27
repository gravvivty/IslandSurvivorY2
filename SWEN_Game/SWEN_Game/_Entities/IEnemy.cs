using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SWEN_Game
{
    public interface IEnemy
    {
        Vector2 Position { get; }
        Rectangle Hitbox { get; set; }
        bool IsAlive { get; }
        float CurrentHealth { get; set; }
        Texture2D Texture { get; set; }
        int EnemyDamage { get; set; }
        float EnemySpeed { get; set; }
        int FrameSize {  get; set; }
        AnimationManager AnimationManager { get; set; }
        int DamageFlashFrames { get; set; }
        int DamageFlashTimer { get; set; }

        void Update(List<Bullet> bulletList, Vector2 playerPostion);
        bool GotHitByBullet(List<Bullet> bulletList);
        void Draw();
        void TakeDamage(float amount);
    }
}
