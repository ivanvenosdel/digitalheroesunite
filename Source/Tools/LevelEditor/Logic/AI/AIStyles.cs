using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterEscape.Logic.AI
{
    public enum AIDirection
    {
        NONE,
        DIRECTIONAL,
        BOX,
        RANDOM
    }

    public enum AIBehavior
    {
        NONE,
        SLEEPER,
        SPITTER
    }
}
