#region Using Statements
using System;

using Microsoft.Xna.Framework;

using Engine.Logic.Actors;
#endregion

namespace Engine.Logic.ClassComponents
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 7.25.2010
    /// Description: Common Component
    /// </summary>
    #region Enum
    public enum ComponentType
    {
        UNKNOWN = 0,
        BOUNDING,
        GRAVITY,
        POSITION,
        SPRITE,
        END
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
        private Actor owner;
        #endregion 

        #region Properties
        /// <summary>The actor owning this component</summary>
        public Actor Owner { get { return this.owner; } }
        #endregion

        #region Constructors
        /// <summary>A object to give properties and methods to an actor</summary>
        /// <param name="a">The actor the object belongs to</param>
        public ClassComponent(Actor owner)
        {
            this.owner = owner;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Removes the owner reference
        /// </summary>
        public virtual void Dispose()
        {
            this.owner = null;
        }

        /// <summary>
        /// Update Component Data
        /// </summary>
        /// <param name="spriteBatch">Current Gametime</param>
        public abstract void Update(GameTime gametime);
        #endregion
    }
}