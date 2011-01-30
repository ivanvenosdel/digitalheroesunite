#region Using Statements
using System;

using Microsoft.Xna.Framework;

using Engine.Graphics.Cameras;
using Engine.Logic.Actors;
using Engine.Utilities;
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
        public Vector2 HackPos;
        public Vector2 Position
        {
            get {
                if (this.Owner.GetBounding() != null)
                    return this.Owner.GetBounding().Fixture.Body.Position;
                else
                    return HackPos;
            }
            set {
                if (this.Owner.GetBounding() != null)
                    this.Owner.GetBounding().Fixture.Body.Position = UtilityGame.GameToPhysics(value);
                else
                    HackPos = value;
            }
        }

        #endregion

        #region Constructors
        /// <summary>The component used to position and orientate objects in our game</summary>
        /// <param name="owner">The actor the object belongs to</param>
        public PositionComponent(Actor owner, Vector2 pos)
            : base(owner)
        {
            this.HackPos = pos;
        }
        #endregion

        #region Public Methods
        public override void  Update(GameTime gametime)
        {
 	        //Update the position to match our Fixture
            //if (this.Owner.GetBounding() != null)
            //    this.Position = this.Owner.GetBounding().Fixture.Body.Position;
        }
        #endregion
    }
}
