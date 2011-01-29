#region Using Statements
using System;
using System.Collections.Generic;
#endregion

namespace MonsterEscape.Graphics.Animations
{
    public class BasicAnimPackage
    {
        public const int WALK_LEFT = 0;
        public const int WALK_RIGHT = 1;
        public const int WALK_UP = 2;
        public const int WALK_DOWN = 3;
        public const int IDLE = 4;
        public const int HURT = 5;
        public const int DIE = 6;
    }

    public class EggCarrierAnimPackage : BasicAnimPackage
    {
        public const int WALK_LEFT_EGG = 7;
        public const int WALK_RIGHT_EGG = 8;
        public const int WALK_UP_EGG = 9;
        public const int WALK_DOWN_EGG = 10;
        public const int IDLE_EGG = 11;
    }

    public class MonsterTangyAnimPackage : EggCarrierAnimPackage
    {
        public const int HAPPY = 12;
    }

    public class MonsterPaunchAnimPackage : MonsterTangyAnimPackage
    {
        public const int EAT_LEFT = 13;
        public const int EAT_RIGHT = 14;
        public const int EAT_UP = 15;
        public const int EAT_DOWN = 16;
        public const int EAT_LEFT_EGG = 17;
        public const int EAT_RIGHT_EGG = 18;
        public const int EAT_UP_EGG = 19;
        public const int EAT_DOWN_EGG = 20;
    }

    public class MonsterNimbletonAnimPackage : MonsterTangyAnimPackage
    {
        public const int JUMP_LEFT = 13;
        public const int JUMP_RIGHT = 14;
        public const int JUMP_UP = 15;
        public const int JUMP_DOWN = 16;
        public const int JUMP_LEFT_EGG = 17;
        public const int JUMP_RIGHT_EGG = 18;
        public const int JUMP_UP_EGG = 19;
        public const int JUMP_DOWN_EGG = 20;
    }

    public class MonsterCyAnimPackage : MonsterTangyAnimPackage
    {
        public const int SMASH_LEFT = 13;
        public const int SMASH_RIGHT = 14;
        public const int SMASH_UP = 15;
        public const int SMASH_DOWN = 16;
        public const int SMASH_LEFT_EGG = 17;
        public const int SMASH_RIGHT_EGG = 18;
        public const int SMASH_UP_EGG = 19;
        public const int SMASH_DOWN_EGG = 20;
    }

    public class SpitterAnimPackage : BasicAnimPackage
    {
        public const int SPIT_LEFT = 7;
        public const int SPIT_RIGHT = 8;
        public const int SPIT_UP = 9;
        public const int SPIT_DOWN = 10;
    }

    public class ObstacleAnimPackage
    {
        public const int NORMAL = 0;
        public const int BREAK = 1;
    }
}
