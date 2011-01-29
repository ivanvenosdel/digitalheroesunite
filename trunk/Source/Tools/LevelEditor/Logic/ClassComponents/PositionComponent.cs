#region Using Statements
using System;

using Microsoft.Xna.Framework;

using MonsterEscape.Logic.Actors;
#endregion

namespace MonsterEscape.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 1.25.2009
    /// Description: Position Component
    /// </summary>
    public class PositionComponent : ClassComponent
    {
        #region Fields
        private Vector2 position;
        #endregion

        #region Properties
        /// <summary> The position </summary>
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }
        #endregion

        #region Constructors
        /// <summary>The component used to position and orientate objects in our game</summary>
        /// <param name="actorRef">The actor the object belongs to</param>
        public PositionComponent(Actor actorRef)
            : base(actorRef)
        {
            this.position = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            //Nothing
        }
        #endregion
    }
}
