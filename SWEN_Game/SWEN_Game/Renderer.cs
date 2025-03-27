using System;
using LDtk;
using LDtk.Renderer;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public class Renderer
    {
        private ExampleRenderer _renderer;
        private Player _player;
        public float zoom { get; private set; } = 4f;

        public Renderer(Player player)
        {
            _player = player;
            _renderer = new ExampleRenderer(Globals.SpriteBatch, Globals.Content);
            // preRender the whole world - each level
            foreach (LDtkLevel level in Globals.World.Levels)
            {
                _renderer.PrerenderLevel(level);
            }
        }

        public void drawWorld()
        {
            Globals.SpriteBatch.Begin(transformMatrix: calcTranslation(), samplerState: SamplerState.PointClamp);
            var level = Globals.World.Levels[0];
            _renderer.RenderPrerenderedLevel(level);
            Globals.SpriteBatch.Draw(_player.texture, _player.position, Color.White);

            // Draw collisions and player collision
            Rectangle entityRect = new Rectangle((int)_player.position.X + _player.texture.Width / 2 - 2, 
                (int)_player.position.Y + _player.texture.Height-5, _player.texture.Width / 4, _player.texture.Height / 10);
            foreach (var rect in Globals.Collisions)
            {
                Globals.SpriteBatch.Draw(_player.texture, rect, Color.Red);
            }
            Globals.SpriteBatch.Draw(_player.texture, entityRect, Color.Green);

            Globals.SpriteBatch.End();
        }

        public void SetZoom(float newZoom)
        {
            zoom = newZoom;
        }
        private Matrix calcTranslation()
        {
            Matrix translation;
            translation = Matrix.CreateTranslation(-_player.position.X, -_player.position.Y, 0) *
                Matrix.CreateScale(this.zoom, this.zoom, 1f) *
                Matrix.CreateTranslation(Globals.Graphics.PreferredBackBufferWidth / 2f - _player.texture.Width*3,
                Globals.Graphics.PreferredBackBufferHeight / 2f - _player.texture.Height, 0);
            return translation;
        }
    }
}

