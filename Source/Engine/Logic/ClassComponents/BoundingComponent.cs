#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine.Logic.Actors;
using Engine.Graphics.Cameras;
using Engine.World;
using Engine.Utilities;
#endregion

namespace Engine.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.07.2010
    /// Description: Bounding Box Component
    /// </summary>
    public class BoundingComponent : ClassComponent
    {
        #region Fields
#if DEBUG
        public static Texture2D BoundingTexture = null;
#endif
        private Point boxDimension;
        private BoundingBox box;
        #endregion

        #region Properties
        /// <summary>The bounding box</summary>
        public BoundingBox Box { get { return this.box; } set { this.box = value; } }
        #endregion

        #region Constructors
        /// <summary>The component used to check collisions</summary>
        /// <param name="owner">The actor the object belongs to</param>
        /// <param name="actorId">The actor ID</param>
        /// <param name="boxDim">The bounding box</param>
        /// <param name="hitDim">The hit box</param>
        public BoundingComponent(Actor owner, Point boxDim)
            : base(owner)
        {
            this.boxDimension = boxDim;

            Update(null);
        }
        #endregion

        #region Public Methods
        public override void Update(GameTime gameTime)
        {
            Vector2 offset = Vector2.Zero;
            if (this.Owner.GetSprite() != null)
                offset = this.Owner.GetSprite().Offset;
            Vector2 pos = this.Owner.GetPosition().Position + offset;
            pos.X -= this.boxDimension.X / 2.0f;
            pos.Y -= this.boxDimension.Y;
            this.box = new BoundingBox(new Vector3(pos.X, pos.Y, 0), new Vector3(pos.X + this.boxDimension.X, pos.Y + this.boxDimension.Y, 0));

            CheckCollisions();
        }

        public void CheckCollisions()
        {
            if (Camera.Instance.OnScreen(new Point((int)Owner.GetPosition().Position.X, (int)Owner.GetPosition().Position.Y)))
            {
                //Make sure the player isnt' falling through a tile
                Point gridPoint = UtilityWorld.WorldToGrid(Owner.GetPosition().Position);
                //Make sure our points are within the world bounds
                gridPoint.X = Math.Max(Math.Min(gridPoint.X, GameWorld.Instance.width - 1), 0);
                gridPoint.Y = Math.Max(Math.Min(gridPoint.Y, GameWorld.Instance.height - 1), 0);

                for (int y = gridPoint.Y - 1; y <= gridPoint.Y + 1; ++y)
                {
                    for (int x = gridPoint.X - 1; x <= gridPoint.Y + 1; ++x)
                    {
                        if (x < 0 || x >= GameWorld.Instance.width ||
                            y < 0 || y >= GameWorld.Instance.height)
                            continue;

                       WorldTile tile = GameWorld.Instance.layout[x + y * GameWorld.Instance.width];
                       //Collison
                       if (tile.ID != 0 && DoesCollid(tile.Bounding))
                       {
                           //Push the monster out of the tile
                           if (x < gridPoint.X)
                               Owner.GetPosition().Position.X += GravityComponent.GRAVITY + GravityComponent.GRAVITY / 2;
                           else if (x > gridPoint.X)
                               Owner.GetPosition().Position.X -= GravityComponent.GRAVITY + GravityComponent.GRAVITY / 2;
                           if (y < gridPoint.Y)
                               Owner.GetPosition().Position.Y += GravityComponent.GRAVITY + GravityComponent.GRAVITY / 2;
                           else if (y > gridPoint.Y)
                               Owner.GetPosition().Position.Y -= GravityComponent.GRAVITY + GravityComponent.GRAVITY / 2;
                       }
                    }
                }
            }
        }

        public bool DoesCollid(BoundingBox box)
        {
            //Check collision
            if (box.Contains(this.box) == ContainmentType.Intersects ||
                box.Contains(this.box) == ContainmentType.Contains)
                return true;
            else
                return false;
        }

        public bool DoesCollid(Vector2 worldPos)
        {
            if (this.Box.Contains(new Vector3(worldPos.X, worldPos.Y, 0)) == ContainmentType.Contains)
                return true;
            else
                return false;
        }

        public bool DoesCollid(Actor actor)
        {
            if (actor == this.Owner)
                return false;

            bool collided = false;

            if (actor.GetBounding() != null)
            {
                BoundingBox b = actor.GetBounding().Box;
                if (actor.GetBounding().Box.Contains(this.Box) == ContainmentType.Contains)
                    collided = true;
                else if (actor.GetBounding().Box.Contains(this.Box) == ContainmentType.Intersects)
                    collided = true;
                else if (actor.GetBounding().Box.Intersects(this.Box))
                    collided = true;

                if (collided)
                    HandleCollision(actor);
            }

            return collided;
        }

#if DEBUG
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //top
            spriteBatch.Draw(BoundingComponent.BoundingTexture, new Rectangle((int)this.box.Min.X, (int)this.box.Min.Y, this.boxDimension.X, 1), Color.Yellow);
            //Right
            spriteBatch.Draw(BoundingComponent.BoundingTexture, new Rectangle((int)this.box.Max.X, (int)this.box.Min.Y, 1, this.boxDimension.Y), Color.Yellow);
            //Bottom
            spriteBatch.Draw(BoundingComponent.BoundingTexture, new Rectangle((int)this.box.Min.X, (int)this.box.Max.Y, this.boxDimension.X, 1), Color.Yellow);
            //Left
            spriteBatch.Draw(BoundingComponent.BoundingTexture, new Rectangle((int)this.box.Min.X, (int)this.box.Min.Y, 1, this.boxDimension.Y), Color.Yellow);
        }
#endif
        #endregion

        #region Private Methods
        /// <summary>
        /// The passed in actor has collided with the Owner
        /// </summary>
        /// <param name="actor">Colliding actor</param>
        private void HandleCollision(Actor actor)
        {
            
        }
        #endregion
    }
}
