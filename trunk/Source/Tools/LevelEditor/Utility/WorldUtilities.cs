#region Using Statements
using System;

using Microsoft.Xna.Framework;

using MonsterEscape.Graphics.Cameras;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.Utility
{
    /// <summary>
    /// World Related Utility Functions
    /// </summary>
    public static class WorldUtilities
    {
        /// <summary>
        /// Screen space to world space
        /// </summary>
        /// <param name="screen">Screen space coordinate</param>
        /// <returns>World space coordinate</returns>
        public static Vector2 ScreenToWorld(Vector2 screen)
        {
            Vector2 world = new Vector2();

            Vector2 cam = new Vector2(Camera.Instance.View.Translation.X, Camera.Instance.View.Translation.Y);

            world.X = (int)(screen.X - cam.X);
            world.Y = (int)(screen.Y - cam.Y);

            return world;
        }

        /// <summary>
        /// World space to screen space
        /// </summary>
        /// <param name="screen">World space coordinate</param>
        /// <returns>The screen coordinate</returns>
        public static Vector2 WorldToScreen(Vector2 world)
        {
            Vector2 screen = new Vector2();

            Vector2 cam = new Vector2(Camera.Instance.View.Translation.X, Camera.Instance.View.Translation.Y);

            screen.X = (int)(world.X + cam.X);
            screen.Y = (int)(world.Y + cam.Y);

            return screen;
        }

        /// <summary>
        /// World space to world tile
        /// </summary>
        /// <param name="world">World space coordinate</param>
        /// <returns>The nearest Tile</returns>
        public static Point WorldToTile(Vector2 world)
        {
            Point point = new Point();

            point.X = (int)(Math.Round((world.X - TerrainKey.HALF_TILE_SIZE) / (float)TerrainKey.TILE_SIZE));
            point.Y = (int)(Math.Round((world.Y - TerrainKey.HALF_TILE_SIZE) / (float)TerrainKey.TILE_SIZE));

            if (point.X < 0)
                point.X = 0;
            if (point.Y < 0)
                point.Y = 0;
            if (point.X > CurrentLevel.Instance.Width - 1)
                point.X = CurrentLevel.Instance.Width - 1;
            if (point.Y > CurrentLevel.Instance.Height - 1)
                point.Y = CurrentLevel.Instance.Height - 1;
            return point;
        }

        /// <summary>
        /// World Tile to World space (Center of Tile)
        /// </summary>
        /// <param name="screen">World Tile Coordinate</param>
        /// <returns>Screen Point of the Tile's Center</returns>
        public static Vector2 TileToWorld(Point tile)
        {
            Vector2 point = new Vector2(0, 0);
            Vector2 tilePos = new Vector2(tile.X * TerrainKey.TILE_SIZE, tile.Y * TerrainKey.TILE_SIZE);

            point.X = (int)Math.Round(tilePos.X + TerrainKey.HALF_TILE_SIZE);
            point.Y = (int)Math.Round(tilePos.Y + TerrainKey.HALF_TILE_SIZE);

            return point;
        }

        /// <summary>
        /// Screen space to world tile
        /// </summary>
        /// <param name="screen">Screen space coordinate</param>
        /// <returns>The nearest Tile</returns>
        public static Point ScreenToTile(Vector2 screen)
        {
            Point point = new Point();

            Vector2 cam = new Vector2(Camera.Instance.View.Translation.X, Camera.Instance.View.Translation.Y);

            point.X = (int)(Math.Round((screen.X - cam.X - TerrainKey.HALF_TILE_SIZE) / (float)TerrainKey.TILE_SIZE));
            point.Y = (int)(Math.Round((screen.Y - cam.Y - TerrainKey.HALF_TILE_SIZE) / (float)TerrainKey.TILE_SIZE));

            if (point.X < 0)
                point.X = 0;
            if (point.Y < 0)
                point.Y = 0;
            if (point.X > CurrentLevel.Instance.Width - 1)
                point.X = CurrentLevel.Instance.Width - 1;
            if (point.Y > CurrentLevel.Instance.Height - 1)
                point.Y = CurrentLevel.Instance.Height - 1;
            return point;
        }

        /// <summary>
        /// World Tile to Screen space (Center of Tile)
        /// </summary>
        /// <param name="screen">World Tile Coordinate</param>
        /// <returns>Screen Point of the Tile's Center</returns>
        public static Vector2 TileToScreen(Point tile)
        {
            Vector2 point = new Vector2(0, 0);
            Vector2 cam = new Vector2(Camera.Instance.View.Translation.X, Camera.Instance.View.Translation.Y);
            Vector2 tilePos = new Vector2(tile.X * TerrainKey.TILE_SIZE, tile.Y * TerrainKey.TILE_SIZE);

            point.X = (int)Math.Round(tilePos.X - cam.X + TerrainKey.HALF_TILE_SIZE);
            point.Y = (int)Math.Round(tilePos.Y - cam.Y + TerrainKey.HALF_TILE_SIZE);

            return point;
        }

        /// <summary>
        /// Determine if TileA and TileB are within the passed in range
        /// </summary>
        /// <param name="TileA">The first tile</param>
        /// <param name="TileB">The second tile</param>
        /// <param name="rangeX">The range on the x-axis</param>
        /// <param name="rangeY">The range on the y-axis</param>
        /// <returns>In Range?</returns>
        public static bool TileInRange(Point TileA, Point TileB, int rangeX, int rangeY)
        {
            if (Math.Abs(TileA.X - TileB.X) <= rangeX &&
                Math.Abs(TileA.Y - TileB.Y) <= rangeY)
                return true;
            else
                return false;
        }
    }
}
