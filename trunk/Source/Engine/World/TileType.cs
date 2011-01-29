using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.World
{
    public class TileType
    {
        #region Properties
        public int ID;
        public Point TextureCoordinate;
        #endregion

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (this.ID == 0)
                return;

            Point framePos = (WorldTypes.TileTypes[this.ID]).TextureCoordinate;
            Rectangle frame = new Rectangle(framePos.X * WorldTile.TILE_SIZE, framePos.Y * WorldTile.TILE_SIZE,
                                     WorldTile.TILE_SIZE, WorldTile.TILE_SIZE);
            spriteBatch.Draw(WorldTypes.TileTexture, position, frame, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
