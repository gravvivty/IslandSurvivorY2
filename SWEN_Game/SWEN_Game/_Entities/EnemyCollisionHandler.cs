using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SWEN_Game._Entities.Enemies;
using SWEN_Game._Shooting;

namespace SWEN_Game._Entities
{
    public class EnemyCollisionHandler
    {
        private Player _player;

        public EnemyCollisionHandler(Player player)
        {
            _player = player;
        }

        public void CheckCollisions(List<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                if (!_player.GetIsInvincible() && enemy.Hitbox.Intersects(_player.Hitbox))
                {
                    _player.TakeDamage(enemy.EnemyDamage);
                }

                if (enemy is Witch witch)
                {
                    foreach (var bullet in witch.GetBullets())
                    {
                        if (bullet.IsVisible && bullet.BulletHitbox.Intersects(_player.Hitbox) && !_player.GetIsInvincible())
                        {
                            _player.TakeDamage((int)bullet.Damage);
                            bullet.IsVisible = false;
                        }
                    }
                }
            }
        }
    }
}