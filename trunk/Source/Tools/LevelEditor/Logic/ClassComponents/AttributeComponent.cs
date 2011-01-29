#region Using Statements
using System;

using Microsoft.Xna.Framework;

using MonsterEscape.Logic.Actors;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 10.09.2010
    /// Description: Attribute Component
    /// </summary>
    public class AttributeComponent : ClassComponent
    {
        #region Fields
        private float speed;
        private float health;
        #endregion

        #region Properties
        /// <summary> The speed in tiles per second </summary>
        public float Speed { get { return this.speed; } set { this.speed = value; } }
        /// <summary> The health </summary>
        public float Health { get { return this.health; } set { this.health = value; } }
        #endregion

        #region Constructors
        /// <summary>The component used to position and orientate objects in our game</summary>
        /// <param name="actorRef">The actor the object belongs to</param>
        /// <param name="actorId">The actor ID</param>
        public AttributeComponent(Actor actorRef)
            : base(actorRef)
        {
            this.speed = 1;
        }

        public override void Update(GameTime gameTime)
        {
            //NOTHING
        }
        #endregion
    }
}
