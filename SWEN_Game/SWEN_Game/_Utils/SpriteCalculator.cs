using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SWEN_Game._Managers;

namespace SWEN_Game._Utils
{
    /// <summary>
    /// Responsible for calculating rendering depth for sprites based on anchor tiles,
    /// helping to correctly layer sprites based on their position relative to the player.
    /// </summary>
    public class SpriteCalculator
    {
        private readonly SpriteManager _spriteManager;
        private readonly IPlayerPos _player;
        private readonly Dictionary<string, int> _anchorTileMap = new Dictionary<string, int>
        {
            { "BigCleanHouse", 413 },
            { "BigMossHouse", 377 },
            { "ShackMoss", 92 },
            { "FrogStatue", 249 },
            { "MonkStatue", 160 },
            { "SmallHouseOrange", 85 },
            { "Cart", 330 },
            { "SmallHouseCrashed", 89 },
            { "GiantLog", 447 },
            { "SingleTree", 284 },
            { "GroupTree", 321 },
            { "Ruins", 178 },
            { "Pillar", 202 },
            { "SmallPillar", 163 },
            { "HouseRuins", 81 },
            { "Walls", 410 },
            { "Bookshelf", 458 },
            { "EmptyShelf", 459 },
            { "Drawer", 457 },
            { "RoundShelf", 456 },
            { "Pool", 348 },
            { "SmallGreenHouseDesert", 101 },
            { "BigGreenHouseDesert", 345 },
            { "MarketStandDesert", 462 },
            { "SmallRectangleHouseDesert", 107 },
            { "BigRectangleHouseDesert", 267 },
            { "CastleTower", 395 },
            { "SingleTreeDesert", 472 },
            { "GroupTreeDesert", 391 },
            { "Watchtower", 175 },
            { "BushDesert", 468 },
            { "LionStatue", 460 },
        };

        public SpriteCalculator(SpriteManager spriteManager, IPlayerPos player)
        {
            _spriteManager = spriteManager;
            _player = player;
        }

        /// <summary>
        /// Calculates the rendering depth for sprite groups based on the position of their anchor tiles.
        /// </summary>
        /// <param name="radius">The radius within which to consider anchor tiles for depth calculation.</param>
        /// <returns>
        /// A dictionary mapping each sprite group identifier (EnumTag) to its computed depth value.
        /// </returns>
        /// <remarks>
        /// For each sprite group, this function identifies the designated anchor tile (using GetAnchorTileID) and selects the instance
        /// closest to the player's position. If this instance is within the specified radius, its depth (based on its Y coordinate) is used
        /// for the entire group, ensuring consistent layer ordering.
        /// </remarks>
        public Dictionary<string, float> SpriteGroupAnchorCalculation(float radius)
        {
            // Dictionary to store computed depth for each sprite group (keyed by EnumTag).
            var result = new Dictionary<string, float>();

            // Retrieve all tile groups categorized by their EnumTag.
            var tileGroups = _spriteManager.GetTileGroups();

            // Iterate over each sprite group.
            foreach (var (enumTag, group) in tileGroups)
            {
                // Get the designated anchor tile ID for this group.
                int anchorID = GetAnchorTileID(enumTag);

                // If there is no valid anchor or the group doesn't contain the anchor tile, skip this group.
                if (anchorID == 0 || !group.ContainsKey(anchorID))
                {
                    continue;
                }

                float minDist = float.MaxValue;
                Vector2 bestAnchorPos = Vector2.Zero;

                // Find the anchor tile occurrence that is closest to the player.
                foreach (var anchorPos in group[anchorID])
                {
                    float dist = Vector2.Distance(anchorPos, _player.RealPos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        bestAnchorPos = anchorPos;
                    }

                    // Draw Anchor Tiles
                    /*Globals.SpriteBatch.Draw(_player.Texture, new Rectangle((int)anchorPos.X, (int)anchorPos.Y, 16, 16), null, Color.Blue, 0f, new Vector2(0, 0),
                SpriteEffects.None, 1f);*/
                }

                // If the closest anchor is within the specified radius, compute its depth and assign it to the sprite group.
                if (minDist <= radius)
                {
                    float anchorDepth = _spriteManager.GetDepth(bestAnchorPos, 16f);
                    result[enumTag] = anchorDepth;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the anchor tile ID corresponding to a sprite group identifier.
        /// The anchor tile represents the "bottom-most" tile used for depth sorting.
        /// </summary>
        /// <param name="enumName">The sprite group identifier (EnumTag).</param>
        /// <returns>The tile ID used as anchor, or 0 if none.</returns>
        public int GetAnchorTileID(string enumName)
        {
            if (_anchorTileMap.TryGetValue(enumName, out int anchorID))
            {
                return anchorID;
            }

            return 0; // Default if not found
        }
    }
}
