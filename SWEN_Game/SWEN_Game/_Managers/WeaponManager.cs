using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;

namespace SWEN_Game
{
    public class WeaponManager
    {
        private Weapon currentWeapon;

        public WeaponManager(Weapon weapon)
        {
            currentWeapon = weapon;
        }

        public void Update()
        {
            currentWeapon.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bullet in currentWeapon.GetBullets())
            {
                bullet.Draw(spriteBatch);
            }
        }

        public void Shoot(Vector2 direction, Vector2 player_position)
        {
            currentWeapon.Shoot(direction, player_position);
        }

        public void SetWeapon(Weapon newWeapon)
        {
            currentWeapon = newWeapon;
        }

        public Weapon GetWeapon()
        {
            return currentWeapon;
        }
    }
}
