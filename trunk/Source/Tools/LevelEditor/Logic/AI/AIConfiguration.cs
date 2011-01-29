#region Using Statements
using System;
using System.Collections.Generic;
#endregion

namespace MonsterEscape.Logic.AI
{
    public class AIConfiguration
    {
        public float Frequency = 0.0f;
        public AIBehavior Behavior = AIBehavior.NONE;
        public bool SpitAim = false;
        public bool SpitGreen = true;
        public AIDirection Direction = AIDirection.NONE;
        public PathDirection DirectionPreference = PathDirection.Unknown;
        public WalkerType Walker = WalkerType.Unknown;
        public float MovementDuration = 0.0f;

        public AIConfiguration() { }
    }
}
