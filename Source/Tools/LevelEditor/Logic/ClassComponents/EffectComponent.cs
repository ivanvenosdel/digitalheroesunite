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
    /// Creation: 10.12.2010
    /// Description: Effect Component
    /// </summary>
    public class EffectComponent : ClassComponent
    {
        #region Fields
        private bool enabled;
        #endregion

        #region Properties
        public bool Enabled { get { return this.enabled; } set { this.enabled = value; } }
        #endregion

        #region Constructors
        /// <summary>The visual effects component</summary>
        /// <param name="actorRef">The actor the object belongs to</param>
        public EffectComponent(Actor actorRef)
            : base(actorRef)
        {
            this.enabled = true;
        }
        #endregion

        #region Public Methods
        public override void Update(GameTime gameTime)
        {
            if (!enabled)
                return;
        }

        public Color GetEffectColor()
        {
            Color returnColor = Color.White;

            return returnColor;
        }
        #endregion
    }
}
