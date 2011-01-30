using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Utilities;

namespace Engine.World
{
    public static class WorldTypes
    {
        public const int TILE_SIZE = 40;

        private static Texture2D tileTexture;

        public static Dictionary<int, TileType> TileTypes = new Dictionary<int, TileType>();
        public static Dictionary<int, Dictionary<int, TileExtra>> LevelTileExtras = new Dictionary<int, Dictionary<int, TileExtra>>();
        public static Dictionary<int, LevelMap> Levels = new Dictionary<int, LevelMap>();

        public static Texture2D TileTexture { get { return tileTexture; } set { tileTexture = value; } }

        public static void Initialize()
        {
            TileType[] tiles = UtilityGame.DeserializeFromXML<TileType[]>("World/tiles.xml");
            foreach (TileType tile in tiles)
            {
                TileTypes.Add(tile.ID, tile);
            }

            LevelTileExtras[] allLevelTileExtras = UtilityGame.DeserializeFromXML<LevelTileExtras[]>("World/tileExtras.xml");
            foreach (LevelTileExtras levelExtras in allLevelTileExtras)
            {
                Dictionary<int, TileExtra> tileExtras = new Dictionary<int, TileExtra>();
                foreach (TileExtra extra in levelExtras.TileExtras)
                {
                    tileExtras.Add(extra.TileID, extra);
                }
                LevelTileExtras.Add(levelExtras.LevelID, tileExtras);
            }

            string[] levels = Directory.GetFiles("Content/World/Levels");
            for (int i = 0; i < levels.Length; ++i)
            {
                //Strip the content directory 
                levels[i] = levels[i].Substring(levels[i].IndexOf("/") + 1);

                LevelMap level = UtilityGame.DeserializeFromXML<LevelMap>(levels[i]);
                Levels.Add(level.Level, level);
            }
        }

        public static void LoadContent()
        {
            tileTexture = DeviceManager.Instance.Content.Load<Texture2D>("World\\tiles");
        }
    }
}
