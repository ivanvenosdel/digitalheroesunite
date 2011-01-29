#region Using Statements
using System;

using Microsoft.Xna.Framework;

using MonsterEscape.Logic.Actors;
#endregion

namespace MonsterEscape.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.9.2007
    /// Description: Common Component
    /// </summary>
    #region Enum
    public enum ComponentType
    {
        /// <summary>Unknown Type</summary>
        UNKNOWN = 0,
        /// <summary>AI Type</summary>
        AI,
        /// <summary>Movement Type</summary>
        ATTRIBUTE,
        /// <summary>Bounding Type</summary>
        BOUNDING,
        /// <summary>Effect Type</summary>
        EFFECT,
        /// <summary>Pathfind Type</summary>
        PATHFIND,
        /// <summary>Position Type</summary>
        POSITION,
        /// <summary>Sprite Type</summary>
        SPRITE
    };
    #endregion

    /// <summary>
    /// Authors: James Kirk
    /// Creation: 5.6.2007
    /// Description: Base Component Object
    /// </summary>
    public abstract class ClassComponent
    {
        #region Fields
        /// <summary>The actor owning this component</summary>
        private Actor actorRef;
        #endregion 

        #region Properties
        /// <summary>The actor owning this component</summary>
        public Actor ActorRef { get { return this.actorRef; } }
        #endregion

        #region Constructors
        /// <summary>A object to give properties and methods to an actor</summary>
        /// <param name="actorRef">The actor the object belongs to</param>
        public ClassComponent(Actor actorRef)
        {
            this.actorRef = actorRef;
        }

        public abstract void Update(GameTime gameTime);
        #endregion
    }
}