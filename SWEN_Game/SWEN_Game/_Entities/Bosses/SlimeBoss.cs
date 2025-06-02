using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Assimp.Unmanaged;
using SharpFont.Cache;
using SWEN_Game._Anims;
using SWEN_Game._Utils;
using SWEN_Game._Shooting;
using SWEN_Game._Shooting._Modifiers;

namespace SWEN_Game._Entities.Enemies
{
    public class SlimeBoss : Enemy
    {
        private float _spawnCooldown = 10f;
        private float _timeSinceLastSpawn = 0f;
        private Random _rand = new Random();

        public SlimeBoss(Vector2 startPosition)
        {
            Position = startPosition;
            CurrentHealth = 1000f;
            EnemyDamage = 2;
            EnemySpeed = 50f;
            FrameWidth = 48;
            FrameHeight = 64;
            Texture = Globals.Content.Load<Texture2D>("Sprites/Entities/Bosses/slime_king");

            Animation walkLeft = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 1);
            Animation walkRight = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 2);

            this.AnimationManager = new AnimationManager();
            this.AnimationManager.AddAnimation("WalkLeft", walkLeft);
            this.AnimationManager.AddAnimation("WalkRight", walkRight);
        }

        public override void UpdateCustomBehavior(EnemyManager enemyManager)
        {
            _timeSinceLastSpawn += Globals.Time;

            if (_timeSinceLastSpawn >= _spawnCooldown)
            {
                _timeSinceLastSpawn = 0f;

                for (int i = 0; i < 8; i++)
                {
                    float angle = MathHelper.TwoPi / 8 * i;
                    float radius = 50f;

                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                    Vector2 spawnPos = Position + offset;

                    enemyManager.QueueEnemy(new Slime(spawnPos));
                }
            }
        }
    }
}
