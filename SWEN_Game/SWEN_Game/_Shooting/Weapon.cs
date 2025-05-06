using System;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SWEN_Game
{
    public abstract class Weapon
    {
        private List<Bullet> _bullets = new List<Bullet>();
        private float _fireCooldown;
        private float _timeSinceLastShot;
        private Texture2D _bulletTexture;
        private Vector2 _position = new Vector2(100,100);

        protected float FireCooldown
        {
            get => _fireCooldown;
            set => _fireCooldown = value;
        }

        protected float TimeSinceLastShot
        {
            get => _timeSinceLastShot;
            set => _timeSinceLastShot = value;
        }

        protected Texture2D BulletTexture => _bulletTexture;
        protected Vector2 Position => _position;
        protected List<Bullet> Bullets => _bullets;

        public Weapon(Texture2D bulletTexture, Vector2 position)
        {
            _bulletTexture = bulletTexture;
            _position = position;
        }

        public abstract void Update();
        public abstract void Shoot(Vector2 direction, Vector2 player_position);

        public List<Bullet> GetBullets() => _bullets;
    }
}
