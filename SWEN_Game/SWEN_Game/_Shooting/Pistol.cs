using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public class Pistol : Weapon
    {
        public Pistol(Texture2D bulletTexture, Vector2 position)
            : base(bulletTexture, position)
        {
            FireCooldown = 0.3f;
        }

        public override void Update()
        {
            float gametime = Globals.Time;
            TimeSinceLastShot += (float)gametime;

            foreach (var bullet in Bullets)
            {
                bullet.Update();
            }

            Bullets.RemoveAll(b => !b.IsVisible);
        }

        public override void Shoot(Vector2 direction, Vector2 player_position)
        {
            System.Diagnostics.Debug.WriteLine("Pistol is now Trying to shoot" + DateTime.Now);
            if (TimeSinceLastShot >= FireCooldown)
            {
                Bullets.Add(new Bullet(BulletTexture, player_position, direction));
                System.Diagnostics.Debug.WriteLine("Pistol is now shooting" + DateTime.Now);
                TimeSinceLastShot = 0f;
            }
        }
    }
}
