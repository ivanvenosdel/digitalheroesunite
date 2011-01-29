#region Using Statements
using System;

using Microsoft.Xna.Framework;
#endregion

namespace MonsterEscape.Logic.Actors.Items
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.07.2010 ~ 9.27.2010
    /// Description: Abstract Powerup Actor
    /// </summary>
    public abstract class Powerup : Actor
    {

        #region Constructor
        public Powerup(Guid id, ActorType type)
            : base(id, type)
        {

        }

        public override abstract void Update(GameTime gametime);

        public abstract void Use(Actor user);
        #endregion
    }
}
