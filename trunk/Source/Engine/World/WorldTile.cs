using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Logic.Actors;
using Engine.Logic.ClassComponents;

namespace Engine.World
{
    public class WorldTile
    {
        public const int TILE_SIZE = 70;

        #region Properties
        public int ID { get; set; }
        public Vector2 Position { get; set; }
        public Point TextureCoordinate { get; set; }
        public BoundingBox Bounding;
        #endregion

        public WorldTile(int id, Vector2 position)
        {
            this.ID = id;
            this.Position = position;

            Vector2 pos = new Vector2(Position.X, Position.Y);
            this.Bounding = new BoundingBox(new Vector3(pos.X, pos.Y, 0), new Vector3(pos.X + WorldTypes.TILE_SIZE, pos.Y + WorldTypes.TILE_SIZE, 0));
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (this.ID == 0)
                return;

            Point framePos = (WorldTypes.TileTypes[this.ID]).TextureCoordinate;
            Rectangle frame = new Rectangle(framePos.X * WorldTypes.TILE_SIZE, framePos.Y * WorldTypes.TILE_SIZE,
                                     WorldTypes.TILE_SIZE, WorldTypes.TILE_SIZE);
            spriteBatch.Draw(WorldTypes.TileTexture, position, frame, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

#if DEBUG
            //If there is a bounding component and debug draw is on, draw the bounding box
            if (Debug.DrawBounding)
            {
                //top
                spriteBatch.Draw(BoundingComponent.BoundingTexture, new Rectangle((int)this.Bounding.Min.X, (int)this.Bounding.Min.Y, WorldTypes.TILE_SIZE, 1), Color.Orange);
                //Right
                spriteBatch.Draw(BoundingComponent.BoundingTexture, new Rectangle((int)this.Bounding.Max.X, (int)this.Bounding.Min.Y, 1, WorldTypes.TILE_SIZE), Color.Yellow);
                //Bottom
                spriteBatch.Draw(BoundingComponent.BoundingTexture, new Rectangle((int)this.Bounding.Min.X, (int)this.Bounding.Max.Y, WorldTypes.TILE_SIZE, 1), Color.Red);
                //Left
                spriteBatch.Draw(BoundingComponent.BoundingTexture, new Rectangle((int)this.Bounding.Min.X, (int)this.Bounding.Min.Y, 1, WorldTypes.TILE_SIZE), Color.Green);
            }
#endif
        }
    }
}
