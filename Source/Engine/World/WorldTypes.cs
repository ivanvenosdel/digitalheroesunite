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
        private static Texture2D tileTexture;

        public static Dictionary<int, TileType> TileTypes = new Dictionary<int, TileType>();
        public static Dictionary<int, LevelMap> Levels = new Dictionary<int, LevelMap>();

        public static Texture2D TileTexture { get { return tileTexture; } set { tileTexture = value; } }

        public static void Initialize()
        {
            TileType[] tile = UtilityGame.DeserializeFromXML<TileType[]>("World/tiles.xml");
            for (int i = 0; i < tile.Length; ++i)
	        {
                TileTypes.Add(tile[i].ID, tile[i]);
	        }

            string[] levels = Directory.GetFiles("Content/World/Levels");
            for (int i = 0; i < levels.Length; ++i)
            {
                //Strip the content directory 
                levels[0] = levels[0].Substring(levels[0].IndexOf("/") + 1);

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
