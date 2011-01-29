#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonsterEscape.Logic.Actors;
using MonsterEscape.Logic.AI;
#endregion

namespace MonsterEscape.Worlds
{
    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.27.2010
    /// Description: Generic Holder used to indicate where
    /// an actor will spawn at the start of a level.
    /// </summary>
    public class EntityMarker
    {
        public ActorType Type;
        public Point Position;

        public EntityMarker() { }

        public EntityMarker(ActorType t, Point pos)
        {
            Type = t;
            Position = pos;
        }
    }

    /// <summary>
    /// Authors: James Kirk
    /// Creation: 9.27.2010
    /// Description: Extends the generic marker to include an ai configuration
    /// </summary>
    public class EntityMarkerEnemy : EntityMarker
    {
        public AIConfiguration AIConfiguration;

        public EntityMarkerEnemy() { }

        public EntityMarkerEnemy(ActorType t, Point pos, AIConfiguration config)
        : base(t, pos)
        {
            AIConfiguration = config;
        }
    }
}
