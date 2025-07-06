using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Anims;
using SWEN_Game._Utils;

namespace SWEN_Game._Entities.Enemies
{
    public class Slime : Enemy
    {
        public Slime(Vector2 startPosition)
        {
            Position = startPosition;
            XPReward = 15;
            CurrentHealth = 35f;
            EnemyDamage = 1;
            EnemySpeed = 100f;
            FrameWidth = 16;
            FrameHeight = 16;
            Texture = Globals.Content.Load<Texture2D>("Sprites/Entities/Enemies/Slime");

            Animation walkLeft = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 1);
            Animation walkRight = new Animation(Texture, 1, 3, 0.2f, FrameWidth, FrameHeight, 2);

            this.AnimationManager = new AnimationManager();
            this.AnimationManager.AddAnimation("WalkLeft", walkLeft);
            this.AnimationManager.AddAnimation("WalkRight", walkRight);
        }

        protected override void UpdateHitbox()
        {
            int biggerWidth = FrameWidth / 3;
            int biggerHeight = FrameHeight / 3;

            Hitbox = new Rectangle(
                (int)(Position.X + FrameWidth / 3f),
                (int)(Position.Y + FrameHeight / 3f),
                biggerWidth,
                biggerHeight);
        }
    }
}
