#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Logic.Actors;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.07.2010
    /// Description: Bounding Box Component
    /// </summary>
    public class BoundingComponent : ClassComponent
    {
        #region Fields
        private Point boxDimension;
        private Point hitDimension;
        private BoundingBox box;
        private BoundingBox hit;
        #endregion

        #region Properties
        /// <summary> The bounding box</summary>
        public BoundingBox Box { get { return this.box; } set { this.box = value; } }
        #endregion

        #region Constructors
        /// <summary>The component used to check collisions</summary>
        /// <param name="actorRef">The actor the object belongs to</param>
        /// <param name="actorId">The actor ID</param>
        /// <param name="rect">The bounding box</param>
        public BoundingComponent(Actor actorRef, Point boxDim, Point hitDim)
            : base(actorRef)
        {
            this.boxDimension = boxDim;
            this.hitDimension = hitDim;

            Update(null);
        }
        #endregion

        #region Public Methods
        public override void Update(GameTime gameTime)
        {
            Vector2 offset = Vector2.Zero;
            if (this.ActorRef.GetSprite() != null)
                offset = this.ActorRef.GetSprite().Offset;
            Vector2 pos = this.ActorRef.GetPosition().Position + offset;
            pos.X -= this.boxDimension.X / 2.0f;
            pos.Y -= this.boxDimension.Y;
            this.box = new BoundingBox(new Vector3(pos.X, pos.Y, 0), new Vector3(pos.X + this.boxDimension.X, pos.Y + this.boxDimension.Y, 0));

            pos = this.ActorRef.GetPosition().Position + offset;
            pos.X -= this.hitDimension.X / 2.0f;
            pos.Y -= this.hitDimension.Y;
            this.hit = new BoundingBox(new Vector3(pos.X, pos.Y, 0), new Vector3(pos.X + this.hitDimension.X, pos.Y + this.hitDimension.Y, 0));
        }

        public bool DoesCollid(Actor actor)
        {
            if (actor.GetBounding() != null)
            {
               if (actor.GetBounding().Box.Intersects(this.Box))
               {
                   return true;
               }
            }

            return false;
        }
        #endregion
    }
}
