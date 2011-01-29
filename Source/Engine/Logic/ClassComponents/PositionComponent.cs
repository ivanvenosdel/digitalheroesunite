#region Using Statements
using System;

using Microsoft.Xna.Framework;

using Engine.Graphics.Cameras;
using Engine.Logic.Actors;
#endregion

namespace Engine.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 1.25.2009
    /// Description: Position Component
    /// </summary>
    public class PositionComponent : ClassComponent
    {
        #region Fields
        public Vector2 Position;
        #endregion

        #region Constructors
        /// <summary>The component used to position and orientate objects in our game</summary>
        /// <param name="owner">The actor the object belongs to</param>
        public PositionComponent(Actor owner, Vector2 pos)
            : base(owner)
        {
            this.Position = pos;
        }
        #endregion

        #region Public Methods
        public override void  Update(GameTime gametime)
        {
 	        
        }
        #endregion
    }
}
