#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.AI;
using MonsterEscape.Utility;
using MonsterEscape.Worlds;
#endregion

namespace MonsterEscape.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 8.08.2010
    /// Description: Pathfind Component
    /// </summary>
    public class PathfindComponent : ClassComponent
    {
        #region Fields
        private WalkerType walkerType;
        #endregion

        #region Properties
        /// <summary>The Actor's walker type</summary>
        public WalkerType Walker { get { return this.walkerType; } }
        #endregion

        #region Constructors
        /// <summary>The component used to pathfind</summary>
        /// <param name="actorRef">The actor the object belongs to</param>
        /// <param name="actorId">The actor ID</param>
        public PathfindComponent(Actor actorRef, WalkerType walker)
            : base(actorRef)
        {
            this.walkerType = walker;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Calculate Pathfind
        /// </summary>
        /// <param name="gameTime">Current Game Time</param>
        public override void Update(GameTime gameTime)
        {
            //EMPTY
        }
        #endregion
    }
}
