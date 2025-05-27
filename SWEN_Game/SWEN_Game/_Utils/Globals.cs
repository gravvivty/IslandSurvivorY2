using System;
using System.Collections.Generic;
using LDtk;
using LDtkTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Ui;

namespace SWEN_Game
{
    /// <summary>
    /// Global Variables like Content,Spritebatch,Time,etc.
    /// </summary>
    public static class Globals
    {
        public static float Time { get; set; }

        // Access everything you want e.g.
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
        public static UiSystem UiSystem { get; set; }
        public static float SoundVolume { get; set; }
        public static float MusicVolume { get; set; }

        public static void UpdateTime(GameTime gameTime)
        {
            Time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static void CalculateAllCollisions()
        {
            var level0 = World.Levels[0];
            var collisionLayer = level0.LayerInstances[1];

            if (collisionLayer != null)
            {
                int gridSize = collisionLayer._GridSize;

                // Number of Cells in a row
                int gridCellWidth = collisionLayer._CWid;
                int cells = 0;
                foreach (var element in collisionLayer.IntGridCsv)
                {
                    if (collisionLayer.IntGridCsv[cells] == 1)
                    {
                        // e.g. x = 52 % 75 -> 52 * 16
                        // e.g. y = 6 / 75 -> 6 * 16
                        // --> loops around cuz math
                        int x = (cells % gridCellWidth) * gridSize; // <-- WorldX/Y needed for offset
                        int y = (cells / gridCellWidth) * gridSize;

                        // Add to Global Collisions List
                        Collisions.Add(new Rectangle(x, y, gridSize, gridSize));
                    }

                    cells++;
                }
            }
        }

        public static void CalculateAllHitboxes()
        {
            var level0 = World.Levels[0];
            var hitboxLayer = level0.LayerInstances[0];

            if (hitboxLayer != null)
            {
                int gridSize = hitboxLayer._GridSize;

                // Number of Cells in a row
                int gridCellWidth = hitboxLayer._CWid;
                int cells = 0;
                foreach (var element in hitboxLayer.IntGridCsv)
                {
                    if (hitboxLayer.IntGridCsv[cells] == 1)
                    {
                        // e.g. x = 52 % 75 -> 52 * 16
                        // e.g. y = 6 / 75 -> 6 * 16
                        // --> loops around cuz math
                        int x = (cells % gridCellWidth) * gridSize; // <-- WorldX/Y needed for offset
                        int y = (cells / gridCellWidth) * gridSize;

                        // Add to Global Hotboxes List
                        Hitboxes.Add(new Rectangle(x, y, gridSize, gridSize));
                    }

                    cells++;
                }
            }
        }

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

        public static void SetZoom(int newZoom)
        {
            Zoom = newZoom;
        }
    }
}