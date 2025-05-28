using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using SWEN_Game;
using SWEN_Game._Managers;
using Xunit;


namespace SWEN_GameTests
    {
        public class AnimationTest
        {
            private readonly Mock<Texture2D> _mockTexture;
            private readonly Mock<SpriteManager> _mockSpriteManager;
            private readonly SpriteBatch _spriteBatch;

            public AnimationTest()
            {
                _mockTexture = new Mock<Texture2D>();
                _mockSpriteManager = new Mock<SpriteManager>();
                _spriteBatch = new SpriteBatch(TestHelpers.CreateGraphicsDevice());
                SWEN_Game._Utils.Globals.SpriteBatch = _spriteBatch;
            }

        public static class TestHelpers
        {
            public static GraphicsDevice CreateGraphicsDevice()
            {
                // Erstellt ein minimales GraphicsDevice für Tests
                var serviceContainer = new GameServiceContainer();
                var graphicsDeviceManager = new GraphicsDeviceManager(new Game());
                return graphicsDeviceManager.GraphicsDevice;
            }
        }
    }
}