#region Using Statements
using System;

using Microsoft.Xna.Framework;

using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.AI;
using MonsterEscape.Worlds;
#endregion


namespace MonsterEscape.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.07.2010
    /// Description: AI Component
    /// </summary>
    public class AIComponent : ClassComponent
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>The component used to give actors intelligence</summary>
        /// <param name="actorRef">The actor the object belongs to</param>
        /// <param name="actorId">The actor ID</param>
        public AIComponent(Actor actorRef, AIConfiguration config)
            : base(actorRef)
        {
        }

        public override void Update(GameTime gameTime)
        {
            //Nothing
        }
        #endregion
    }
}
