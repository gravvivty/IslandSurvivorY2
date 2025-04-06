using System;
using System.Collections.Generic;
using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SWEN_Game
{
    public class Renderer
    {
        private readonly float DepthRadius = 120f;
        private Player _player;
        private SpriteManager _spriteManager;
        private SpriteCalculator _spriteCalculator;

        public Renderer(Player player, SpriteManager spriteManager, SpriteCalculator spriteCalculator)
        {
            _player = player;
            _spriteManager = spriteManager;
            _spriteCalculator = spriteCalculator;
        }
    }
}
