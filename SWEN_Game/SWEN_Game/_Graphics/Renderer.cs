using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game._Utils;
using SWEN_Game._Interfaces;

namespace SWEN_Game._Graphics
{
    public class Renderer
    {
        private static readonly float DepthRadius = 500f;
        private IPlayer _player;
        private SpriteManager _spriteManager;
        private SpriteCalculator _spriteCalculator;

        public Renderer(IPlayer player, SpriteManager spriteManager, SpriteCalculator spriteCalculator)
        {
            _player = player;
            _spriteManager = spriteManager;
            _spriteCalculator = spriteCalculator;
        }

        /// <summary>
        /// Draws the entire game world including background tiles and sprite groups.
        /// </summary>
        /// <remarks>
        /// This method precomputes anchor depths for sprite groups near the player, applies the camera transformation,
        /// and renders background layers, grouped tiles with shared depth based on their anchor tile, as well as other game entities.
        /// </remarks>
        public void DrawWorld()
        {
            // Precompute anchor depths for sprite groups within a specific radius around the player.
            // This groups tiles by their EnumTag and uses the nearest anchor tile's Y value for consistent depth.
            var anchorDepths = _spriteCalculator.SpriteGroupAnchorCalculation(DepthRadius);

            // Get the current level and mapping between EnumTags and tile IDs.
            var level = Globals.World.Levels[0];
            var tileMappings = _spriteManager.GetTileMappings();

            // Process each layer in the level.
            foreach (var layer in level.LayerInstances)
            {
                bool isBackground = layer._Identifier == "Background";

                // Skip layers without an associated tileset texture.
                if (layer._TilesetRelPath == null)
                {
                    continue;
                }

                // Retrieve the texture for this tileset.
                Texture2D tilesetTexture = _spriteManager.GetTilesetTextureFromRenderer(level, layer._TilesetRelPath);
                float renderRadius = 1000f; // 1000px in all directions going out from the player in the middle
                Vector2 playerPos = _player.RealPos;

                // Process each tile in the current layer.
                foreach (var tile in layer.GridTiles)
                {
                    // Calculate the world position of the tile by applying layer offsets.
                    Vector2 position = new(tile.Px.X + layer._PxTotalOffsetX, tile.Px.Y + layer._PxTotalOffsetY);
                    Rectangle srcRect = new(tile.Src.X, tile.Src.Y, layer._GridSize, layer._GridSize);

                    if (Vector2.DistanceSquared(position, playerPos) > renderRadius * renderRadius)
                    {
                        continue;
                    }

                    // For background layers, draw with a forced depth of 0 (ensuring they render behind all other tiles).
                    if (isBackground)
                    {
                        DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, layer);
                        continue;
                    }

                    // Determine if the tile belongs to a sprite group by checking its tile ID in the mappings.
                    string foundEnumTag = null;
                    foreach (var (enumTag, ids) in tileMappings)
                    {
                        if (ids.Contains(tile.T))
                        {
                            foundEnumTag = enumTag;
                            break;
                        }
                    }

                    // If the tile is part of a sprite group and an anchor depth has been computed for that group,
                    // use the group's anchor depth for all its tiles. Otherwise, calculate depth per tile normally.
                    if (!string.IsNullOrEmpty(foundEnumTag) &&
                        anchorDepths.TryGetValue(foundEnumTag, out float anchorDepth))
                    {
                        DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, anchorDepth, layer);
                    }
                    else
                    {
                        DrawTile(Globals.SpriteBatch, tilesetTexture, srcRect, position, layer);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the Camera with Mouse offset for the game.
        /// </summary>
        /// <returns>Matrix that corresponds with the actual offsets.</returns>
        public Matrix CalcTranslation()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 screenCenter = new Vector2(
                Globals.WindowSize.X / 2f,
                Globals.WindowSize.Y / 2f);

            // Raw mouse offset from the screen center -> cuz character is center of screen
            Vector2 rawMouseOffset = new Vector2(mouseState.X, mouseState.Y) - screenCenter;

            Vector2 maxCameraOffset = new Vector2(60f, 60f);
            Vector2 maxMouseRange = new Vector2(
                Globals.WindowSize.X,
                Globals.WindowSize.Y); // Mouse can affect camera within this range

            // Scales the Offset down - 0->maxMouseRange gets scaled to 0->maxCameraOffset
            // Ensures Camera smoothness
            Vector2 mouseOffset = new Vector2(
                rawMouseOffset.X * (maxCameraOffset.X / maxMouseRange.X),
                rawMouseOffset.Y * (maxCameraOffset.Y / maxMouseRange.Y)); // Camera shifts within this range

            // Ensure the final offset never exceeds maxCameraOffset
            if (mouseOffset.Length() > maxCameraOffset.Length())
            {
                mouseOffset.Normalize(); // Keep direction
                mouseOffset = mouseOffset * maxCameraOffset; // Clamp to maxCameraOffset
            }

            Vector2 cameraTarget = _player.RealPos + mouseOffset;
            cameraTarget = new Vector2(
                (float)Math.Floor(cameraTarget.X),
                (float)Math.Floor(cameraTarget.Y));

            return Matrix.CreateTranslation(
                -cameraTarget.X,
                -cameraTarget.Y,
                0) *
                Matrix.CreateScale(Globals.Zoom, Globals.Zoom, 1f) *
                Matrix.CreateTranslation(
                    Globals.Graphics.PreferredBackBufferWidth / 2f,
                    Globals.Graphics.PreferredBackBufferHeight / 2f,
                    0);
        }

        /// <summary>
        /// Draws a single tile using a dynamically calculated depth based on its world position.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render the tile.</param>
        /// <param name="texture">The tileset texture containing the source tile graphic.</param>
        /// <param name="sourceRect">The source rectangle within the texture to draw (tile's appearance).</param>
        /// <param name="position">The world position where the tile should be drawn.</param>
        /// <param name="layer">The layer instance the tile belongs to, used for depth calculation context.</param>
        private void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRect, Vector2 position, LayerInstance layer)
        {
            float depth = _spriteManager.GetDepth(position, sourceRect.Width, layer);
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draws a single tile using a shared anchor depth, typically used for grouped sprite rendering.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render the tile.</param>
        /// <param name="texture">The tileset texture containing the source tile graphic.</param>
        /// <param name="sourceRect">The source rectangle within the texture to draw (tile's appearance).</param>
        /// <param name="position">The world position where the tile should be drawn.</param>
        /// <param name="anchorDepth">The precomputed depth based on an anchor tile shared by the sprite group.</param>
        /// <param name="layer">The layer instance the tile belongs to, used for contextual rendering data.</param>
        private void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRect, Vector2 position, float anchorDepth, LayerInstance layer)
        {
            float depth = _spriteManager.GetDepth(anchorDepth, layer);
            spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }
    }
}
