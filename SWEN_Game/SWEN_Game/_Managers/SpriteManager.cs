using System;
using System.Collections.Generic;
using Assimp;
using LDtk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game._Utils;

namespace SWEN_Game._Managers
{
    public class SpriteManager
    {
        private Dictionary<string, List<int>> tileMappings = new Dictionary<string, List<int>>();
        private float maxY = 2000f;
        private Dictionary<string, Dictionary<int, List<Vector2>>> tileGroups = new();

        /*
        "House": {
            231: [ (100, 200), (150, 250) ],
            432: [ (300, 400) ] }
        ,
        "Tree": {
            512: [ (50, 50), (75, 125) ]
        }*/

        // How TileGroups looks like --> so now we can go through this -->
        // since we have a radius around the player we should check idk 64px
        // so now we can go through this collection and see which anchorTileID is closest to the tile that just came into our radius
        public SpriteManager()
        {
           // dummy
        }

        /// <summary>
        /// Loads a tileset texture associated with a level and a relative path.
        /// </summary>
        /// <param name="level">The LDtk level containing the tileset.</param>
        /// <param name="tilesetPath">The relative path to the tileset texture file.</param>
        /// <returns>A loaded <see cref="Texture2D"/> representing the tileset.</returns>
        public Texture2D GetTilesetTextureFromRenderer(LDtkLevel level, string tilesetPath)
        {
            if (Globals.Content == null)
            {
                string directory = System.IO.Path.GetDirectoryName(level.WorldFilePath) !;
                string assetName = System.IO.Path.Join(directory, tilesetPath);
                return Texture2D.FromFile(Globals.Graphics.GraphicsDevice, assetName);
            }
            else
            {
                string file = System.IO.Path.ChangeExtension(tilesetPath, null);
                string directory = System.IO.Path.GetDirectoryName(level.WorldFilePath) !;
                string assetName = System.IO.Path.Join(directory, file);
                return Globals.Content.Load<Texture2D>(assetName);
            }
        }

        /// <summary>
        /// Retrieves the dictionary of grouped tile positions by enum tag and tile ID.
        /// </summary>
        /// <returns>
        /// A dictionary where each key is an enum tag (e.g., "House"), and the value is another dictionary mapping tile IDs to their world positions.
        /// </returns>
        public Dictionary<string, Dictionary<int, List<Vector2>>> GetTileGroups()
        {
            return tileGroups;
        }

        /// <summary>
        /// Retrieves the dictionary mapping enum tags to associated tile IDs.
        /// </summary>
        /// <returns>
        /// A dictionary where each key is an enum tag (e.g., "Tree") and the value is a list of tile IDs corresponding to that tag.
        /// </returns>
        public Dictionary<string, List<int>> GetTileMappings()
        {
            return tileMappings;
        }

        /// <summary>
        /// Computes a depth value for rendering based on the tile's position and layer configuration.
        /// </summary>
        /// <param name="position">The world position of the tile.</param>
        /// <param name="spriteHeight">The height of the sprite in pixels.</param>
        /// <param name="layer">The layer instance to which the tile belongs.</param>
        /// <returns>A normalized float value representing the depth, where lower values are rendered first (further back).</returns>
        public float GetDepth(Vector2 position, float spriteHeight, LayerInstance layer)
        {
            float depth = (position.Y + spriteHeight) / maxY;
            float layerOffset = 0;

            // low layerOffset --> Foreground
            switch (layer._Identifier)
            {
                case "Deco_BigBackground":
                    layerOffset = 4;
                    break;
                case "Deco_BigMiddleground":
                    layerOffset = 3;
                    break;
                case "Deco_BigForeground":
                    layerOffset = 2;
                    break;
                case "Deco_Small":
                    depth = 0.00015f;
                    break;
                case "Deco_Small2":
                    depth = 0.00015f;
                    break;
                case "Deco_SmallDesert":
                    depth = 0.0015f;
                    break;
                case "Deco_Background":
                    depth = 0.0000001f;
                    break;
                case "Deco_Background2":
                    depth = 0.0000001f;
                    break;
                case "Background":
                    depth = 0f;
                    break;
                case "Background2":
                    depth = 0.0005f;
                    break;
            }

            return depth - (layerOffset / maxY);
        }

