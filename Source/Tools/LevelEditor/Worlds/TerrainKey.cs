#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Graphics.Backgrounds;
using MonsterEscape.Utility;
using Engine.World;
#endregion

namespace MonsterEscape.Worlds
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 6.12.2010
    /// Description: Map and Terrain Helpers
    /// </summary>
    public static class TerrainKey
    {
        public const int TILE_SIZE = 40;
        public const int HALF_TILE_SIZE = TILE_SIZE / 2;

        public const int SPIRE_OFFSET = 7;

        public static Dictionary<int, World> WorldTypes = new Dictionary<int, World>();
        public static Dictionary<int, TileType> TileTypes = new Dictionary<int, TileType>();
        public static Dictionary<int, SpireType> SpireTypes = new Dictionary<int, SpireType>();
        public static Dictionary<int, SceneryType> SceneryTypes = new Dictionary<int, SceneryType>();
        public static Texture2D[] SceneryTextures = new Texture2D[5];
        public static Background[] Backgrounds = new Background[5];
        public static Texture2D TerrainTexture;
        public static Texture2D SpireTexture;

        /// <summary>
        /// Loads all terrain types and stores them in the TerrainTypes Dictionary
        /// </summary>
        public static void Initialize()
        {
            ////Load World Types
            //World[] world = WinphoneUtilities.DeserializeFromXML<World[]>("Levels/worlds.xml");
            //for (int i = 0; i < world.Length; ++i)
            //{
            //    WorldTypes.Add(world[i].ID, world[i]);
            //}
            World world = new World();
            world.ID = 1;
            world.Levels = 1;
            world.Name = "";
            WorldTypes.Add(world.ID, world);

            //Load Terrain Types
            TileType[] tiles = WinphoneUtilities.DeserializeFromXML<TileType[]>("World/tiles.xml");
            for (int i = 0; i < tiles.Length; ++i)
            {
                TileTypes.Add(tiles[i].ID, tiles[i]);
            }

            //TerrainType[] terrainTypes = WinphoneUtilities.DeserializeFromXML<TerrainType[]>("Levels/terrain.xml");
            //for (int i = 0; i < terrainTypes.Length; ++i)
            //{
            //    TileTypes.Add(terrainTypes[i].ID, terrainTypes[i]);
            //}

            ////Load Spire Types
            //SpireType[] spireTypes = WinphoneUtilities.DeserializeFromXML<SpireType[]>("Levels/spires.xml");
            //for (int i = 0; i < spireTypes.Length; ++i)
            //{
            //    SpireTypes.Add(spireTypes[i].ID, spireTypes[i]);
            //}

            ////Load Scenery Types
            //SceneryType[] sceneryTypes = WinphoneUtilities.DeserializeFromXML<SceneryType[]>("Levels/scenery.xml");
            //for (int i = 0; i < sceneryTypes.Length; ++i)
            //{
            //    SceneryTypes.Add(sceneryTypes[i].ID, sceneryTypes[i]);
            //}

            //Load Textures
            //SceneryTextures[0] = DeviceManager.Instance.ContentManager.Load<Texture2D>("Levels/Scenery/forest");
            //SceneryTextures[1] = DeviceManager.Instance.ContentManager.Load<Texture2D>("Levels/Scenery/sewer");
            //SceneryTextures[2] = DeviceManager.Instance.ContentManager.Load<Texture2D>("Levels/Scenery/city");
            //SceneryTextures[3] = DeviceManager.Instance.ContentManager.Load<Texture2D>("Levels/Scenery/lava1");
            //SceneryTextures[4] = DeviceManager.Instance.ContentManager.Load<Texture2D>("Levels/Scenery/lava2");
            TerrainTexture = DeviceManager.Instance.ContentManager.Load<Texture2D>("World/tiles");
            //SpireTexture = DeviceManager.Instance.ContentManager.Load<Texture2D>("Levels/spires");

            ////Calculate Origins
            //foreach (int key in SceneryTypes.Keys)
            //{
            //    SceneryTypes[key].Origin = new Vector2(SceneryTypes[key].Frame.Width / 2, SceneryTypes[key].Frame.Height);
            //}

            ////Load instances of backgrounds
            //Backgrounds[0] = new ForestBackground();
            //Backgrounds[1] = new CityBackground();
            //Backgrounds[2] = new SewerBackground();
            //Backgrounds[3] = new LavaBackground();
            //Backgrounds[4] = new ForestBackground();
        }
    }
}
