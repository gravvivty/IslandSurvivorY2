using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Anims;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities.Enemies
{
    public class Shark : Enemy
    {
        public Shark(Vector2 startPosition)
        {
            Position = startPosition;
            XPReward = 65;
            CurrentHealth = 125f;
            EnemyDamage = 1;
            EnemySpeed = 80f;
            FrameWidth = 24;
            FrameHeight = 24;
            Texture = Globals.Content.Load<Texture2D>("Sprites/Entities/Enemies/Tiburones");

            Animation walkLeft = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 1);
            Animation walkRight = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 2);

            this.AnimationManager = new AnimationManager();
            this.AnimationManager.AddAnimation("WalkLeft", walkLeft);
            this.AnimationManager.AddAnimation("WalkRight", walkRight);
        }

        protected override void UpdateHitbox()
        {
            float biggerWidth = FrameWidth / 1.5f;
            float biggerHeight = FrameHeight / 2f;

            Hitbox = new Rectangle(
                (int)(Position.X + FrameWidth / 4f),
                (int)(Position.Y + FrameHeight / 3f),
                (int)biggerWidth,
                (int)biggerHeight);
        }
    }
}