        /// <summary>
        /// Computes a general depth value for a sprite without considering specific layer offsets.
        /// </summary>
        /// <param name="position">The world position of the sprite.</param>
        /// <param name="spriteHeight">The height of the sprite in pixels.</param>
        /// <returns>A normalized float value representing the depth.</returns>
        public float GetDepth(Vector2 position, float spriteHeight)
        {
            // Using the bottom of the sprite as the reference
            float depth = (position.Y + spriteHeight) / maxY;
            return depth;
        }

        /// <summary>
        /// Adjusts a base depth value using a layer-specific offset. Used for grouped sprite layers with anchor depths.
        /// </summary>
        /// <param name="depth">The base depth (often from an anchor sprite).</param>
        /// <param name="layer">The layer to which the sprite belongs, determining the layer offset.</param>
        /// <returns>A normalized float value adjusted for the layer's depth offset.</returns>
        public float GetDepth(float depth, LayerInstance layer)
        {
            float layerOffset = 0;
            switch (layer._Identifier)
            {
                case "Deco_BigBackground":
                    layerOffset = 8;
                    break;
                case "Deco_BigMiddleground":
                    layerOffset = 6;
                    break;
                case "Deco_BigForeground":
                    layerOffset = 4;
                    break;
            }

            return depth - (layerOffset / maxY);
        }

        /// <summary>
        /// Maps tile IDs to their corresponding enum tags and stores their world positions in organized structures.
        /// </summary>
        /// <remarks>
        /// Populates <c>tileMappings</c> and <c>tileGroups</c> dictionaries based on the LDtk enum-tagged tiles across all levels.
        /// </remarks>
        public void MapTileToTexture()
        {
            Vector2 invalid = new Vector2(-1, -1);

            // Go through all Enum Values
            foreach (var item in Globals.File.Defs.Enums[0].Values)
            {
                // e.g. House, Tree_Big, etc.
                string enumID = item.Id;

                // Go through all tilesets
                foreach (var tileset in Globals.File.Defs.Tilesets)
                {
                    if (tileset.EnumTags != null)
                    {
                        // Every EnumTag
                        foreach (var enumTag in tileset.EnumTags)
                        {
                            // if Tileset contains Enum that exists
                            if (enumTag.EnumValueId == enumID)
                            {
                                // New Key if it doesn't exist yet (e.g. "House")
                                if (!tileMappings.ContainsKey(enumTag.EnumValueId))
                                {
                                    tileMappings[enumTag.EnumValueId] = new List<int>();
                                    tileGroups[enumTag.EnumValueId] = new Dictionary<int, List<Vector2>>();
                                }

                                // Add all tileIDs to the correct enum
                                tileMappings[enumTag.EnumValueId].AddRange(enumTag.TileIds);

                                // Get correct WorldPos
                                foreach (var tileID in enumTag.TileIds)
                                {
                                    List<Vector2> tilePositions = GetTileWorldPosition(tileID, enumTag.EnumValueId);

                                    // New Key if (e.g. "House", 130 doesnt exist)
                                    if (!tileGroups[enumTag.EnumValueId].ContainsKey(tileID))
                                    {
                                        tileGroups[enumTag.EnumValueId][tileID] = new List<Vector2>();
                                    }

                                    foreach (var pos in tilePositions)
                                    {
                                        tileGroups[enumTag.EnumValueId][tileID].Add(pos);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all world positions for a given tile ID and enum tag across all levels and layers.
        /// </summary>
        /// <param name="tileID">The tile ID to locate.</param>
        /// <param name="enumTag">The enum tag category the tile belongs to (e.g., "House").</param>
        /// <returns>A list of <see cref="Vector2"/> positions where the tile appears in the world.</returns>
        private List<Vector2> GetTileWorldPosition(int tileID, string enumTag)
        {
            List<Vector2> positions = new List<Vector2>();

            // Go through all levels
            foreach (var level in Globals.World.Levels)
            {
                // Go through all layers
                foreach (var layer in level.LayerInstances)
                {
                    // Check if it is a Tiles layer
                    if (layer._Type == LayerType.Tiles)
                    {
                        // Go through all gridTiles
                        foreach (var gridTile in layer.GridTiles)
                        {
                            // Check if tileIDs match
                            if (gridTile.T == tileID)
                            {
                                // If Tile with EnumTag, ID and same coords exists --> dont return - iterate again
                                Vector2 tilePos = new Vector2(gridTile.Px.X, gridTile.Px.Y);
                                positions.Add(tilePos);
                            }
                        }
                    }
                }
            }

            // No matching tile with ID
            return positions;
        }
    }
}
