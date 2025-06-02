using Microsoft.Xna.Framework;
using LDtk;
using SWEN_Game;
using Xunit;
using SWEN_Game._Managers;

namespace SWEN_GameTests
{
    public class SpriteManagerTest
    {
        // Test für die Depth-Berechnung mit Position
        [Fact]
        public void GetDepth_WithPosition_ReturnsCorrectValue()
        {
            var manager = new SpriteManager();
            var position = new Vector2(100, 200);
            float spriteHeight = 32f;
            float result = manager.GetDepth(position, spriteHeight);
            Assert.Equal(0.116f, result); // (200 + 32) / maxf = 0.116 (maxf=2000)
        }

        // Test für Background Layer
        [Fact]
        public void GetDepth_WithBackgroundLayer_ReturnsInputValue()
        {
            var manager = new SpriteManager();
            var layer = new LayerInstance { _Identifier = "Background" };
            float inputDepth = 0.0f;
            float result = manager.GetDepth(inputDepth, layer);
            Assert.Equal(inputDepth, result); 
        }
        // Test für normale Layer
        [Fact]
        public void GetDepth_WithNormalLayer_ReturnsAdjustedValue()
        {
            var manager = new SpriteManager();
            var layer = new LayerInstance { _Identifier = "Deco_Big1" };
            float result = manager.GetDepth(0.5f, layer);
            Assert.Equal(0.5, result); 
        }
    }
}