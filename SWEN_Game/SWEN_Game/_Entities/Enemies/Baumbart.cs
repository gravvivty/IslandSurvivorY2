using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Anims;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities.Enemies
{
    public class Baumbart : Enemy
    {
        public Baumbart(Vector2 startPosition)
        {
            Position = startPosition;
            XPReward = 170;
            CurrentHealth = 225f;
            EnemyDamage = 2;
            EnemySpeed = 30f;
            FrameWidth = 24;
            FrameHeight = 24;
            Texture = Globals.Content.Load<Texture2D>("Sprites/Entities/Enemies/Kapre");

            Animation walkLeft = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 1);
            Animation walkRight = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 2);

            this.AnimationManager = new AnimationManager();
            this.AnimationManager.AddAnimation("WalkLeft", walkLeft);
            this.AnimationManager.AddAnimation("WalkRight", walkRight);
        }

        protected override void UpdateHitbox()
        {
            float biggerWidth = FrameWidth / 3f;
            float biggerHeight = FrameHeight / 1.5f;

            Hitbox = new Rectangle(
                (int)(Position.X + FrameWidth / 3f),
                (int)(Position.Y + FrameHeight / 4f),
                (int)biggerWidth,
                (int)biggerHeight);
        }
    }
}
