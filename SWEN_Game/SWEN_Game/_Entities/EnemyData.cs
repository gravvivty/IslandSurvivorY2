using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SWEN_Game
{
    public class EnemyData
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }
    }
}
