using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Worlds;
namespace Engine.World
{
    public class WorldTile
    {
        public const int TILE_SIZE = 70;

        #region Properties
        public int ID { get; set; }
        public Point TextureCoordinate { get; set; }
        public Vector2 Offset;
        public BoundingBox Bounding;
        #endregion

        public WorldTile(int id)
        {
            this.ID = id;
            this.Bounding = new BoundingBox();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (this.ID == 0)
                return;

            Point framePos = (TerrainKey.TileTypes[this.ID]).TextureCoordinate;
            Rectangle frame = new Rectangle(framePos.X * TerrainKey.TILE_SIZE, framePos.Y * TerrainKey.TILE_SIZE,
                                     TerrainKey.TILE_SIZE, TerrainKey.TILE_SIZE);
            spriteBatch.Draw(TerrainKey.TerrainTexture, position, frame, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale)
        {
            if (this.ID == 0)
                return;

            Point framePos = (TerrainKey.TileTypes[this.ID]).TextureCoordinate;
            Rectangle frame = new Rectangle(framePos.X * TerrainKey.TILE_SIZE, framePos.Y * TerrainKey.TILE_SIZE,
                                     TerrainKey.TILE_SIZE, TerrainKey.TILE_SIZE);
            spriteBatch.Draw(TerrainKey.TerrainTexture, position, frame, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
        }
    }
}
