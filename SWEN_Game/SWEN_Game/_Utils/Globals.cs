using System;
using System.Collections.Generic;
using LDtk;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SWEN_Game._Managers;

namespace SWEN_Game._Utils
{
    /// <summary>
    /// Holds global references and utilities used across the game, such as content,
    /// rendering tools, world data, collision grids, and game timing.
    /// </summary>
    public static class Globals
    {
        public static float Time { get; set; }
        public static float TotalGameTime { get; private set; }
        public static ContentManager Content { get; set; }
        public static GraphicsDeviceManager Graphics { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static SpriteManager SpriteManager { get; set; }
        public static Point WindowSize { get; set; } = new Point(1920, 1080); // Default size
        public static LDtkFile File { get; set; }
        public static LDtkWorld World { get; set; }
        public static List<Rectangle> Hitboxes { get; set; } = new List<Rectangle>();
        public static List<Rectangle> Collisions { get; set; } = new List<Rectangle>();
        public static int Zoom { get; private set; } = 4;
        public static bool Fullscreen { get; set; } = false;
        public static bool Borderless { get; set; } = false;

        /// <summary>
        /// Updates the time delta and total game time using the provided GameTime.
        /// </summary>
        /// <param name="gameTime">The GameTime from the game loop.</param>
        public static void UpdateTime(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TotalGameTime += Time;
        }

        /// <summary>
        /// Calculates collision rectangles from the collision layer of the LDtk world.
        /// Populates the <see cref="Collisions"/> list.
        /// </summary>
        public static void CalculateAllCollisions()
        {
            var level0 = World.Levels[0];
            CalculateLayerRects(level0.LayerInstances[1], Collisions);
        }

        public static void CalculateAllHitboxes()
        {
            var level0 = World.Levels[0];
            CalculateLayerRects(level0.LayerInstances[0], Hitboxes);
        }

        /// <summary>
        /// Checks if the given rectangle collides with any world collision rectangle.
        /// </summary>
        /// <param name="entityRect">The rectangle to test.</param>
        /// <returns>True if a collision occurs, otherwise false.</returns>
        public static bool IsColliding(Rectangle entityRect)
        {
            // Assumes entity collision as small rectangle at the very bottom of the Sprite

            foreach (var rect in Collisions)
            {
                if (entityRect.Intersects(rect))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the given rectangle collides with any hitbox rectangle.
        /// </summary>
        /// <param name="entityRect">The rectangle to test.</param>
        /// <returns>True if a hitbox is intersected, otherwise false.</returns>
        public static bool IsCollidingHitbox(Rectangle entityRect)
        {
            // Assumes entity collision as small rectangle at the very bottom of the Sprite

            foreach (var rect in Hitboxes)
            {
                if (entityRect.Intersects(rect))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets a new zoom level for the game view.
        /// </summary>
        /// <param name="newZoom">The new zoom level to apply.</param>
        public static void SetZoom(int newZoom)
        {
            Zoom = newZoom;
        }

        private static void CalculateLayerRects(LayerInstance layer, List<Rectangle> outputList)
        {
            if (layer == null)
            {
                return;
            }

            int gridSize = layer._GridSize;
            int gridCellWidth = layer._CWid;

            for (int i = 0; i < layer.IntGridCsv.Count(); i++)
            {
                if (layer.IntGridCsv[i] == 1)
                {
                    int x = (i % gridCellWidth) * gridSize;
                    int y = (i / gridCellWidth) * gridSize;
                    outputList.Add(new Rectangle(x, y, gridSize, gridSize));
                }
            }
        }
    }
}