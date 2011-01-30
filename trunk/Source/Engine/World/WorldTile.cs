using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

using Engine.Logic.Actors;
using Engine.Logic.ClassComponents;
using Engine.Utilities;

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
        public Fixture fixture;
        #endregion

        public WorldTile(int id, Vector2 position)
        {
            this.ID = id;
            this.Position = position;

            this.Bounding = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3(position.X + WorldTypes.TILE_SIZE, position.Y + WorldTypes.TILE_SIZE, 0));

            if (this.ID == 0)
                return;

            Vector2 dim = UtilityGame.GameToPhysics(new Vector2(WorldTypes.TILE_SIZE, WorldTypes.TILE_SIZE));
            Vector2 pos = UtilityGame.GameToPhysics(new Vector2(this.Position.X + WorldTypes.TILE_SIZE/2, this.Position.Y + WorldTypes.TILE_SIZE/2));
            Fixture fixture = FixtureFactory.CreateRectangle(DeviceManager.Instance.Physics.WorldSimulation, dim.X, dim.Y, 3, pos);
            fixture.Body.BodyType = BodyType.Static;
            fixture.CollisionCategories = CollisionCategory.Cat1;
            unchecked
            {
                fixture.CollisionGroup = (short)(CollisionCategory.All & ~CollisionCategory.Cat1);
            }
            this.fixture = fixture;
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
