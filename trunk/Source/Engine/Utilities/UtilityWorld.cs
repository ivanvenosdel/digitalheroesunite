#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Engine.World;
#endregion

namespace Engine.Utilities
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 1.23.2011
    /// Description: World Utilities
    /// </summary>
    public static class UtilityWorld
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Public Methods
        public static Point WorldToGrid(Vector2 worldPosition)
        {
            Point gridPos = Point.Zero;

            gridPos.X = (int)(worldPosition.X + WorldTypes.TILE_SIZE / 2) / WorldTypes.TILE_SIZE;
            gridPos.Y = (int)(worldPosition.Y / WorldTypes.TILE_SIZE);

            return gridPos;
        }

        public static Vector2 GridToWorld(Point gridPosition)
        {
            Vector2 gridPos = Vector2.Zero;
            gridPos.X = (gridPosition.X * WorldTypes.TILE_SIZE) - WorldTypes.TILE_SIZE / 2;
            gridPos.Y = (gridPosition.Y * WorldTypes.TILE_SIZE);
            return gridPos;
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
